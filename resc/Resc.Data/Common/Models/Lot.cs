using System.Collections.Generic;
using Resc.Data.Common.Interfaces;

namespace Resc.Data.Common.Models
{
	public abstract class Lot<TCommit> : IEntity
		where TCommit : Commit
	{
		public int Id { get; set; }
		public int LotNumber { get; set; }

		public ICollection<TCommit> Commits { get; set; } = new List<TCommit>();
	}
}
