using Resc.Application.Nomenclatures.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Resc.Application.InstitutionSpecialities.Dtos
{
	public class SpecialityFilterDto : BaseNomenclatureFilterDto<SpecialityListItemDto>
	{
		public int? EntityId { get; set; }
		public int? InstitutionId { get; set; }
		public int? ResearchAreaId { get; set; }
		public int? EducationalQualificationId { get; set; }
		public int? EducationFormId { get; set; }
		public int? SchoolYearId { get; set; }

		public override ICollection<Expression<Func<SpecialityListItemDto, object>>> Orders => new List<Expression<Func<SpecialityListItemDto, object>>> {
			e => e.Name,
			e => e.Id
		};
	}
}
