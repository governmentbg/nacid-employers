using Microsoft.AspNetCore.Mvc;
using Resc.Application.Common.Constants;
using Resc.Application.Lists;
using Resc.Application.Lists.Dtos;
using Resc.Application.Lists.Employer;
using Resc.Data.Lists;
using Resc.Hosting.Infrastructure.Auth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Resc.Application.Common.Dtos;
using Resc.Application.InstitutionSpecialities.Dtos;

namespace Resc.Hosting.Controllers.Lists
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerListController : BaseListController<EmployerList, EmployerListItem>
    {
        private readonly EmployerListService employerListService;

        public EmployerListController(
            IListService<EmployerList, EmployerListItem> listService,
            EmployerListService employerListService)
            : base(listService)
        {
            this.employerListService = employerListService;
        }

        [HttpGet]
        public async Task<EmployerList> GetList([FromQuery] BaseListFilter filter, CancellationToken cancellationToken)
            => await this.employerListService.GetListAsync(filter, cancellationToken);

        [HttpGet("{employerId:int}")]
        public async Task<EmployerListItem> GetItem([FromRoute] int employerId, CancellationToken cancellationToken)
            => await this.employerListService.GetItemAsync(employerId, cancellationToken);

        [HttpGet("search")]
        public async Task<SearchResultItemDto<EmployerListItem>> GetItemsFiltered([FromQuery] EmployerListFilter filter, CancellationToken cancellationToken)
            => await this.employerListService.GetItemsFiltered(filter, cancellationToken);

        [HttpGet("select")]
        public async Task<IEnumerable<EmployerListItem>> SelectItems([FromQuery] EmployerFilterDto filter, CancellationToken cancellationToken)
            => await this.employerListService.SelectItemsAsync(filter, cancellationToken);

        [HttpPost("addItem")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.CONTROL_USER)]
        public async Task<EmployerListItem> AddItem([FromBody] EmployerListItem model, CancellationToken cancellationToken)
           => await this.employerListService.AddItemAsync(model, cancellationToken);

        [HttpPut]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.CONTROL_USER)]
        public async Task EditItem([FromBody] EmployerListItem model, CancellationToken cancellationToken)
            => await this.employerListService.EditItemAsync(model, cancellationToken);
    }
}