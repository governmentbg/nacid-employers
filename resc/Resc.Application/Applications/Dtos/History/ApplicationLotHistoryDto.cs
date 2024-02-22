using System.Collections.Generic;

namespace Resc.Application.Applications.Dtos.History
{
	public class ApplicationLotHistoryDto
	{
		public List<ApplicationCommitHistoryItemDto> Commits { get; set; } = new List<ApplicationCommitHistoryItemDto>();

		public int ActualCommitId { get; set; }
	}
}
