using Resc.Data.Common.Enums;
using System;

namespace Resc.Application.Applications.Dtos.History
{
	public class ApplicationCommitHistoryItemDto
	{
		public int? CommitId { get; set; }
		public int LotId { get; set; }

		public CommitState State { get; set; }

		public int? Number { get; set; }
		public DateTime CreateDate { get; set; }
		public string ContractNumber { get; set; }
		public string ChangeStateDescription { get; set; }
		public string CreatorUser { get; set; }
	}
}
