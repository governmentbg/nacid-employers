using Microsoft.AspNetCore.Mvc;
using Resc.Application.Common.Constants;
using Resc.Application.InstitutionSpecialities.Dtos;
using Resc.Application.Lists;
using Resc.Application.Lists.Dtos;
using Resc.Application.Lists.Specialities;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Data.Lists;
using Resc.Data.Nomenclatures;
using Resc.Hosting.Controllers.Lists;
using Resc.Hosting.Infrastructure.Auth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Resc.Application.Common.Dtos;

namespace Resc.Hosting.Controllers.Specialities
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialityListController : BaseListController<SpecialityList, SpecialityListItem>
    {
        private readonly SpecialityListService specialityListService;

        public SpecialityListController(
            IListService<SpecialityList, SpecialityListItem> listService,
            SpecialityListService specialityListService)
            : base(listService)
        {
            this.specialityListService = specialityListService;
        }

        [HttpGet]
        public async Task<SpecialityList> GetList([FromQuery] BaseListFilter filter, CancellationToken cancellationToken)
            => await this.specialityListService.GetListAsync(filter, cancellationToken);

        [HttpGet("{specialityId:int}")]
        public async Task<SpecialityListItem> GetItem([FromRoute] int specialityId, CancellationToken cancellationToken)
            => await this.specialityListService.GetItemAsync(specialityId, cancellationToken);

        [HttpGet("search")]
        public async Task<SearchResultItemDto<SpecialityListItem>> GetItemsFiltered([FromQuery] SpecialityListFilter filter, CancellationToken cancellationToken)
            => await this.specialityListService.GetItemsFiltered(filter, cancellationToken);

        [HttpGet("select")]
        public async Task<IEnumerable<SpecialitySelectDto>> SelectItems([FromQuery]SpecialityFilterDto filter, CancellationToken cancellationToken)
            => await this.specialityListService.SelectItemsAsync(filter, cancellationToken);

        [HttpGet("report/select")]
        public async Task<IEnumerable<SpecialitySelectDto>> SelectReportItems([FromQuery] SpecialityFilterDto filter, CancellationToken cancellationToken)
           => await this.specialityListService.SelectReportItemsAsync(filter, cancellationToken);

        [HttpGet("selectUniversity")]
        public async Task<IEnumerable<NomenclatureDto<Institution>>> SelectUniversitites([FromQuery] SpecialityFilterDto filter, CancellationToken cancellationToken)
            => await this.specialityListService.SelectUniversitiesAsync(filter, cancellationToken);

        [HttpGet("selectResearchArea")]
        public async Task<IEnumerable<NomenclatureDto<ResearchArea>>> SelectResearchAreas([FromQuery] SpecialityFilterDto filter, CancellationToken cancellationToken)
            => await this.specialityListService.SelectResearchAreasAsync(filter, cancellationToken);

        [HttpPost("addItem")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.CONTROL_USER)]
        public virtual async Task<SpecialityListItem> AddItem([FromBody] SpecialityListItem model, CancellationToken cancellationToken)
           => await this.specialityListService.AddItemAsync(model, cancellationToken);

        [HttpPut]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.CONTROL_USER)]
        public async Task EditItem([FromBody] SpecialityListItem model, CancellationToken cancellationToken)
           => await this.specialityListService.EditItemAsync(model, cancellationToken);
    }
}
