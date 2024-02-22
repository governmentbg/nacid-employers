using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Application.Nomenclatures.Services;
using Resc.Data.Nomenclatures;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Hosting.Controllers.Nomenclatures
{
    [AllowAnonymous]
    [Route("api/public/[controller]")]
    public class SchoolYearPublicController
            //: BasePublicNomenclatureController<SchoolYear, NomenclatureDto<SchoolYear>, NomenclatureFilterDto<SchoolYear>>
    {
        private readonly SchoolYearService schoolYearService;

        public SchoolYearPublicController(/*INomenclatureService<SchoolYear> service,*/SchoolYearService schoolYearService)
            //: base(service)
        {
            this.schoolYearService = schoolYearService;
        }

        [HttpGet("current")]
        public async Task<SchoolYear> GetCurrentYear(CancellationToken cancellationToken)
            => await this.schoolYearService.GetCurrentYear(cancellationToken);

        [HttpGet("select")]
        public async Task<IEnumerable<SchoolYear>> SelectSchoolYear(CancellationToken cancellationToken)
            => await this.schoolYearService.SelectAsync(cancellationToken);
    }
}
