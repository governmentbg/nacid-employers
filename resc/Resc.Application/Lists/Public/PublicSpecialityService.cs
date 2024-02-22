using Microsoft.EntityFrameworkCore;
using Resc.Application.Common.Dtos;
using Resc.Application.Common.Interfaces;
using Resc.Application.Lists.Dtos;
using Resc.Application.Lists.Public.Dtos;
using Resc.Data.Applications.Register;
using Resc.Data.Common.Enums;
using Resc.Data.Lists;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Lists.Public
{
    public class PublicSpecialityService
    {
        private readonly IAppDbContext context;

        public PublicSpecialityService(IAppDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SpecialityListItemPublicDto>> GetSpecialityListAsync(SpecialityListFilter filter, CancellationToken cancellationToken)
        {
            var commits = this.context.Set<ApplicationCommit>()
                .AsNoTracking()
                .Where(c => c.State == CommitState.Entered ||
                    c.State == CommitState.EnteredWithModification ||
                    c.State == CommitState.EnteredWithChange ||
                    c.State == CommitState.EnteredModification ||
                    c.State == CommitState.Terminated ||
                    c.State == CommitState.Expired);

            IQueryable<SpecialityListItem> query = this.context.Set<SpecialityListItem>()
                .AsNoTracking()
                .Include(s => s.Institution)
                .Include(s => s.Speciality)
                .Include(s => s.ResearchArea)
                .Include(s => s.SpecialityMinisters)
                    .ThenInclude(e => e.Minister)
                .Include(s => s.EducationFormType)
                .Include(s => s.EducationalQualification)
                .Where(s => s.SpecialityList.SchoolYearId == filter.SchoolYearId && s.SpecialityList.IsPublished);

            if (!string.IsNullOrWhiteSpace(filter.SpecialityName))
            {
                query = query.Where(s => s.Speciality.Name == filter.SpecialityName);
            }

            if (filter.InstitutionId.HasValue)
            {
                query = query.Where(s => s.InstitutionId == filter.InstitutionId);
            }

            if (filter.ResearchAreaId.HasValue)
            {
                query = query.Where(s => s.ResearchAreaId == filter.ResearchAreaId);
            }

            var items = await query
                .OrderBy(x => x.ResearchArea.Name)
                    .ThenBy(x => x.Institution.Name)
                        .ThenBy(x => x.Speciality.Name)
                .Select(i => new SpecialityListItemPublicDto {
                    Id = i.Id,
                    EducationalQualification = i.EducationalQualification,
                    EducationFormType = i.EducationFormType,
                    Institution = i.Institution,
                    ResearchArea = i.ResearchArea,
                    Speciality = i.Speciality,
                    SpecialityMinisters = i.SpecialityMinisters,
                    StudentsCount = i.StudentsCount
                })
                .ToListAsync(cancellationToken);

            foreach (var item in items)
            {
                var contractCount = commits
                    .Count(c => c.UniversityPart.Entity.SpecialityListItemId == item.Id);

                item.FreeSpotsCount = item.StudentsCount - contractCount;
            }

            return items;
        }
    }
}
