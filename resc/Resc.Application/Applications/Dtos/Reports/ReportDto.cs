using Resc.Data.Applications.Enums;
using System.Collections.Generic;

namespace Resc.Application.Applications.Dtos.Reports
{
	public class ReportDto
	{
		public List<ReportItemDto> Reports = new List<ReportItemDto>();

		public int TotalCommits { get; set; }

		public int TotalEnteredCommits { get; set; }

		public int TotalChangedCommits { get; set; }

		public int TotalTerminatedCommits { get; set; }

		public int TotalExpiredCommits { get; set; }

		public ReportType ReportType { get; set; }
	}
}
