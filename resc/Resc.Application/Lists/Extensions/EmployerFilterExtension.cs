using Resc.Application.InstitutionSpecialities.Dtos;
using Resc.Data.Lists;
using System.Linq;

namespace Resc.Application.Lists.Extensions
{
	public static class EmployerFilterExtension
	{
		public static IQueryable<EmployerListItem> GetFiltered(this IQueryable<EmployerListItem> query, EmployerFilterDto filter)
		{
			if (!string.IsNullOrWhiteSpace(filter.TextFilter))
			{
				query = query.Where(e => e.Name.Trim().ToLower().Contains(filter.TextFilter.Trim().ToLower()));
			}

			return query;
		}
	}
}
