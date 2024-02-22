using Microsoft.AspNetCore.Mvc;
using Resc.Application.Common.Constants;
using Resc.Application.Common.Dtos;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Application.Nomenclatures.Services;
using Resc.Data.Nomenclatures.Models;
using Resc.Hosting.Infrastructure.Auth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Resc.Hosting.Controllers.Nomenclatures
{
	[ApiController]
	[Route("api/[controller]")]
	public abstract class BaseNomenclatureController<T, TDto, TFilter> : ControllerBase
		where T : Nomenclature
		where TDto : IMapping<T, TDto>, new()
		where TFilter : BaseNomenclatureFilterDto<T>
	{
		protected readonly INomenclatureService<T> nomenclatureService;

		public BaseNomenclatureController(INomenclatureService<T> nomenclatureService)
		{
			this.nomenclatureService = nomenclatureService;
		}

		[HttpGet]
		public Task<IEnumerable<T>> GetNomenclatures([FromQuery] TFilter filter)
			=> this.nomenclatureService.GetNomenclaturesAsync(filter);

		[HttpGet("Select")]
		public Task<IEnumerable<TDto>> SelectNomenclatures([FromQuery] TFilter filter)
			=> this.nomenclatureService.SelectNomenclaturesAsync<TFilter, TDto>(filter);

		[HttpPost]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
		public Task<T> AddNomenclature([FromBody] T model)
			=> this.nomenclatureService.InsertNomenclatureAsync(model);

		[HttpPut("{id:int}")]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
		public Task<T> UpdateNomenclature([FromRoute] int id, [FromBody] T model)
			=> this.nomenclatureService.UpdateNomenclatureAsync(id, model);

		[HttpDelete("{id:int}")]
		[ClaimAuthorization(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR)]
		public Task DeleteNomenclature([FromRoute] int id)
			=> this.nomenclatureService.DeleteNomenclatureAsync(id);
	}
}
