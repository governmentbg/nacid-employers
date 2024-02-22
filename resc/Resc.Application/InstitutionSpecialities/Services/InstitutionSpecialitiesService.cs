using Microsoft.EntityFrameworkCore;
using Resc.Application.Common.Extensions;
using Resc.Application.Common.Interfaces;
using Resc.Application.InstitutionSpecialities.Dtos;
using Resc.Application.InstitutionSpecialities.Extensions;
using Resc.Application.Lists.Extensions;
using Resc.Data.Nomenclatures;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.InstitutionSpecialities.Services
{
    public class InstitutionSpecialitiesService
    {
        private readonly IAppDbContext context;

        public InstitutionSpecialitiesService(IAppDbContext context)
        {
            this.context = context;
        }

        public async Task SaveInstitution(InstitutionDto dto)
        {
            var model = await context.Set<Institution>()
                .Include(e => e.InstitutionSpecialities)
                    .ThenInclude(s => s.Speciality)
                .SingleOrDefaultAsync(e => e.ExternalId == dto.Id);
            if (model == null)
            {
                model = new Institution(dto.Name, dto.IsActive, dto.InstitutionType, dto.Id, dto.RootId, dto.ParentId, dto.Level);
                context.Entry(model).State = EntityState.Added;
            }
            else
            {
                model.Update(dto.Name, dto.IsActive, dto.InstitutionType, dto.Id, dto.RootId, dto.ParentId, dto.Level);
            }

            var specialities = await context.Set<Speciality>()
                .AsNoTracking()
                .ToListAsync();

            var forRemove = model.InstitutionSpecialities
                .Where(e => !dto.InstitutionSpecialities.Any(d => d.Id == e.ExternalId))
                .ToList();
            if (forRemove.Any())
            {
                forRemove.ForEach(fr => model.InstitutionSpecialities.Remove(fr));
                context.Set<InstitutionSpeciality>().RemoveRange(forRemove);
            }

            foreach (var dtoInstitutionSpeciality in dto.InstitutionSpecialities)
            {
                var speciality = model.InstitutionSpecialities
                    .Select(e => e.Speciality)
                    .Distinct()
                    .SingleOrDefault(e => e.ExternalId == dtoInstitutionSpeciality.SpecialityId);
                if (speciality == null)
                {
                    speciality = specialities.SingleOrDefault(e => e.ExternalId == dtoInstitutionSpeciality.SpecialityId);
                }

                if (speciality == null)
                {
                    speciality = new Speciality(
                        dtoInstitutionSpeciality.Speciality.Name,
                        dtoInstitutionSpeciality.SpecialityId,
                        dtoInstitutionSpeciality.Speciality.EducationalQualificationId,
                        dtoInstitutionSpeciality.Speciality.IsActive
                    );
                }
                else
                {
                    speciality.Update(
                        dtoInstitutionSpeciality.Speciality.Name,
                        dtoInstitutionSpeciality.SpecialityId,
                        dtoInstitutionSpeciality.Speciality.EducationalQualificationId,
                        dtoInstitutionSpeciality.Speciality.IsActive
                    );
                }

                var institutionSpeciality = model.InstitutionSpecialities
                    .SingleOrDefault(e => e.ExternalId == dtoInstitutionSpeciality.Id);
                if (institutionSpeciality != null)
                {
                    institutionSpeciality.SpecialityId = speciality.Id;
                    institutionSpeciality.Speciality = speciality;
                    institutionSpeciality.EducationalFormId = dtoInstitutionSpeciality.EducationalFormId;
                    institutionSpeciality.Duration = dtoInstitutionSpeciality.Duration;
                    institutionSpeciality.IsActive = dtoInstitutionSpeciality.IsActive;
                }
                else
                {
                    model.InstitutionSpecialities.Add(new InstitutionSpeciality {
                        ExternalId = dtoInstitutionSpeciality.Id,
                        Speciality = speciality,
                        SpecialityId = speciality.Id,
                        EducationalFormId = dtoInstitutionSpeciality.EducationalFormId,
                        Duration = dtoInstitutionSpeciality.Duration,
                        IsActive = dtoInstitutionSpeciality.IsActive
                    });
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SpecialityListItemDto>> GetInstitutionSpecialities(SpecialityFilterDto filter, CancellationToken cancellationToken)
        {
            var faculties = this.context.Set<Institution>()
                .Where(i => i.RootId == filter.EntityId)
                .AsQueryable();

            var institutionSpecialities = this.context.Set<InstitutionSpeciality>()
                .Where(i => faculties.Any(f => f.Id == i.InstitutionId))
                .AsNoTracking()
                .OrderBy(i => i.Speciality.Name)
                .GetFiltered(filter);

            if (filter.ResearchAreaId.HasValue)
            {
                institutionSpecialities = institutionSpecialities.Where(e => e.ResearchAreaId == filter.ResearchAreaId);
            }

            if (filter.EducationalQualificationId.HasValue)
            {
                institutionSpecialities = institutionSpecialities.Where(e => e.Speciality.EducationalQualificationId == filter.EducationalQualificationId);
            }

            if (filter.EducationFormId.HasValue)
            {
                institutionSpecialities = institutionSpecialities.Where(e => e.EducationalFormId == filter.EducationFormId);
            }

            filter.Limit = await institutionSpecialities.CountAsync(cancellationToken);

            if (filter.Limit.HasValue && filter.Offset.HasValue && filter.Offset == 0)
            {
                institutionSpecialities = institutionSpecialities.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
            }
            else
            {
                institutionSpecialities = institutionSpecialities.ApplyPagination(filter.Limit.Value, filter.Limit.Value);
            }

            var result = await institutionSpecialities
                .Select(i => new SpecialityListItemDto(i.Speciality.Id, i.Speciality.Name))
                .ToListAsync(cancellationToken);

            return result.GroupBy(s => s.Name).Select(e => e.First());
        }
    }
}
