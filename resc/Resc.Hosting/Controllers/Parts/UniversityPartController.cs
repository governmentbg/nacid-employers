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
	public class UniversityPartController : ControllerBase
	{
		private readonly UniversityPartService universityPartService;

		public UniversityPartController(UniversityPartService universityPartService)
		{
			this.universityPartService = universityPartService;
		}

		[HttpPut("{partId:int}/entity")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
		public Task UpdateUniversityPart([FromRoute] int partId, [FromBody] UniversityDto model, CancellationToken cancellationToken)
			=> this.universityPartService.UpdateUniversityPart(partId, model, cancellationToken);

		[HttpPost("{partId:int}/startmodification")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
		public async Task<PartDto<UniversityDto>> StartUniversityPartModification([FromRoute] int partId, CancellationToken cancellationToken)
		{
			int newPartId = await this.universityPartService.StartPartModification(partId, cancellationToken);
			return await this.universityPartService.GetUniversityPart(newPartId, cancellationToken);
		}

		[HttpPost("{partId:int}/cancelmodification")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
		public async Task<PartDto<UniversityDto>> CancelUniversityPartModification([FromRoute] int partId, CancellationToken cancellationToken)
		{
			int resultPartId = await this.universityPartService.CancelPartModification<ApplicationCommit>(partId, cancellationToken);
			return await this.universityPartService.GetUniversityPart(resultPartId, cancellationToken);
		}
	}
}
