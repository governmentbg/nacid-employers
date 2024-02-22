using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polly;
using Resc.Application.Applications.Dtos.Modification;
using Resc.Application.Applications.Interfaces;
using Resc.Application.Common.Dtos;
using Resc.Application.Common.Extensions;
using Resc.Application.Common.Interfaces;
using Resc.Application.Emails.Interfaces;
using Resc.Application.Ems.Converters;
using Resc.Data.Applications;
using Resc.Data.Applications.Register;
using Resc.Data.Common.Enums;
using Resc.Data.Emails;
using Resc.Data.Nomenclatures;
using Resc.Data.Users;
using Resc.Infrastructure.Ems;
using Resc.Infrastructure.Ems.Enums;
using Resc.Infrastructure.Ems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Applications
{
	public class ApplicationModificationService : IApplicationModificationService
	{
		const string RegisterIndexAlias = "ApplicationRegisterIndex";
		const string ElectornicServiceUri = "Resc";

		private readonly IAppDbContext context;
		private readonly IEmailService emailService;
		private readonly IUserContext userContext;
		private readonly IEmsApplicationConverter converter;
		private readonly EmsService emsService;

		public ApplicationModificationService(
			IAppDbContext context,
			IEmailService emailService,
			IUserContext userContext,
			IEmsApplicationConverter converter,
			EmsService emsService
			)
		{
			this.context = context;
			this.emailService = emailService;
			this.userContext = userContext;
			this.converter = converter;
			this.emsService = emsService;
		}

		public async Task<CommitInfoDto> StartModification(int lotId, string changeStateDescription, CancellationToken cancellationToken)
		{
			var actualCommit = await context.Set<ApplicationCommit>()
				.Include(a => a.StudentPart)
				.Include(a => a.UniversityPart)
				.Include(a => a.EmployerPart)
				.Include(a => a.ContractPart)
				.Include(a => a.ActualEducationPart)
				.SingleAsync(e => e.LotId == lotId && e.State == CommitState.Actual, cancellationToken);

			actualCommit.State = CommitState.ActualWithModification;

			var modificationCommit = (ApplicationCommit)Activator.CreateInstance(typeof(ApplicationCommit), actualCommit);
			modificationCommit.State = CommitState.Modification;
			modificationCommit.ChangeStateDescription = changeStateDescription;

			context.Set<ApplicationCommit>().Add(modificationCommit);

			await context.SaveChangesAsync(cancellationToken);

			await this.AddApplicationStatusHistory(modificationCommit.LotId, null, modificationCommit.CreatorUserId, modificationCommit.State, modificationCommit.ChangeStateDescription, cancellationToken);

			await context.SaveChangesAsync(cancellationToken);

			return new CommitInfoDto {
				LotId = modificationCommit.LotId,
				CommitId = modificationCommit.Id
			};
		}

		public async Task<CommitInfoDto> FinishCommitModification(int lotId, CancellationToken cancellationToken)
		{
			var lot = await context.Set<ApplicationLot>()
				.Include(e => e.Commits)
				.SingleAsync(e => e.Id == lotId, cancellationToken);

			var currentCommit = lot.Commits.Single(e => e.State == CommitState.Modification || e.State == CommitState.InitialDraft || e.State == CommitState.CommitReady);

			currentCommit.State = CommitState.Actual;

			var previousActualCommit = lot.Commits.OrderByDescending(e => e.Id).FirstOrDefault(e => e.State == CommitState.ActualWithModification);
			if (previousActualCommit != null)
			{
				previousActualCommit.State = CommitState.History;
				previousActualCommit.ChangeStateDescription = currentCommit.ChangeStateDescription;
			}

			currentCommit.Number = previousActualCommit?.Number + 1 ?? 1;
			currentCommit.ChangeStateDescription = "";

			await this.AddApplicationStatusHistory(currentCommit.LotId, currentCommit.Id, this.userContext.UserId, currentCommit.State, currentCommit.ChangeStateDescription, cancellationToken);

			await context.SaveChangesAsync(cancellationToken);

			return new CommitInfoDto {
				LotId = currentCommit.LotId,
				CommitId = currentCommit.Id
			};
		}

		public async Task<CommitInfoDto> CancelModification(int lotId, CancellationToken cancellationToken)
		{
			var modificationCommit = await context.Set<ApplicationCommit>()
					.Include(e => e.StudentPart)
							.ThenInclude(s => s.Entity)
					.Include(e => e.UniversityPart)
							.ThenInclude(u => u.Entity)
					.Include(e => e.EmployerPart)
							.ThenInclude(em => em.Entity)
					.Include(e => e.ContractPart)
							.ThenInclude(c => c.Entity)
					.Include(e => e.ActualEducationPart)
							.ThenInclude(c => c.Entity)
					.SingleAsync(e => e.LotId == lotId && e.State == CommitState.Modification, cancellationToken);

			context.Set<ApplicationCommit>().Remove(modificationCommit);

			if (modificationCommit.StudentPart.State == PartState.Modified)
			{
				context.Set<Student>().Remove(modificationCommit.StudentPart.Entity);
			}
			context.Set<StudentPart>().Remove(modificationCommit.StudentPart);

			if (modificationCommit.UniversityPart.State == PartState.Modified)
			{
				context.Set<University>().Remove(modificationCommit.UniversityPart.Entity);
			}
			context.Set<UniversityPart>().Remove(modificationCommit.UniversityPart);

			if (modificationCommit.EmployerPart.State == PartState.Modified)
			{
				context.Set<Employer>().Remove(modificationCommit.EmployerPart.Entity);
			}
			context.Set<EmployerPart>().Remove(modificationCommit.EmployerPart);

			if (modificationCommit.ContractPart.State == PartState.Modified)
			{
				context.Set<Contract>().Remove(modificationCommit.ContractPart.Entity);
			}

			if (modificationCommit.ActualEducationPart.State == PartState.Modified)
			{
				context.Set<ActualEducation>().Remove(modificationCommit.ActualEducationPart.Entity);
			}
			context.Set<ActualEducationPart>().Remove(modificationCommit.ActualEducationPart);

			var actualWithModificationCommit = await context.Set<ApplicationCommit>()
				.SingleAsync(e => e.LotId == lotId && e.State == CommitState.ActualWithModification);

			actualWithModificationCommit.State = CommitState.Actual;

			await context.SaveChangesAsync(cancellationToken);

			return new CommitInfoDto {
				LotId = actualWithModificationCommit.LotId,
				CommitId = actualWithModificationCommit.Id
			};
		}

		public async Task<CommitInfoDto> EraseApplication(int lotId, string changeStateDescription, CancellationToken cancellationToken)
		{
			var actualCommit = await this.context.Set<ApplicationCommit>()
				.SingleAsync(e => e.LotId == lotId && (e.State == CommitState.Actual || e.State == CommitState.Modification || e.State == CommitState.InitialDraft), cancellationToken);
			actualCommit.State = CommitState.Deleted;
			actualCommit.ChangeStateDescription = changeStateDescription;

			await this.AddApplicationStatusHistory(actualCommit.LotId, actualCommit.Id, actualCommit.CreatorUserId, actualCommit.State, actualCommit.ChangeStateDescription, cancellationToken);

			await this.context.SaveChangesAsync(cancellationToken);

			return new CommitInfoDto {
				LotId = actualCommit.LotId,
				CommitId = actualCommit.Id
			};
		}

		public async Task<CommitInfoDto> RevertErasedApplication(int lotId, CancellationToken cancellationToken)
		{
			var actualCommit = await this.context.Set<ApplicationCommit>()
				.SingleAsync(e => e.LotId == lotId && e.State == CommitState.Deleted, cancellationToken);
			actualCommit.State = CommitState.InitialDraft;
			actualCommit.ChangeStateDescription = "";

			await this.context.SaveChangesAsync(cancellationToken);

			return new CommitInfoDto {
				LotId = actualCommit.LotId,
				CommitId = actualCommit.Id
			};
		}

		public async Task<CommitInfoDto> EnterApplication(int lotId, CancellationToken cancellationToken)
		{
			var actualCommit = await this.context.Set<ApplicationCommit>()
				.SingleAsync(e => e.LotId == lotId && e.State == CommitState.Actual, cancellationToken);
			actualCommit.State = CommitState.Entered;
			actualCommit.CreateDate = DateTime.Now;

			await this.AddApplicationStatusHistory(actualCommit.LotId, null, this.userContext.UserId, actualCommit.State, actualCommit.ChangeStateDescription, cancellationToken);

			var lot = await this.context.Set<ApplicationLot>()
				.SingleAsync(x => x.Id == lotId);

			if (string.IsNullOrWhiteSpace(lot.RegisterNumber))
			{
				var registerIndexCounter = await this.context.Set<RegisterIndexCounter>()
					.AsNoTracking()
					.Include(e => e.RegisterIndex)
					.SingleAsync(e => e.RegisterIndex.Alias == RegisterIndexAlias && e.Year == DateTime.Now.Year, cancellationToken);

				string query = $"update {nameof(RegisterIndexCounter).ToLower()} set {nameof(RegisterIndexCounter.Counter).ToLower()} = {nameof(RegisterIndexCounter.Counter).ToLower()} + 1 where id = @id returning {nameof(RegisterIndexCounter.Counter).ToLower()}";
				var queryParams = new Dictionary<string, object>() {
					{"id", registerIndexCounter.Id }
				};

				int registerIndexCount = await this.context.ExecuteRawSqlScalarAsync<int>(query, queryParams);
				lot.RegisterNumber = string.Format(registerIndexCounter.RegisterIndex.Format, registerIndexCount, DateTime.Now.Date);
			}

			await this.context.SaveChangesAsync(cancellationToken);

			EmsApplication emsApplication = this.converter.ToEmsApplication(ElectornicServiceUri, actualCommit.UniversityPart, lot.RegisterNumber, null, false);
			var pendingEmsApplication = new {
				Application = JsonConvert.SerializeObject(emsApplication),
				Status = EmsIncomingDocStatus.Pending
			};

			var docGuid = await emsService.SubmitApplicationAsync(JsonConvert.SerializeObject(pendingEmsApplication));
			var response = await Policy
				.HandleResult<EmsDocStatusResponse>(e => e.Status == EmsIncomingDocStatus.Pending)
				.WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
				.ExecuteAsync(async () => await emsService.GetEmsApplicationStatus(docGuid))
			;


			return new CommitInfoDto {
				LotId = actualCommit.LotId,
				CommitId = actualCommit.Id
			};
		}

		public async Task<CommitInfoDto> StartModificationEnteredApplication(int lotId, string changeStateDescription, CancellationToken cancellationToken)
		{
			var actualCommit = await context.Set<ApplicationCommit>()
				.Include(a => a.StudentPart)
				.Include(a => a.UniversityPart)
				.Include(a => a.EmployerPart)
				.Include(a => a.ContractPart)
				.Include(a => a.ActualEducationPart)
				.SingleOrDefaultAsync(e => e.LotId == lotId && (e.State == CommitState.Entered || e.State == CommitState.EnteredWithChange), cancellationToken);

			actualCommit.State = CommitState.EnteredWithModification;

			var modificationCommit = (ApplicationCommit)Activator.CreateInstance(typeof(ApplicationCommit), actualCommit);
			modificationCommit.State = CommitState.EnteredModification;
			modificationCommit.ChangeStateDescription = changeStateDescription;

			context.Set<ApplicationCommit>().Add(modificationCommit);

			await context.SaveChangesAsync(cancellationToken);

			await this.AddApplicationStatusHistory(modificationCommit.LotId, modificationCommit.Id, this.userContext.UserId, modificationCommit.State, modificationCommit.ChangeStateDescription, cancellationToken);

			await context.SaveChangesAsync(cancellationToken);

			return new CommitInfoDto {
				LotId = modificationCommit.LotId,
				CommitId = modificationCommit.Id
			};
		}

		public async Task<CommitInfoDto> FinishEnteredModification(int lotId, CancellationToken cancellationToken)
		{
			var lot = await context.Set<ApplicationLot>()
				.Include(e => e.Commits)
				.SingleAsync(e => e.Id == lotId, cancellationToken);

			var currentCommit = lot.Commits.Single(e => e.State == CommitState.EnteredModification);

			currentCommit.State = CommitState.Entered;

			var previousActualCommit = lot.Commits.OrderByDescending(e => e.Id).FirstOrDefault(e => e.State == CommitState.EnteredWithModification);
			if (previousActualCommit != null)
			{
				previousActualCommit.State = CommitState.History;
				previousActualCommit.ChangeStateDescription = currentCommit.ChangeStateDescription;
			}

			currentCommit.Number = previousActualCommit?.Number + 1 ?? 1;
			currentCommit.ChangeStateDescription = "";

			await this.AddApplicationStatusHistory(currentCommit.LotId, null, this.userContext.UserId, currentCommit.State, currentCommit.ChangeStateDescription, cancellationToken);

			await context.SaveChangesAsync(cancellationToken);

			return new CommitInfoDto {
				LotId = currentCommit.LotId,
				CommitId = currentCommit.Id
			};
		}

		public async Task SendEmail(string alias, object payload, int userId, CancellationToken cancellationToken)
		{
			var userEmail = await this.context.Set<User>()
				.Where(x => x.Id == userId)
				.Select(x => x.Email)
				.SingleOrDefaultAsync(cancellationToken);

			//some exception must be thrown
			if (userEmail == null)
			{
				return;
			}

			Email email = await this.emailService.ComposeEmailAsync(alias, payload, userEmail);
			this.context.Set<Email>().Add(email);

			await this.context.SaveChangesAsync(cancellationToken);
		}

		public async Task<CommitInfoDto> ChangeEnteredContract(int lotId, ApplicationModificationDto dto, CancellationToken cancellationToken)
		{
			var commit = await this.context.Set<ApplicationCommit>()
				.Include(a => a.StudentPart)
				.Include(a => a.UniversityPart)
				.Include(a => a.EmployerPart)
				.Include(a => a.ContractPart)
				.Include(a => a.ActualEducationPart)
				.Include(a => a.ApplicationModification)
					.ThenInclude(e => e.AnnexFile)
				.SingleAsync(a => a.LotId == lotId && (a.State == CommitState.Entered || a.State == CommitState.EnteredWithChange), cancellationToken);
			commit.State = CommitState.History;

			var modificationCommit = (ApplicationCommit)Activator.CreateInstance(typeof(ApplicationCommit), commit);
			modificationCommit.State = CommitState.EnteredWithChange;
			modificationCommit.AddApplicationModification(dto.ModificationDate, dto.Reason, dto.AnnexFile);

			this.context.Set<ApplicationCommit>().Add(modificationCommit);
			await this.context.SaveChangesAsync(cancellationToken);

			await this.AddApplicationStatusHistory(modificationCommit.LotId, modificationCommit.Id, this.userContext.UserId, modificationCommit.State, modificationCommit.ChangeStateDescription, cancellationToken);

			await this.context.SaveChangesAsync(cancellationToken);

			return new CommitInfoDto {
				CommitId = modificationCommit.Id,
				LotId = lotId
			};
		}

		public async Task<CommitInfoDto> TerminateContract(int lotId, ApplicationTerminationDto dto, CancellationToken cancellationToken)
		{
			var commit = await this.context.Set<ApplicationCommit>()
				.Include(a => a.StudentPart)
				.Include(a => a.UniversityPart)
				.Include(a => a.EmployerPart)
				.Include(a => a.ContractPart)
				.Include(a => a.ActualEducationPart)
				.Include(a => a.ApplicationModification)
					.ThenInclude(e => e.AnnexFile)
				.SingleAsync(x => x.LotId == lotId && (x.State == CommitState.Entered || x.State == CommitState.EnteredWithChange), cancellationToken);
			commit.State = CommitState.EnteredWithModification;

			var modificationCommit = (ApplicationCommit)Activator.CreateInstance(typeof(ApplicationCommit), commit);
			modificationCommit.State = CommitState.Terminated;

			modificationCommit.AddApplicationTermination(dto.TerminationDate, dto.TerminationReason.Id, dto.AnnexFile);

			this.context.Set<ApplicationCommit>().Add(modificationCommit);
			await this.context.SaveChangesAsync(cancellationToken);

			await this.AddApplicationStatusHistory(modificationCommit.LotId, modificationCommit.Id, this.userContext.UserId, modificationCommit.State, modificationCommit.ChangeStateDescription, cancellationToken);

			await this.context.SaveChangesAsync(cancellationToken);

			return new CommitInfoDto {
				CommitId = modificationCommit.Id,
				LotId = lotId
			};
		}

		public async Task DeleteDraft(int lotId, CancellationToken cancellationToken)
		{
			var commit = await this.context.Set<ApplicationCommit>()
				.SingleOrDefaultAsync(x => x.LotId == lotId && x.State == CommitState.InitialDraft, cancellationToken);

			if (commit != null)
			{
				this.context.Entry(commit).State = EntityState.Deleted;
				await this.context.SaveChangesAsync(cancellationToken);
			}
		}

		private async Task AddApplicationStatusHistory(int lotId, int? commitId, int creatorUserId, CommitState commitState, string changeStateDescription, CancellationToken cancellationToken)
		{
			var user = await this.context.Set<User>()
			   .SingleOrDefaultAsync(x => x.Id == creatorUserId, cancellationToken);

			var creatorUser = $"{user.FirstName} {user.LastName}";

			var applicationStatusHistory = new ApplicationStatusHistory(lotId, commitId, creatorUser, DateTime.Now, commitState, changeStateDescription);

			this.context.Set<ApplicationStatusHistory>().Add(applicationStatusHistory);
		}
	}
}
