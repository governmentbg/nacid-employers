using Microsoft.AspNetCore.Mvc;
using Resc.Application.Common.Dtos;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Application.Nomenclatures.Services;
using Resc.Data.Nomenclatures.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resc.Hosting.Controllers.Nomenclatures
{
	[ApiController]
	[Route("api/public/[controller]")]
	public abstract class BasePublicNomenclatureController<T, TDto, TFilter> : ControllerBase
		where T : Nomenclature
		where TDto : IMapping<T, TDto>, new()
		where TFilter : BaseNomenclatureFilterDto<T>
	{
		protected readonly INomenclatureService<T> nomenclatureService;

		public BasePublicNomenclatureController(INomenclatureService<T> nomenclatureService)
		{
			this.nomenclatureService = nomenclatureService;
		}

		[HttpGet]
		public Task<IEnumerable<T>> GetNomenclatures([FromQuery] TFilter filter)
			=> this.nomenclatureService.GetNomenclaturesAsync(filter);

		[HttpGet("Select")]
		public Task<IEnumerable<TDto>> SelectNomenclatures([FromQuery] TFilter filter)
			=> this.nomenclatureService.SelectNomenclaturesAsync<TFilter, TDto>(filter);
	}
}
