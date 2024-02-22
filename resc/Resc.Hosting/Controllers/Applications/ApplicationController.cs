using Microsoft.AspNetCore.Mvc;
using Resc.Application.Applications.Dtos.Create;
using Resc.Application.Applications.Dtos.History;
using Resc.Application.Applications.Dtos.Modification;
using Resc.Application.Applications.Dtos.Search;
using Resc.Application.Applications.Interfaces;
using Resc.Application.Common.Constants;
using Resc.Application.Common.Dtos;
using Resc.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Hosting.Controllers.Applications
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService applicationService;
        private readonly IUserContext userContext;

        public ApplicationController(
            IApplicationService applicationService,
            IUserContext userContext)
        {
            this.applicationService = applicationService;
            this.userContext = userContext;
        }

        [HttpGet]
        public async Task<SearchResultItemDto<ApplicationSearchResultDto>> GetApplicationsFiltered([FromQuery] SearchApplicationFilter filter, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(userContext.InstitutionName) && userContext.Role == UserRoleAliases.UNIVERSITY_USER)
            {
                filter.Institution = userContext.InstitutionName;
            }

            return await this.applicationService.GetApplicationsFiltered(filter, cancellationToken);
        }

        [HttpGet("lot/{lotId:int}/commit/{commitId:int}")]
        public async Task<ApplicationCommitDto> GetApplicationCommit([FromRoute] int lotId, [FromRoute] int commitId, CancellationToken cancellationToken)
            => await this.applicationService.GetApplicationCommit(lotId, commitId, cancellationToken);

        [HttpPost]
        public async Task<CommitInfoDto> CreateApplication([FromBody] ApplicationDto model, CancellationToken cancellationToken)
            => await this.applicationService.CreateApplication(model, cancellationToken);

        [HttpGet("lot/{lotId:int}/history")]
        public Task<ApplicationLotHistoryDto> GetApplicationCommitsHistory([FromRoute] int lotId, CancellationToken cancellationToken)
            => this.applicationService.GetApplicationLotHistory(lotId, cancellationToken);

        [HttpPut]
        public async Task UpdateApplicationCommit([FromQuery] int commitId, [FromBody] ApplicationUpdateDto model, CancellationToken cancellationToken)
            => await this.applicationService.UpdateApplication(commitId, model, cancellationToken);
    }
}
