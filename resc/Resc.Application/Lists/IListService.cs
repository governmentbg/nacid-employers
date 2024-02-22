using Resc.Data.Lists;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Lists
{
	public interface IListService<TList, TItem>
		where TList: BaseList
		where TItem: BaseListItem
	{
		Task<bool> ChangePublishStatusAsync(int listId, bool isPublished, CancellationToken cancellationToken);

		Task RemoveItemAsync(int itemId, CancellationToken cancellationToken);
	}
}
