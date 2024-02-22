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
	public class StudentPartController : ControllerBase
	{
		private readonly StudentPartService studentPartService;

		public StudentPartController(StudentPartService studentPartService)
		{
			this.studentPartService = studentPartService;
		}

		[HttpPut("{partId:int}/entity")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
		public Task UpdateStudentPart([FromRoute] int partId, [FromBody] StudentDto model, CancellationToken cancellationToken)
			=> this.studentPartService.UpdateStudentPart(partId, model, cancellationToken);

		[HttpPost("{partId:int}/startmodification")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
		public async Task<PartDto<StudentDto>> StartStudentPartModification([FromRoute] int partId, CancellationToken cancellationToken)
		{
			int newPartId = await this.studentPartService.StartPartModification(partId, cancellationToken);
			return await this.studentPartService.GetStudentPart(newPartId, cancellationToken);
		}

		[HttpPost("{partId:int}/cancelmodification")]
        [ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, ClaimOperator.Or, UserRoleAliases.UNIVERSITY_USER)]
		public async Task<PartDto<StudentDto>> CancelStudentPartModification([FromRoute] int partId, CancellationToken cancellationToken)
		{
			int resultPartId = await this.studentPartService.CancelPartModification<ApplicationCommit>(partId, cancellationToken);
			return await this.studentPartService.GetStudentPart(resultPartId, cancellationToken);
		}
	}
}
