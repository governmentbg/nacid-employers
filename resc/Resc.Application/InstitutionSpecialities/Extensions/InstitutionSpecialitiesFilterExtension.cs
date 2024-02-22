using Resc.Application.InstitutionSpecialities.Dtos;
using System.Linq;
using Resc.Data.Nomenclatures;

namespace Resc.Application.InstitutionSpecialities.Extensions
{
	public static class EmployerSpecialityFilterExtension
	{
		public static IQueryable<InstitutionSpeciality> GetFiltered(this IQueryable<InstitutionSpeciality> query, SpecialityFilterDto filter)
		{
			if (!filter.IncludeInactive.HasValue || (filter.IncludeInactive.HasValue && !filter.IncludeInactive.Value))
			{
				query = query.Where(e => e.IsActive);
			}

			if (!string.IsNullOrWhiteSpace(filter.TextFilter))
			{
				query = query.Where(e => e.Speciality.Name.Trim().ToLower().Contains(filter.TextFilter.Trim().ToLower()));
			}

			return query;
		}
	}
}
