using Microsoft.EntityFrameworkCore;
using Resc.Application.Common.Interfaces;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Data.Lists;
using Resc.Data.Nomenclatures;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Nomenclatures.Services
{
    public class SchoolYearService
    {
        private readonly IAppDbContext context;

        public SchoolYearService(IAppDbContext context)
        {
            this.context = context;
        }

        public async Task<SchoolYear> GetCurrentYear(CancellationToken cancellationToken)
        {
            var query = this.PrepareQuery();

            var year = await query
                .OrderBy(e => e.ViewOrder)
                .FirstOrDefaultAsync(cancellationToken);

            return year;
        }

        public async Task<IEnumerable<SchoolYear>> SelectAsync(CancellationToken cancellationToken)
        {
            var query = this.PrepareQuery();

            var items = await query.ToListAsync(cancellationToken);

            return items;
        }

        private IQueryable<SchoolYear> PrepareQuery()
        {
            var employerList = this.context.Set<EmployerList>()
                .AsNoTracking()
                .Where(e => e.IsPublished)
                .AsQueryable();

            var specialityList = this.context.Set<SpecialityList>()
                .AsNoTracking()
                .Where(e => e.IsPublished)
                .AsQueryable();

            var query = this.context.Set<SchoolYear>()
                .AsNoTracking()
                .Where(e => specialityList.Any(s => s.SchoolYearId == e.Id) && employerList.Any(emp => emp.SchoolYearId == e.Id))
                .AsQueryable();

            return query;
        }
    }
}
