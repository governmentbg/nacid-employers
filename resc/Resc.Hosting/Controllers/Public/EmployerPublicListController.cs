using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resc.Application.Lists.Dtos;
using Resc.Application.Lists.Employer;
using Resc.Data.Lists;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Resc.Application.Common.Dtos;
using Resc.Application.InstitutionSpecialities.Dtos;

namespace Resc.Hosting.Controllers.Lists
{
    [Route("api/public/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EmployerPublicListController : ControllerBase
    {
        private readonly EmployerListService employerListService;

        public EmployerPublicListController(
            EmployerListService employerListService)
        {
            this.employerListService = employerListService;
        }

        [HttpGet]
        public async Task<EmployerList> GetList([FromQuery] BaseListFilter filter, CancellationToken cancellationToken)
            => await this.employerListService.GetListAsync(filter, cancellationToken);

        [HttpGet("search")]
        public async Task<IEnumerable<EmployerListItem>> GetItemsFiltered([FromQuery] EmployerListFilter filter, CancellationToken cancellationToken)
        {
            var filtered = await this.employerListService.GetItemsFiltered(filter, cancellationToken);

            return filtered.Items;
        }

        [HttpGet("select")]
        public async Task<IEnumerable<EmployerListItem>> SelectItems([FromQuery] EmployerFilterDto filter, CancellationToken cancellationToken)
            => await this.employerListService.SelectItemsAsync(filter, cancellationToken);
    }
}