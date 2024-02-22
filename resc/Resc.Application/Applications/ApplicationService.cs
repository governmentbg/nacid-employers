using Microsoft.EntityFrameworkCore;
using Resc.Application.Applications.Dtos.Create;
using Resc.Application.Applications.Dtos.History;
using Resc.Application.Applications.Dtos.Modification;
using Resc.Application.Applications.Dtos.Search;
using Resc.Application.Applications.Interfaces;
using Resc.Application.Common.Constants;
using Resc.Application.Common.Dtos;
using Resc.Application.Common.Extensions;
using Resc.Application.Common.Interfaces;
using Resc.Data.Applications.Register;
using Resc.Data.Common.Enums;
using Resc.Data.Users;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Applications
{
	public class ApplicationService : IApplicationService
	{
		private readonly IAppDbContext context;
		private readonly IUserContext userContext;

		public ApplicationService(
			IAppDbContext context, 
			IUserContext userContext
			)
		{
			this.context = context;
			this.userContext = userContext;
		}

		public async Task<CommitInfoDto> CreateApplication(ApplicationDto model, CancellationToken cancellationToken)
		{
			int? lastLotNumber = await context.Set<ApplicationLot>()
					.MaxAsync(e => (int?)e.LotNumber, cancellationToken);

			var lot = new ApplicationLot {
				LotNumber = (lastLotNumber ?? 0) + 1
			};

			context.Set<ApplicationLot>().Add(lot);
			await context.SaveChangesAsync(cancellationToken);

			var commit = new ApplicationCommit {
				LotId = lot.Id,
				State = model.IsDraft == true ? CommitState.InitialDraft : CommitState.Actual,
				Number = 1,
				StudentPart = new StudentPart {
					Entity = model.Student?.ToModel()
				},
				UniversityPart = new UniversityPart {
					Entity = model.University?.ToModel()
				},
				EmployerPart = new EmployerPart {
					Entity = model.Employer?.ToModel()
				},
				ContractPart = new ContractPart {
					Entity = model.Contract?.ToModel()
				},
				ActualEducationPart = new ActualEducationPart {
					Entity = model.ActualEducation?.ToModel()
				}
			};

			context.Set<ApplicationCommit>().Add(commit);
			await context.SaveChangesAsync(cancellationToken);

			if (commit.State == CommitState.Actual)
			{
				var user = await this.context.Set<User>()
				.SingleOrDefaultAsync(x => x.Id == lot.CreatorUserId, cancellationToken);

				var creatorUser = $"{user.FirstName} {user.LastName}";
				var applicationStatusHistory = new ApplicationStatusHistory(commit.LotId, commit.Id, creatorUser, commit.CreateDate, commit.State, commit.ChangeStateDescription);

				context.Set<ApplicationStatusHistory>().Add(applicationStatusHistory);
				await context.SaveChangesAsync(cancellationToken);
			}

			return new CommitInfoDto {
				LotId = lot.Id,
				CommitId = commit.Id
			};
		}

		public async Task<ApplicationCommitDto> GetApplicationCommit(int lotId, int commitId, CancellationToken cancellationToken)
		{
			var commitsCount = await this.context.Set<ApplicationCommit>()
				.AsNoTracking()
				.CountAsync(x => x.LotId == lotId);

			var commit = await context.Set<ApplicationCommit>()
					.AsNoTracking()
					.Where(e => e.LotId == lotId && e.Id == commitId)
					.Select(ApplicationCommitDto.SelectExpression)
					.SingleOrDefaultAsync(cancellationToken);

			commit.HasOtherCommits = commitsCount > 1;

			return commit;
		}

		public async Task<SearchResultItemDto<ApplicationSearchResultDto>> GetApplicationsFiltered(SearchApplicationFilter filter, CancellationToken cancellationToken)
		{
			var userRole = this.userContext.Role;

			var query = context.Set<ApplicationCommit>()
					.Where(e =>
					e.State == CommitState.InitialDraft ||
					e.State == CommitState.CommitReady ||
					e.State == CommitState.Actual ||
					e.State == CommitState.Modification ||
					e.State == CommitState.Deleted ||
					e.State == CommitState.Entered ||
					e.State == CommitState.EnteredWithChange ||
					e.State == CommitState.EnteredModification ||
					e.State == CommitState.Expired ||
					e.State == CommitState.Terminated);

			if (this.userContext.Role == UserRoleAliases.ADMINISTRATOR || this.userContext.Role == UserRoleAliases.CONTROL_USER)
			{
				query = query.Where(e => e.State != CommitState.InitialDraft && e.State != CommitState.Deleted);
			}
			//else if (this.userContext.Role == UserRoleAliases.CONTROL_USER)
			//{
			//	query = query.Where(e => e.State == CommitState.Entered || e.State == CommitState.EnteredWithChange || e.State == CommitState.Terminated || e.State == CommitState.Expired);
			//}

			if (!string.IsNullOrWhiteSpace(filter.RegisterNumber))
			{
				query = query.Where(e => e.Lot.RegisterNumber.Trim().ToLower().Contains(filter.RegisterNumber.Trim().ToLower()));
			}

			if (!string.IsNullOrWhiteSpace(filter.ContractNumber))
			{
				query = query.Where(e => e.ContractPart.Entity.Number.Trim().ToLower().Contains(filter.ContractNumber.Trim().ToLower()));
			}

			if (filter.SigningDateFrom.HasValue)
			{
				query = query.Where(e => e.ContractPart.Entity.SigningDate >= filter.SigningDateFrom);
			}

			if (filter.SigningDateTo.HasValue)
			{
				query = query.Where(e => e.ContractPart.Entity.SigningDate <= filter.SigningDateTo);
			}

			if (filter.State.HasValue)
			{
				query = query.Where(e => e.State == filter.State);
			}

			if (filter.EmployerListItemId.HasValue)
			{
				query = query.Where(e => e.EmployerPart.Entity.EmployerListItemId == filter.EmployerListItemId);
			}

			if (!string.IsNullOrWhiteSpace(filter.Bulstat))
			{
				query = query.Where(e => e.EmployerPart.Entity.EmployerListItem.Bulstat.Trim().ToLower().Contains(filter.Bulstat.Trim().ToLower()));
			}

			if (!string.IsNullOrWhiteSpace(filter.Institution))
			{
				query = query.Where(e => e.UniversityPart.Entity.Institution.Name.Trim().ToLower().Contains(filter.Institution.Trim().ToLower()));
			}

			if (!string.IsNullOrWhiteSpace(filter.StudentName))
			{
				var names = filter.StudentName
						.Split(" ")
						.Select(e => e.ToLower().Trim())
						.ToList();

				Expression<Func<Student, bool>> namesExpression = ExpressionHelper.BuildOrStringExpression<Student>(nameof(StudentPart.Entity.FullName), names);

				var innerQuery = context.Set<Student>()
						.Where(namesExpression)
						.Select(e => e.Id);
				query = query.Where(e => innerQuery.Contains(e.StudentPart.EntityId));
			}

			if (!string.IsNullOrWhiteSpace(filter.StudentUIN))
			{
				query = query.Where(e => e.StudentPart.Entity.UIN.Trim().ToLower().Contains(filter.StudentUIN.Trim().ToLower()));
			}

			var items = await query
				.Select(ApplicationSearchResultDto.SelectExpression)
				.OrderByDescending(e => e.LotId)
					.ThenBy(e => e.CommitId)
				.Skip(filter.Offset)
				.Take(filter.Limit)
				.ToListAsync(cancellationToken);

			var result = new SearchResultItemDto<ApplicationSearchResultDto> {
				Items = items,
				TotalCount = await query.CountAsync()
			};

			return result;
		}

		public async Task<ApplicationLotHistoryDto> GetApplicationLotHistory(int lotId, CancellationToken cancellationToken)
		{
			var actualCommitId = await this.context.Set<ApplicationCommit>()
				.Where(x => x.LotId == lotId)
				.OrderByDescending(x => x.Id)
				.Select(x => x.Id)
				.FirstAsync();

			var commits = await this.context.Set<ApplicationStatusHistory>()
					.Where(e => e.LotId == lotId)
					.ToListAsync();

			foreach (var commit in commits.Where(x => x.CommitId != null))
			{
				var commitsCount = commits.Count(x => x.CommitId == commit.CommitId);

				if (commitsCount > 1)
				{
					var equalCommits = commits.Where(x => x.CommitId == commit.CommitId).ToList();
					equalCommits[0].CommitId = null;
				}
			}

			var lotHistory = new ApplicationLotHistoryDto();

			foreach (var commit in commits)
			{
				var historyItem = new ApplicationCommitHistoryItemDto {
					CommitId = commit.CommitId,
					LotId = commit.LotId,
					State = commit.CommitState,
					CreateDate = commit.CreateDate,
					ChangeStateDescription = commit.ChangeStateDescription,
					CreatorUser = commit.CreatorUser
				};

				lotHistory.Commits.Add(historyItem);
			}

			lotHistory.Commits = lotHistory.Commits.OrderByDescending(x => x.CreateDate).ToList();
			lotHistory.ActualCommitId = actualCommitId;

			return lotHistory;
		}

		public async Task UpdateApplication(int commitId, ApplicationUpdateDto model, CancellationToken cancellationToken)
		{
			var commit = await this.context.Set<ApplicationCommit>()
				.Include(e => e.UniversityPart.Entity)
				.Include(e => e.EmployerPart.Entity)
				.Include(e => e.StudentPart.Entity)
				.Include(e => e.ContractPart.Entity)
					.ThenInclude(e => e.ContractFile)
				.Include(e => e.ContractPart.Entity)
					.ThenInclude(e => e.Contacts)
				.Include(e => e.ActualEducationPart.Entity)
				.SingleAsync(x => x.Id == commitId, cancellationToken);

			commit.UniversityPart.Entity.Update(model.University.Institution.Id, model.University.SpecialityListItem.Id, model.University.Rector);
			commit.EmployerPart.Entity.Update(model.Employer.EmployerListItem?.Id, model.Employer.Representative, model.Employer.Email, model.Employer.PhoneNumber);
			commit.StudentPart.Entity.Update(model.Student.FirstName, model.Student.MiddleName, model.Student.LastName, model.Student.UIN, model.Student.Email,
				model.Student.PhoneNumber, model.Student.EducationType, model.Student.Status, model.Student.GraduationDate);
			commit.ContractPart.Entity.Update(model.Contract.SigningDate, model.Contract.EndDate, model.Contract.Number, model.Contract.Term, model.Contract.EmploymentTerm,
				model.Contract.TaxType, model.Contract.AttachedFile);
			foreach (var contactPerson in model.Contract.Contacts)
			{
				commit.ContractPart.Entity.UpdateContact(contactPerson.Id, contactPerson.Name, contactPerson.PhoneNumber, contactPerson.Email, contactPerson.Type);
			}
			commit.ActualEducationPart.Entity.Update(model.ActualEducation.EducationalQualification.Id, model.ActualEducation.Status, model.ActualEducation.CourseYear, model.ActualEducation.GraduationDate, model.ActualEducation.EducationType);

			if (model.ChangeStatus)
			{
				commit.State = CommitState.Actual;
				var user = await this.context.Set<User>()
				.SingleOrDefaultAsync(x => x.Id == commit.CreatorUserId, cancellationToken);

				var creatorUser = $"{user.FirstName} {user.LastName}";
				var applicationStatusHistory = new ApplicationStatusHistory(commit.LotId, commit.Id, creatorUser, commit.CreateDate, commit.State, commit.ChangeStateDescription);

				context.Set<ApplicationStatusHistory>().Add(applicationStatusHistory);
				await context.SaveChangesAsync(cancellationToken);
			}

			await this.context.SaveChangesAsync(cancellationToken);
		}
	}
}
