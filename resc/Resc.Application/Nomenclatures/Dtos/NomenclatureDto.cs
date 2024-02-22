using System;
using System.Linq.Expressions;
using Resc.Application.Common.Dtos;
using Resc.Data.Nomenclatures.Models;

namespace Resc.Application.Nomenclatures.Dtos
{
	public class NomenclatureDto<TNomenclature> : IMapping<TNomenclature, NomenclatureDto<TNomenclature>>
		where TNomenclature : Nomenclature
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public virtual Expression<Func<TNomenclature, NomenclatureDto<TNomenclature>>> Map()
		{
			return e => new NomenclatureDto<TNomenclature> {
				Id = e.Id,
				Name = e.Name
			};
		}
	}
}
