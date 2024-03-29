﻿using Resc.Application.Common.Dtos;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Data.Nomenclatures.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Resc.Application.Nomenclatures.Services
{
	public interface INomenclatureService<TNomenclature>
		where TNomenclature : Nomenclature
	{
		Task<IEnumerable<TNomenclature>> GetNomenclaturesAsync<TFilter>(TFilter filter)
			where TFilter : BaseNomenclatureFilterDto<TNomenclature>;

		Task<IEnumerable<TDto>> SelectNomenclaturesAsync<TFilter, TDto>(TFilter filter)
			where TFilter : BaseNomenclatureFilterDto<TNomenclature>
			where TDto : IMapping<TNomenclature, TDto>, new();

		Task<TNomenclature> GetSingleOrDefaultNomenclatureAsync(Expression<Func<TNomenclature, bool>> predicate);

		Task<TNomenclature> InsertNomenclatureAsync(TNomenclature model);
		Task<TNomenclature> UpdateNomenclatureAsync(int id, TNomenclature model);
		Task DeleteNomenclatureAsync(int id);
	}
}
