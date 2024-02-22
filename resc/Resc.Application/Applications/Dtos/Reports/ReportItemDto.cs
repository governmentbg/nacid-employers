namespace Resc.Application.Applications.Dtos.Reports
{
	public class ReportItemDto
	{
		public string SchoolYear { get; set; }

		public string ResearchArea { get; set; }

		public string Speciality { get; set; }

		public string Institution { get; set; }

		public int StudentsCount { get; set; }

		public int EnteredCommitsCount { get; set; }

		public int FreeSpotsCounts { get; set; }

		public int ChangedCommitsCount { get; set; }

		public int TerminatedCommitsCount { get; set; }
		
		public int ExpiredCommitsCount { get; set; }
	}
}
