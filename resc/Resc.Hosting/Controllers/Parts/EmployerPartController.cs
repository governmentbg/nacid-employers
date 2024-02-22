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
	public class EmployerPartController : ControllerBase
	{
		private readonly EmployerPartService employerPartService;

		public EmployerPartController(EmployerPartService employerPartService)
		{
			this.employerPartService = employerPartService;
		}

		[HttpPut("{partId:int}/entity")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
		public Task UpdateEmployerPart([FromRoute] int partId, [FromBody] EmployerDto model, CancellationToken cancellationToken)
			=> this.employerPartService.UpdateEmployerPart(partId, model, cancellationToken);

		[HttpPost("{partId:int}/startmodification")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
		public async Task<PartDto<EmployerDto>> StartEmployerPartModification([FromRoute] int partId, CancellationToken cancellationToken)
		{
			int newPartId = await this.employerPartService.StartPartModification(partId, cancellationToken);
			return await this.employerPartService.GetEmployerPart(newPartId, cancellationToken);
		}

		[HttpPost("{partId:int}/cancelmodification")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
		public async Task<PartDto<EmployerDto>> CancelEmployerPartModification([FromRoute] int partId, CancellationToken cancellationToken)
		{
			int resultPartId = await this.employerPartService.CancelPartModification<ApplicationCommit>(partId, cancellationToken);
			return await this.employerPartService.GetEmployerPart(resultPartId, cancellationToken);
		}
	}
}
