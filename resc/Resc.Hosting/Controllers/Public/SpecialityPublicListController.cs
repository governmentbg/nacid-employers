using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resc.Application.InstitutionSpecialities.Dtos;
using Resc.Application.Lists.Dtos;
using Resc.Application.Lists.Specialities;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Data.Lists;
using Resc.Data.Nomenclatures;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Hosting.Controllers.Specialities
{
    [Route("api/public/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SpecialityPublicListController : ControllerBase
    {
        private readonly SpecialityListService specialityListService;

        public SpecialityPublicListController(
            SpecialityListService specialityListService)
        {
            this.specialityListService = specialityListService;
        }

        [HttpGet]
        public async Task<SpecialityList> GetList([FromQuery] BaseListFilter filter, CancellationToken cancellationToken)
            => await this.specialityListService.GetListAsync(filter, cancellationToken);

        [HttpGet("select")]
        public async Task<IEnumerable<SpecialitySelectDto>> SelectItems([FromQuery] SpecialityFilterDto filter, CancellationToken cancellationToken)
           => await this.specialityListService.SelectItemsAsync(filter, cancellationToken);

        [HttpGet("search")]
        public async Task<IEnumerable<SpecialityListItem>> GetItemsFiltered([FromQuery] SpecialityListFilter filter, CancellationToken cancellationToken)
        {
            var filtered = await this.specialityListService.GetItemsFiltered(filter, cancellationToken);

            return filtered.Items;
        }

        //[HttpGet("search")]
        //public async Task<IEnumerable<SpecialityListItemPublicDto>> GetItemsFiltered([FromQuery] SpecialityListFilter filter, CancellationToken cancellationToken)
        //    => await this.publicService.GetSpecialityListAsync(filter, cancellationToken);

        [HttpGet("selectUniversity")]
        public async Task<IEnumerable<NomenclatureDto<Institution>>> SelectUniversitites([FromQuery] SpecialityFilterDto filter, CancellationToken cancellationToken)
            => await this.specialityListService.SelectUniversitiesAsync(filter, cancellationToken);

        [HttpGet("selectResearchArea")]
        public async Task<IEnumerable<NomenclatureDto<ResearchArea>>> SelectResearchAreas([FromQuery] SpecialityFilterDto filter, CancellationToken cancellationToken)
            => await this.specialityListService.SelectResearchAreasAsync(filter, cancellationToken);
    }
}
