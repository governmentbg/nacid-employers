using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resc.Application.Common.Interfaces;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Application.Nomenclatures.Services;
using Resc.Data.Lists;
using Resc.Data.Nomenclatures;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Hosting.Controllers.Nomenclatures
{
    public class SchoolYearController
        : BaseNomenclatureController<SchoolYear, NomenclatureDto<SchoolYear>, NomenclatureFilterDto<SchoolYear>>
    {
        private readonly IAppDbContext context;
		private readonly SchoolYearService schoolYearService;

		public SchoolYearController(INomenclatureService<SchoolYear> service, IAppDbContext context, SchoolYearService schoolYearService)
            : base(service)
        {
            this.context = context;
			this.schoolYearService = schoolYearService;
		}

        //TODO: move it to service
        [HttpPost("create")]
        public async Task<SpecialityList> CreateSchoolYear([FromBody] int schoolYearId, CancellationToken cancellationToken)
        {
            var yearsList = await this.context.Set<SchoolYear>()
                      .ToListAsync(cancellationToken);

            var currentYear = yearsList.Where(s => s.IsCurrent && s.Id == schoolYearId).SingleOrDefault();

            var newEntry = new SchoolYear(currentYear.PrimaryYear);
            await this.context.Set<SchoolYear>().AddAsync(newEntry, cancellationToken);

            currentYear.IsCurrent = false;
            this.context.Entry(currentYear).State = EntityState.Modified;

            yearsList.ForEach(e => {
                e.ViewOrder++;
                this.context.Entry(e).State = EntityState.Modified;
            });

            await this.context.SaveChangesAsync(cancellationToken);

            var specialityList = new SpecialityList(newEntry.Id, false);
            await this.context.Set<SpecialityList>().AddAsync(specialityList, cancellationToken);

            var employerList = new EmployerList(newEntry.Id, false);
            await this.context.Set<EmployerList>().AddAsync(employerList, cancellationToken);

            await this.context.SaveChangesAsync(cancellationToken);

            return specialityList;
        }

        [HttpGet("current")]
        public async Task<SchoolYear> GetCurrentYear(CancellationToken cancellationToken)
            => await this.schoolYearService.GetCurrentYear(cancellationToken);
    }
}