using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Resc.Application.Applications.Dtos.Modification;
using Resc.Application.Applications.Interfaces;
using Resc.Application.Common.Configurations;
using Resc.Application.Common.Constants;
using Resc.Data.Nomenclatures.Constants;
using Resc.Hosting.Infrastructure.Auth;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Hosting.Controllers.Applications
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationModificationController : ControllerBase
    {
        private readonly IApplicationService applicationService;
		private readonly AuthConfiguration authConfiguration;
		private readonly IApplicationModificationService modificationService;

        public ApplicationModificationController(
            IApplicationModificationService modificationService,
            IApplicationService applicationService,
            IOptions<AuthConfiguration> authOptions
            )
        {
            this.modificationService = modificationService;
            this.applicationService = applicationService;
			this.authConfiguration = authOptions.Value;
		}

        [HttpPost("lot/{lotId:int}/startmodification")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.CONTROL_USER)]
        public async Task<ApplicationCommitDto> StartModification([FromRoute] int lotId, [FromBody] ChangeStateDescriptionDto changeStateDescription, CancellationToken cancellationToken)
        {
            var modificationCommitInfo = await this.modificationService.StartModification(lotId, changeStateDescription.ChangeStateDescription, cancellationToken);

            var result = await this.applicationService.GetApplicationCommit(lotId, modificationCommitInfo.CommitId, cancellationToken);
            
            var payload = new {
                //ContractNumber = result.ContractPart.Entity.Number,
                Description = result.ChangeStateDescription.Trim() ?? "Няма зададени указания",
            };

            await this.modificationService.SendEmail(EmailTypeAlias.MODIFICATION_APPLICATION, payload, result.CreatorUserId, cancellationToken);
            return result;
        }

        [HttpPost("lot/{lotId:int}/finishmodification")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
        public async Task<ApplicationCommitDto> FinishModification([FromRoute] int lotId, CancellationToken cancellationToken)
        {
            var commit = await this.modificationService.FinishCommitModification(lotId, cancellationToken);

            return await this.applicationService.GetApplicationCommit(lotId, commit.CommitId, cancellationToken);
        }

        [HttpPost("lot/{lotId:int}/cancelmodification")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
        public async Task<ApplicationCommitDto> CancelModification([FromRoute] int lotId, CancellationToken cancellationToken)
        {
            var commit = await this.modificationService.CancelModification(lotId, cancellationToken);

            return await this.applicationService.GetApplicationCommit(lotId, commit.CommitId, cancellationToken);
        }

        [HttpPost("lot/{lotId:int}/erase")]
        public async Task<ApplicationCommitDto> EraseApplication([FromRoute] int lotId, [FromBody] ChangeStateDescriptionDto changeStateDescription, CancellationToken cancellationToken)
        {
            var erasedCommitInfo = await this.modificationService.EraseApplication(lotId, changeStateDescription.ChangeStateDescription, cancellationToken);
            return await this.applicationService.GetApplicationCommit(erasedCommitInfo.LotId, erasedCommitInfo.CommitId, cancellationToken);
        }

        [HttpPost("lot/{lotId:int}/reverterased")]
        public async Task<ApplicationCommitDto> RevertErasedApplication([FromRoute] int lotId, CancellationToken cancellationToken)
        {
            var revertedCommitInfo = await this.modificationService.RevertErasedApplication(lotId, cancellationToken);
            return await this.applicationService.GetApplicationCommit(revertedCommitInfo.LotId, revertedCommitInfo.CommitId, cancellationToken);
        }

        [HttpPost("lot/{lotId:int}/enter")]
        public async Task<ApplicationCommitDto> EnterApplication([FromRoute] int lotId, CancellationToken cancellationToken)
        {
            var enteredCommitInfo = await this.modificationService.EnterApplication(lotId, cancellationToken);
            var result = await this.applicationService.GetApplicationCommit(enteredCommitInfo.LotId, enteredCommitInfo.CommitId, cancellationToken);

            var payload = new {
                RegisterNumber = result.RegisterNumber,
                //ApplicationLink = $"{this.authConfiguration.Issuer}/application/lot/{result.LotId}/commit/{result.Id}",
            };

            await this.modificationService.SendEmail(EmailTypeAlias.ENTERED_APPLICATION, payload, result.CreatorUserId, cancellationToken);

            return result;
        }

        [HttpPost("lot/{lotId:int}/startmodificationentered")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
        public async Task<ApplicationCommitDto> StartModificationEntered([FromRoute] int lotId, [FromBody] ChangeStateDescriptionDto changeStateDescription, CancellationToken cancellationToken)
        {
            var modificationCommitInfo = await this.modificationService.StartModificationEnteredApplication(lotId, changeStateDescription.ChangeStateDescription, cancellationToken);
            return await this.applicationService.GetApplicationCommit(lotId, modificationCommitInfo.CommitId, cancellationToken);
        }

        [HttpPost("lot/{lotId:int}/finishmodificationentered")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
        public async Task<ApplicationCommitDto> FinishEnteredModification([FromRoute] int lotId, CancellationToken cancellationToken)
        {
            var commit = await this.modificationService.FinishEnteredModification(lotId, cancellationToken);

            return await this.applicationService.GetApplicationCommit(lotId, commit.CommitId, cancellationToken);
        }

        [HttpPost("lot/{lotId:int}/changecontract")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.UNIVERSITY_USER)]
        public async Task<ApplicationCommitDto> ChangeEnteredContract([FromRoute] int lotId, [FromBody] ApplicationModificationDto dto, CancellationToken cancellationToken)
        {
            var commit = await this.modificationService.ChangeEnteredContract(lotId, dto, cancellationToken);

            return await this.applicationService.GetApplicationCommit(lotId, commit.CommitId, cancellationToken);
        }

        [HttpPost("lot/{lotId:int}/terminate")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.UNIVERSITY_USER)]
        public async Task<ApplicationCommitDto> TerminateContract([FromRoute] int lotId, [FromBody] ApplicationTerminationDto dto, CancellationToken cancellationToken)
        {
            var commit = await this.modificationService.TerminateContract(lotId, dto, cancellationToken);

            return await this.applicationService.GetApplicationCommit(lotId, commit.CommitId, cancellationToken);
        }

        [HttpDelete("lot/{lotId:int}/deleteDraft")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.UNIVERSITY_USER)]
        public async Task DeleteDraft([FromRoute] int lotId, CancellationToken cancellationToken)
            => await this.modificationService.DeleteDraft(lotId, cancellationToken);
    }
}
