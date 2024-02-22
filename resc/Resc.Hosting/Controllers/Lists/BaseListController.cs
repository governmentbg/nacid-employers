using Microsoft.AspNetCore.Mvc;
using Resc.Application.Common.Constants;
using Resc.Application.Lists;
using Resc.Data.Lists;
using Resc.Hosting.Infrastructure.Auth;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Hosting.Controllers.Lists
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseListController<TList, TItem> : ControllerBase
        where TList: BaseList
		where TItem: BaseListItem
    {
		private readonly IListService<TList, TItem> listService;

		public BaseListController(
			IListService<TList, TItem> listService)
		{
			this.listService = listService;
		}

        [HttpPut("publish/{listId:int}")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.CONTROL_USER)]
        public async Task<bool> ChangePublishStatus([FromRoute] int listId, [FromBody] bool isPublished, CancellationToken cancellationToken)
            => await this.listService.ChangePublishStatusAsync(listId, isPublished, cancellationToken);

        [HttpDelete("removeItem/{itemId:int}")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.CONTROL_USER)]
        public async Task RemoveSpeciality([FromRoute] int itemId, CancellationToken cancellationToken)
            => await this.listService.RemoveItemAsync(itemId, cancellationToken);
    }
}
