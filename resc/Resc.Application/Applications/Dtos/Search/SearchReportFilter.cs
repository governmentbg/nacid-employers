using Resc.Data.Applications.Enums;
using System;

namespace Resc.Application.Applications.Dtos.Search
{
	public class SearchReportFilter
	{
		public int? InstitutionId { get; set; }
		public string InstitutionName { get; set; }

		public int? SpecialityId { get; set; }
		public string SpecialityName { get; set; }

		public int? ResearchAreaId { get; set; }
		public string ResearchAreaName { get; set; }

		public int? EducationalQualificationId { get; set; }
		public string EducationalQualificationName { get; set; }

		public int? SchoolYearId { get; set; }
		public string SchoolYearName { get; set; }

		public DateTime? CreatedReportDate { get; set; }

		public int Limit { get; set; } = 10;
		public int Offset { get; set; } = 0;

		public ReportType ReportType { get; set; }
	}
}
