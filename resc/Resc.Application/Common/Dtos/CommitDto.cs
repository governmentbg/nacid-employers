using Resc.Data.Common.Enums;

namespace Resc.Application.Common.Dtos
{
	public abstract class CommitDto
	{
		public int Id { get; set; }
		public int LotId { get; set; }
		public CommitState State { get; set; }
		public string ChangeStateDescription { get; set; }
		public bool HasOtherCommits { get; set; }
	}
}
