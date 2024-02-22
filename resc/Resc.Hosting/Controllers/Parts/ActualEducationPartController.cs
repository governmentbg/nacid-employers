using Microsoft.AspNetCore.Mvc;
using Resc.Application.Applications.Dtos.Create;
using Resc.Application.Applications.Parts;
using Resc.Application.Common.Constants;
using Resc.Application.Common.Dtos;
using Resc.Data.Applications.Register;
using Resc.Hosting.Infrastructure.Auth;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Hosting.Controllers.Parts
{
	[Route("api/[controller]")]
	public class ActualEducationPartController : ControllerBase
	{
		private readonly ActualEducationPartService actualEducationPartService;

		public ActualEducationPartController(ActualEducationPartService actualEducationPartService)
		{
			this.actualEducationPartService = actualEducationPartService;
		}

		[HttpPut("{partId:int}/entity")]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
		public Task UpdateActualEducationPart([FromRoute] int partId, [FromBody] ActualEducationDto model, CancellationToken cancellationToken)
			=> this.actualEducationPartService.UpdateActualEducationPart(partId, model, cancellationToken);

		[HttpPost("{partId:int}/startmodification")]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
		public async Task<PartDto<ActualEducationDto>> StartActualEducationPartModification([FromRoute] int partId, CancellationToken cancellationToken)
		{
			int newPartId = await this.actualEducationPartService.StartPartModification(partId, cancellationToken);
			return await this.actualEducationPartService.GetActualEducationPart(newPartId, cancellationToken);
		}

		[HttpPost("{partId:int}/cancelmodification")]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
		public async Task<PartDto<ActualEducationDto>> CancelActualEducationPartModification([FromRoute] int partId, CancellationToken cancellationToken)
		{
			int resultPartId = await this.actualEducationPartService.CancelPartModification<ApplicationCommit>(partId, cancellationToken);
			return await this.actualEducationPartService.GetActualEducationPart(resultPartId, cancellationToken);
		}
	}
}
