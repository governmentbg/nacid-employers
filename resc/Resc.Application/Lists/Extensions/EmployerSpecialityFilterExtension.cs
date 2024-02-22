using Resc.Application.InstitutionSpecialities.Dtos;
using Resc.Data.Lists;
using System.Linq;

namespace Resc.Application.Lists.Extensions
{
	public static class EmployerSpecialityFilterExtension
	{
		public static IQueryable<SpecialityListItem> GetFiltered(this IQueryable<SpecialityListItem> query, SpecialityFilterDto filter, bool isResearchArea, bool isInstitution)
		{
			if (!string.IsNullOrWhiteSpace(filter.TextFilter) && !isResearchArea && !isInstitution)
			{
				query = query.Where(e => e.Speciality.Name.Trim().ToLower().Contains(filter.TextFilter.Trim().ToLower()));
			}

			if (!string.IsNullOrWhiteSpace(filter.TextFilter) && isInstitution)
			{
				query = query.Where(e => e.Institution.Name.Trim().ToLower().Contains(filter.TextFilter.Trim().ToLower()));
			}

			if (!string.IsNullOrWhiteSpace(filter.TextFilter) && isResearchArea)
			{
				query = query.Where(e => e.ResearchArea.Name.Trim().ToLower().Contains(filter.TextFilter.Trim().ToLower()));
			}

			return query;
		}
	}
}
