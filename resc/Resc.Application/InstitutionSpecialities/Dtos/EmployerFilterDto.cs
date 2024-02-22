using Resc.Application.Nomenclatures.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Resc.Application.InstitutionSpecialities.Dtos
{
    public class EmployerFilterDto : BaseNomenclatureFilterDto<EmployerListItemDto>
    {
		public string Speciality { get; set; }

		public override ICollection<Expression<Func<EmployerListItemDto, object>>> Orders => new List<Expression<Func<EmployerListItemDto, object>>> {
			e => e.Name,
			e => e.Id
		};
	}
}
