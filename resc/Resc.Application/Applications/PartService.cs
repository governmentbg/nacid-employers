using Microsoft.EntityFrameworkCore;
using Resc.Application.Applications.Interfaces;
using Resc.Application.Common.Interfaces;
using Resc.Data.Common.Enums;
using Resc.Data.Common.Interfaces;
using Resc.Data.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Applications
{
	public class PartService<TPart, TEntity> : IPartService<TPart, TEntity>
			where TPart : Part<TEntity>
			where TEntity : class, IEntity
	{
		protected readonly IAppDbContext context;

		public PartService(IAppDbContext context)
		{
			this.context = context;
		}

		public async Task<int> StartPartModification(int id, CancellationToken cancellationToken)
		{
			var part = await this.LoadPart()
					.SingleAsync(e => e.Id == id, cancellationToken);

			var entity = (TEntity)Activator.CreateInstance(typeof(TEntity), part.Entity);
			this.context.Set<TEntity>().Add(entity);
			await this.context.SaveChangesAsync(cancellationToken);

			part.Entity = entity;
			part.EntityId = entity.Id;
			part.State = PartState.Modified;
			await this.context.SaveChangesAsync(cancellationToken);

			return part.Id;
		}

		public async Task<int> CancelPartModification<TCommit>(int id, CancellationToken cancellationToken)
			where TCommit : Commit
		{
			var part = await this.LoadPart()
				.SingleAsync(e => e.Id == id, cancellationToken);

			var currentLotIdQuery = this.context.Set<TCommit>()
				.Where(e => e.Id == part.Id)
				.Select(e => e.LotId);

			var previousCommitIdQuery = this.context.Set<TCommit>()
				.Where(e => currentLotIdQuery.Contains(e.LotId) && (e.State == CommitState.ActualWithModification || e.State == CommitState.EnteredWithModification))
				.Select(e => e.Id);

			var previousPart = await this.context.Set<TPart>()
				.Include(e => e.Entity)
				.AsNoTracking()
				.Where(e => previousCommitIdQuery.Contains(e.Id))
				.SingleAsync(cancellationToken);

			var currentEntity = part.Entity;

			part.Entity = previousPart.Entity;
			part.EntityId = previousPart.EntityId;
			part.State = PartState.Unchanged;

			this.context.Set<TEntity>().Remove(currentEntity);

			await this.context.SaveChangesAsync(cancellationToken);

			return part.Id;
		}

		protected virtual IQueryable<TPart> LoadPart()
		{
			return context.Set<TPart>()
				.Include(e => e.Entity);
		}
	}
}
