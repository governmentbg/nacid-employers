using System;
using Resc.Data.Common.Enums;
using Resc.Data.Common.Interfaces;

namespace Resc.Data.Common.Models
{
	public abstract class Part<TEntity> : IEntity, IAuditable
		where TEntity : IEntity
	{
		public int Id { get; set; }

		public int EntityId { get; set; }
		public TEntity Entity { get; set; }

		public PartState State { get; set; }

		public DateTime CreateDate { get; set; }
		public int CreatorUserId { get; set; }


        public Part()
		{
			this.State = PartState.Unchanged;
		}

		public Part(Part<TEntity> part)
			:this()
		{
			this.EntityId = part.EntityId;
		}
	}
}
