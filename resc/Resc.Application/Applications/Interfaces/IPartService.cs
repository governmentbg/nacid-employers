using Resc.Data.Common.Interfaces;
using Resc.Data.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Applications.Interfaces
{
	public interface IPartService<TPart, TEntity>
			where TPart : Part<TEntity>
			where TEntity : class, IEntity
	{
		Task<int> StartPartModification(int id, CancellationToken cancellationToken);

		Task<int> CancelPartModification<TCommit>(int id, CancellationToken cancellationToken)
			where TCommit : Commit;
	}
}
