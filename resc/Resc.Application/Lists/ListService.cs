using Microsoft.EntityFrameworkCore;
using Resc.Application.Common.Interfaces;
using Resc.Application.DomainValidations;
using Resc.Application.DomainValidations.Enums;
using Resc.Data.Lists;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Lists
{
    public class ListService<TList, TItem> : IListService<TList, TItem>
        where TList : BaseList
        where TItem : BaseListItem
    {
        protected readonly IAppDbContext context;
        protected readonly DomainValidationService validation;

        public ListService(IAppDbContext context, DomainValidationService validation)
        {
            this.context = context;
            this.validation = validation;
        }

        public async Task<bool> ChangePublishStatusAsync(int listId, bool isPublished, CancellationToken cancellationToken)
        {
            var list = await this.context.Set<TList>()
                .SingleOrDefaultAsync(x => x.Id == listId, cancellationToken);

            if (list == null)
            {
                this.validation.ThrowErrorMessage(ListsErrorCode.List_Not_Found);
            }

            list.IsPublished = isPublished == true;

            await this.context.SaveChangesAsync(cancellationToken);

            return list.IsPublished;
        }

        public async Task RemoveItemAsync(int itemId, CancellationToken cancellationToken)
        {
            var itemToRemove = await this.context.Set<TItem>()
                .SingleOrDefaultAsync(x => x.Id == itemId, cancellationToken);

            if (itemToRemove == null)
            {
                this.validation.ThrowErrorMessage(ListsErrorCode.Item_Not_Found);
            }

			try
			{
                this.context.Set<TItem>().Remove(itemToRemove);
                await this.context.SaveChangesAsync(cancellationToken);
            }
			catch (Exception ex)
			{
                this.validation.ThrowErrorMessage(ListsErrorCode.Item_In_Use);
            }
        }
    }
}
