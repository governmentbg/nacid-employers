using Microsoft.EntityFrameworkCore;
using Resc.Application.Applications.Dtos.Reports;
using Resc.Application.Applications.Dtos.Search;
using Resc.Application.Applications.Interfaces;
using Resc.Application.Common.Constants;
using Resc.Application.Common.Interfaces;
using Resc.Data.Applications.Enums;
using Resc.Data.Applications.Register;
using Resc.Data.Common.Enums;
using Resc.Data.Lists;
using Resc.Data.Users;
using System.Linq;
using System.Threading.Tasks;

namespace Resc.Application.Applications
{
	public class ReportService : IReportService
	{
		private readonly IAppDbContext context;
		private readonly IUserContext userContext;

		public ReportService(IAppDbContext context, IUserContext userContext)
		{
			this.context = context;
			this.userContext = userContext;
		}

		public async Task<ReportDto> GetReport(SearchReportFilter filter)
		{
			var commits = this.context.Set<ApplicationCommit>()
				.Include(a => a.UniversityPart.Entity.SpecialityListItem.Speciality)
				.Include(a => a.UniversityPart.Entity.Institution)
				.Include(a => a.UniversityPart.Entity.SpecialityListItem.ResearchArea)
				.Where(a => a.UniversityPart.Entity.SpecialityListItem.SpecialityList.SchoolYearId == filter.SchoolYearId 
				&& (a.State == CommitState.Entered || a.State == CommitState.EnteredWithChange || a.State == CommitState.Terminated || a.State == CommitState.Expired))
				.AsQueryable();

			var specialityListItems = this.context.Set<SpecialityList>()
				.Where(x => x.SchoolYearId == filter.SchoolYearId)
				.SelectMany(x => x.Items)
				.Include(e => e.Institution)
				.Include(e => e.ResearchArea)
				.Include(e => e.Speciality)
				.AsQueryable();

			if (this.userContext.Role == UserRoleAliases.UNIVERSITY_USER)
			{
				var userInstitutionId = await this.context.Set<User>()
					.Where(x => x.Id == this.userContext.UserId)
					.Select(x => x.InstitutionId)
					.SingleAsync();

				commits = commits.Where(x => x.UniversityPart.Entity.InstitutionId == userInstitutionId);
				specialityListItems = specialityListItems.Where(x => x.InstitutionId == userInstitutionId);
			}

			if (filter.EducationalQualificationId.HasValue)
			{
				commits = commits.Where(x => x.UniversityPart.Entity.SpecialityListItem.EducationalQualificationId == filter.EducationalQualificationId);
				specialityListItems = specialityListItems.Where(x => x.EducationalQualificationId == filter.EducationalQualificationId);
			}

			if (filter.SpecialityId.HasValue)
			{
				specialityListItems = specialityListItems.Where(x => x.SpecialityId == filter.SpecialityId);
			}

			if (filter.InstitutionId.HasValue)
			{
				specialityListItems = specialityListItems.Where(x => x.InstitutionId == filter.InstitutionId);
			}

			if (filter.ResearchAreaId.HasValue)
			{
				specialityListItems = specialityListItems.Where(x => x.ResearchAreaId == filter.ResearchAreaId);
			}

			var report = new ReportDto {
				ReportType = filter.ReportType,
			};

			var materializedListItems = specialityListItems.ToList();


			if (filter.ReportType == ReportType.ReportBySpecialty)
			{
				var groupedListItems = materializedListItems
					.GroupBy(x => x.Speciality.Name)
					.Select(x => x.First())
					.ToList();

				foreach (var item in groupedListItems)
				{
					var filteredCommits = commits.Where(x => x.UniversityPart.Entity.SpecialityListItem.Speciality.Id == item.SpecialityId);

					var reportItem = new ReportItemDto {
						Speciality = item.Speciality.Name,
						StudentsCount = specialityListItems.Where(e => e.Speciality.Name == item.Speciality.Name).Sum(e => e.StudentsCount),
						EnteredCommitsCount = filteredCommits.Where(a => a.State == CommitState.Entered || a.State == CommitState.EnteredWithChange || a.State == CommitState.Terminated).Count(),
						ChangedCommitsCount = filteredCommits.Where(a => a.State == CommitState.EnteredWithChange).Count(),
						TerminatedCommitsCount = filteredCommits.Where(a => a.State == CommitState.Terminated).Count(),
						ExpiredCommitsCount = filteredCommits.Where(a => a.State == CommitState.Expired).Count()
					};

					reportItem.FreeSpotsCounts = reportItem.StudentsCount - reportItem.EnteredCommitsCount;

					report.Reports.Add(reportItem);
				}
			}

			if (filter.ReportType == ReportType.ReportByInstitution)
			{
				var groupedListItems = materializedListItems
					.GroupBy(x => x.Institution.Name)
					.Select(x => x.First())
					.ToList();

				foreach (var item in groupedListItems)
				{
					var filteredCommits = commits.Where(x => x.UniversityPart.Entity.InstitutionId == item.InstitutionId);

					var reportItem = new ReportItemDto {
						Institution = item.Institution.Name,
						StudentsCount = specialityListItems.Where(e => e.Institution.Name == item.Institution.Name).Sum(e => e.StudentsCount),
						EnteredCommitsCount = filteredCommits.Where(a => a.State == CommitState.Entered || a.State == CommitState.EnteredWithChange || a.State == CommitState.Terminated).Count(),
						ChangedCommitsCount = filteredCommits.Where(a => a.State == CommitState.EnteredWithChange).Count(),
						TerminatedCommitsCount = filteredCommits.Where(a => a.State == CommitState.Terminated).Count(),
						ExpiredCommitsCount = filteredCommits.Where(a => a.State == CommitState.Expired).Count()
					};

					reportItem.FreeSpotsCounts = reportItem.StudentsCount - reportItem.EnteredCommitsCount;

					report.Reports.Add(reportItem);
				}
			}

			if (filter.ReportType == ReportType.ReportByResearchArea)
			{
				var groupedListItems = materializedListItems
					.GroupBy(x => x.ResearchArea.Name)
					.Select(x => x.First())
					.ToList();

				foreach (var item in groupedListItems)
				{
					var filteredCommits = commits.Where(x => x.UniversityPart.Entity.SpecialityListItem.ResearchAreaId == item.ResearchAreaId);

					var reportItem = new ReportItemDto {
						ResearchArea = item.ResearchArea.Name,
						StudentsCount = specialityListItems.Where(e => e.ResearchArea.Name == item.ResearchArea.Name).Sum(e => e.StudentsCount),
						EnteredCommitsCount = filteredCommits.Where(a => a.State == CommitState.Entered || a.State == CommitState.EnteredWithChange || a.State == CommitState.Terminated).Count(),
						ChangedCommitsCount = filteredCommits.Where(a => a.State == CommitState.EnteredWithChange).Count(),
						TerminatedCommitsCount = filteredCommits.Where(a => a.State == CommitState.Terminated).Count(),
						ExpiredCommitsCount = filteredCommits.Where(a => a.State == CommitState.Expired).Count()
					};

					reportItem.FreeSpotsCounts = reportItem.StudentsCount - reportItem.EnteredCommitsCount;

					report.Reports.Add(reportItem);
				}
			}

			if (filter.ReportType == ReportType.ReportByResearchAreaAndSpecialty)
			{
				var groupedListItems = materializedListItems
					.GroupBy(x => new { x.ResearchArea.Name, x.SpecialityId })
					.Select(x => x.First())
					.ToList();

				foreach (var item in groupedListItems)
				{
					var filteredCommits = commits.Where(x => x.UniversityPart.Entity.SpecialityListItem.ResearchAreaId == item.ResearchAreaId && x.UniversityPart.Entity.SpecialityListItem.Speciality.Id == item.SpecialityId);

					var reportItem = new ReportItemDto {
						ResearchArea = item.ResearchArea.Name,
						Speciality = item.Speciality.Name,
						StudentsCount = specialityListItems.Where(e => e.ResearchArea.Name == item.ResearchArea.Name && e.Speciality.Name == item.Speciality.Name).Sum(e => e.StudentsCount),
						EnteredCommitsCount = filteredCommits.Where(a => a.State == CommitState.Entered || a.State == CommitState.EnteredWithChange || a.State == CommitState.Terminated).Count(),
						ChangedCommitsCount = filteredCommits.Where(a => a.State == CommitState.EnteredWithChange).Count(),
						TerminatedCommitsCount = filteredCommits.Where(a => a.State == CommitState.Terminated).Count(),
						ExpiredCommitsCount = filteredCommits.Where(a => a.State == CommitState.Expired).Count()
					};

					reportItem.FreeSpotsCounts = reportItem.StudentsCount - reportItem.EnteredCommitsCount;

					report.Reports.Add(reportItem);
				}
			}

			if (filter.ReportType == ReportType.ReportByResearchAreaAndSpecialityAndInstitution)
			{
				var groupedListItems = materializedListItems
					   .GroupBy(x => new { x.ResearchArea.Name, x.SpecialityId, x.InstitutionId })
					   .Select(x => x.First())
					   .ToList();

				foreach (var item in groupedListItems)
				{
					var filteredCommits = commits.Where(x => x.UniversityPart.Entity.SpecialityListItem.ResearchAreaId == item.ResearchAreaId 
					&& x.UniversityPart.Entity.SpecialityListItem.Speciality.Id == item.SpecialityId
					&& x.UniversityPart.Entity.InstitutionId == item.InstitutionId);

					var reportItem = new ReportItemDto {
						Institution = item.Institution.Name,
						ResearchArea = item.ResearchArea.Name,
						Speciality = item.Speciality.Name,
						StudentsCount = specialityListItems.Where(e => e.ResearchArea.Name == item.ResearchArea.Name && e.Speciality.Name == item.Speciality.Name && e.Institution.Name == item.Institution.Name).Sum(e => e.StudentsCount),
						EnteredCommitsCount = filteredCommits.Where(a => a.State == CommitState.Entered || a.State == CommitState.EnteredWithChange || a.State == CommitState.Terminated).Count(),
						ChangedCommitsCount = filteredCommits.Where(a => a.State == CommitState.EnteredWithChange).Count(),
						TerminatedCommitsCount = filteredCommits.Where(a => a.State == CommitState.Terminated).Count(),
						ExpiredCommitsCount = filteredCommits.Where(a => a.State == CommitState.Expired).Count()
					};

					reportItem.FreeSpotsCounts = reportItem.StudentsCount - reportItem.EnteredCommitsCount;

					report.Reports.Add(reportItem);
				}

			}

			if (filter.ReportType == ReportType.DefaultReport)
			{
				var reportItem = new ReportItemDto {
					StudentsCount = specialityListItems.Sum(e => e.StudentsCount),
					EnteredCommitsCount = commits.Where(a => a.State == CommitState.Entered || a.State == CommitState.EnteredWithChange || a.State == CommitState.Terminated).Count(),
					ChangedCommitsCount = commits.Where(a => a.State == CommitState.EnteredWithChange).Count(),
					TerminatedCommitsCount = commits.Where(a => a.State == CommitState.Terminated).Count(),
					ExpiredCommitsCount = commits.Where(a => a.State == CommitState.Expired).Count()
				};

				reportItem.FreeSpotsCounts = reportItem.StudentsCount - reportItem.EnteredCommitsCount;

				report.Reports.Add(reportItem);
			}

			//foreach (var schoolYear in schoolYears)
			//{
			//	var reportItem = new ReportItemDto {
			//		SchoolYear = schoolYear.Name
			//	};

			//	var filteredCommits = commits.Where(a => a.UniversityPart.Entity.SpecialityListItem.SpecialityList.SchoolYearId == schoolYear.Id);
				
			//	if (filter.ReportType == ReportType.ReportByInstitution && filter.InstitutionId != null)
			//	{
			//		filteredCommits = filteredCommits.Where(x => x.UniversityPart.Entity.InstitutionId == filter.InstitutionId);
			//		reportItem.Institution = filter.InstitutionName;
			//	}
			//	else if (filter.ReportType == ReportType.ReportByResearchArea && filter.ResearchAreaId != null)
			//	{
			//		filteredCommits = filteredCommits.Where(x => x.UniversityPart.Entity.SpecialityListItem.ResearchAreaId == filter.ResearchAreaId);
			//		reportItem.ResearchArea = filter.ResearchAreaName;
			//	}
			//	else if (filter.ReportType == ReportType.ReportBySpecialty)
			//	{
			//		if (filter.SpecialityId.HasValue)
			//		{
			//			filteredCommits = filteredCommits.Where(x => x.UniversityPart.Entity.SpecialityListItem.SpecialityId == filter.SpecialityId);
			//		}

			//		reportItem.Speciality = filter.SpecialityName;
			//	}
			//	else if (filter.ReportType == ReportType.ReportByResearchAreaAndSpecialty && filter.SpecialityId != null && filter.ResearchAreaId != null)
			//	{
			//		filteredCommits = filteredCommits.Where(x => x.UniversityPart.Entity.SpecialityListItem.SpecialityId == filter.SpecialityId
			//		&& x.UniversityPart.Entity.SpecialityListItem.ResearchAreaId == filter.ResearchAreaId);
			//		reportItem.Speciality = filter.SpecialityName;
			//		reportItem.ResearchArea = filter.ResearchAreaName;
			//	}
			//	else if (filter.ReportType == ReportType.ReportByResearchAreaAndSpecialityAndInstitution && filter.SpecialityId != null && filter.ResearchAreaId != null && filter.InstitutionId != null)
			//	{
			//		filteredCommits = filteredCommits.Where(x => x.UniversityPart.Entity.SpecialityListItem.SpecialityId == filter.SpecialityId
			//		&& x.UniversityPart.Entity.SpecialityListItem.ResearchAreaId == filter.ResearchAreaId && x.UniversityPart.Entity.InstitutionId == filter.InstitutionId);
			//		reportItem.Speciality = filter.SpecialityName;
			//		reportItem.ResearchArea = filter.ResearchAreaName;
			//		reportItem.Institution = filter.InstitutionName;
			//	}

			//	reportItem.CommitsCount = filteredCommits.Count();
			//	reportItem.EnteredCommitsCount = filteredCommits.Where(a => a.State == CommitState.Entered).Count();
			//	reportItem.ChangedCommitsCount = filteredCommits.Where(a => a.State == CommitState.EnteredWithChange).Count();
			//	reportItem.TerminatedCommitsCount = filteredCommits.Where(a => a.State == CommitState.Terminated).Count();
			//	reportItem.ExpiredCommitsCount = filteredCommits.Where(a => a.State == CommitState.Expired).Count();

			//	report.Reports.Add(reportItem);

			//	if (filter.ReportType == ReportType.DefaultReport)
			//	{
			//		report.TotalCommits += reportItem.CommitsCount;
			//		report.TotalEnteredCommits += reportItem.EnteredCommitsCount;
			//		report.TotalChangedCommits += reportItem.ChangedCommitsCount;
			//		report.TotalTerminatedCommits += reportItem.TerminatedCommitsCount;
			//		report.TotalExpiredCommits += reportItem.ExpiredCommitsCount;
			//	}
			//}

			return report;
		}
	}
}
