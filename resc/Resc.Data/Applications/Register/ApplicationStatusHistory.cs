using Resc.Data.Common.Enums;
using Resc.Data.Common.Interfaces;
using System;

namespace Resc.Data.Applications.Register
{
	public class ApplicationStatusHistory : IEntity
	{
		public int Id { get; set; }

		public string CreatorUser { get; set; }

		public DateTime CreateDate { get; set; }

		public CommitState CommitState { get; set; }

		public string ChangeStateDescription { get; set; }

		public int LotId { get; set; }

		public int? CommitId { get; set; }

		public ApplicationStatusHistory(int lotId, int? commitId, string creatorUser, DateTime createDate, CommitState commitState, string changeStateDescription)
		{
			this.LotId = lotId;
			this.CommitId = commitId;
			this.CreatorUser = creatorUser;
			this.CreateDate = createDate;
			this.CommitState = commitState;
			this.ChangeStateDescription = changeStateDescription;
		}
	}
}
