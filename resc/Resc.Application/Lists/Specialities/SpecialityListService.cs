using Microsoft.EntityFrameworkCore;
using Resc.Application.Common.Extensions;
using Resc.Application.Common.Interfaces;
using Resc.Application.DomainValidations;
using Resc.Application.DomainValidations.Enums;
using Resc.Application.InstitutionSpecialities.Dtos;
using Resc.Application.Lists.Dtos;
using Resc.Application.Lists.Extensions;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Data.Lists;
using Resc.Data.Nomenclatures;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Resc.Application.Common.Dtos;

namespace Resc.Application.Lists.Specialities
{
    public class SpecialityListService : ListService<SpecialityList, SpecialityListItem>
    {
        public SpecialityListService(IAppDbContext context, DomainValidationService validation)
            : base(context, validation)
        {

        }

        public async Task<SpecialityList> GetListAsync(BaseListFilter filter, CancellationToken cancellationToken)
        {
            var list = await base.context.Set<SpecialityList>()
                .Include(e => e.Items)
                    .ThenInclude(e => e.EducationalQualification)
                .Include(e => e.Items)
                    .ThenInclude(e => e.EducationFormType)
                .Include(e => e.Items)
                    .ThenInclude(e => e.SpecialityMinisters)
                        .ThenInclude(e => e.Minister)
                .Include(e => e.Items)
                    .ThenInclude(e => e.Speciality)
                .Include(e => e.Items)
                    .ThenInclude(e => e.Institution)
                .Include(e => e.Items)
                    .ThenInclude(e => e.ResearchArea)
                .Include(e => e.SchoolYear)
                .Where(el => filter.SchoolYearId.HasValue ? el.SchoolYear.Id == filter.SchoolYearId : el.SchoolYear.IsCurrent)
                .SingleOrDefaultAsync(cancellationToken);

            list.Items = list.Items
                .OrderBy(x => x.ResearchArea.Name)
                    .ThenBy(x => x.Institution.Name)
                        .ThenBy(x => x.Speciality.Name);

            return list;
        }

        public async Task<SpecialityListItem> GetItemAsync(int specialityId, CancellationToken cancellationToken)
        {
            var item = await base.context.Set<SpecialityListItem>()
                .Include(e => e.EducationalQualification)
                .Include(e => e.EducationFormType)
                .Include(e => e.SpecialityMinisters)
                    .ThenInclude(e => e.Minister)
                .Include(e => e.Speciality)
                .Include(e => e.Institution)
                .Include(e => e.ResearchArea)
                .Where(s => s.Id == specialityId)
                .SingleOrDefaultAsync(cancellationToken);

            return item;
        }

        public async Task<SpecialityListItem> AddItemAsync(SpecialityListItem model, CancellationToken cancellationToken)
        {
            var item = await base.context.Set<SpecialityListItem>()
                .AsNoTracking()
                .Where(s =>
                    s.InstitutionId == model.Institution.Id &&
                    s.ResearchAreaId == model.ResearchArea.Id &&
                    s.EducationalQualificationId == model.EducationalQualification.Id &&
                    s.EducationFormTypeId == model.EducationFormType.Id &&
                    s.SpecialityId == model.Speciality.Id &&
                    s.SpecialityListId == model.SpecialityListId)
                .SingleOrDefaultAsync(cancellationToken);

            if (item != null)
            {
                base.validation.ThrowErrorMessage(ListsErrorCode.Item_Already_Exists);
            }

            var newItem = new SpecialityListItem {
                SpecialityListId = model.SpecialityListId,
                SpecialityId = model.Speciality.Id,
                ResearchAreaId = model.ResearchArea.Id,
                InstitutionId = model.Institution.Id,
                EducationalQualificationId = model.EducationalQualification.Id,
                EducationFormTypeId = model.EducationFormType.Id,
                StudentsCount = model.StudentsCount
            };

            foreach (var minister in model.SpecialityMinisters)
            {
                newItem.SpecialityMinisters.Add(new SpecialityMinister { MinisterId = minister.Minister.Id });
            }

            await base.context.Set<SpecialityListItem>().AddAsync(newItem);
            await base.context.SaveChangesAsync(cancellationToken);

            return model;
        }

        public async Task EditItemAsync(SpecialityListItem model, CancellationToken cancellationToken)
        {
            var specialityListItem = await base.context.Set<SpecialityListItem>()
                .Include(e => e.SpecialityMinisters)
                .SingleAsync(x => x.Id == model.Id);

            specialityListItem.Update(model.EducationalQualification.Id, model.EducationFormType.Id, model.Institution.Id, model.Speciality.Id, model.ResearchArea.Id);

            var ministersToAdd = model.SpecialityMinisters.Where(e => !specialityListItem.SpecialityMinisters.Select(t => t.MinisterId).Contains(e.Minister.Id));
            foreach (var minister in ministersToAdd)
            {
                var specialityMinister = new SpecialityMinister {
                    MinisterId = minister.Minister.Id
                };

                specialityListItem.SpecialityMinisters.Add(specialityMinister);

            }

            var ministersToRemove = specialityListItem.SpecialityMinisters.Where(e => !model.SpecialityMinisters.Select(t => t.Minister.Id).Contains(e.MinisterId));
            foreach (var minister in ministersToRemove)
            {
                this.context.Set<SpecialityMinister>().Remove(minister);
            }

            await base.context.SaveChangesAsync(cancellationToken);
        }

        public async Task<SearchResultItemDto<SpecialityListItem>> GetItemsFiltered(SpecialityListFilter filter, CancellationToken cancellationToken)
        {
            IQueryable<SpecialityListItem> query = base.context.Set<SpecialityListItem>()
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
                .ToListAsync(cancellationToken);

            var result = new SearchResultItemDto<SpecialityListItem> {
                Items = items,
                TotalCount = null
            };

            return result;
        }

        public async Task<IEnumerable<SpecialitySelectDto>> SelectItemsAsync(SpecialityFilterDto filter, CancellationToken cancellationToken)
        {
            var query = base.context.Set<SpecialityList>()
                .AsNoTracking()
                .Include(e => e.Items)
                    .ThenInclude(e => e.EducationalQualification)
                .Include(e => e.Items)
                    .ThenInclude(e => e.EducationFormType)
                .Include(e => e.Items)
                    .ThenInclude(e => e.ResearchArea)
                .Include(e => e.Items)
                    .ThenInclude(e => e.Speciality);

            var list = new SpecialityList();

            if (!filter.SchoolYearId.HasValue)
            {
                list = await query
                    .Where(e => e.IsPublished)
                    .OrderByDescending(e => e.Id)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                list = await query
                    .Where(e => e.SchoolYearId == filter.SchoolYearId)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            var items = list.Items.AsQueryable().GetFiltered(filter, false, false);

            filter.Limit = items.Count();

            if (filter.InstitutionId.HasValue)
            {
                items = items.Where(e => e.InstitutionId == filter.InstitutionId);
            }

            if (filter.ResearchAreaId.HasValue)
            {
                items = items.Where(e => e.ResearchAreaId == filter.ResearchAreaId);
            }

            if (filter.EducationalQualificationId.HasValue)
            {
                items = items.Where(e => e.EducationalQualificationId == filter.EducationalQualificationId);
            }

            if (filter.EducationFormId.HasValue)
            {
                items = items.Where(e => e.EducationFormTypeId == filter.EducationFormId);
            }

            if (filter.Limit.HasValue && filter.Offset.HasValue && filter.Offset == 0)
            {
                items = items.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
            }
            else
            {
                items = items.ApplyPagination(filter.Limit.Value, filter.Limit.Value);
            }

            var result = items
                .Select(SpecialitySelectDto.SelectExpression)
                .ToList();

            return result.GroupBy(s => s.Name).Select(e => e.First());
        }

        public async Task<IEnumerable<SpecialitySelectDto>> SelectReportItemsAsync(SpecialityFilterDto filter, CancellationToken cancellationToken)
		{
            var query = await base.context.Set<SpecialityList>()
                .AsNoTracking()
                .Include(e => e.Items)
                    .ThenInclude(e => e.EducationalQualification)
                .Include(e => e.Items)
                    .ThenInclude(e => e.EducationFormType)
                .Include(e => e.Items)
                    .ThenInclude(e => e.ResearchArea)
                .Include(e => e.Items)
                    .ThenInclude(e => e.Speciality)
                .Where(e => e.IsPublished && e.SchoolYearId == filter.SchoolYearId)
                .OrderByDescending(e => e.Id)
                .FirstOrDefaultAsync(cancellationToken);

            var items = query.Items.AsQueryable().GetFiltered(filter, false, false);

            filter.Limit = items.Count();

            if (filter.InstitutionId.HasValue)
            {
                items = items.Where(e => e.InstitutionId == filter.InstitutionId);
            }

            if (filter.ResearchAreaId.HasValue)
            {
                items = items.Where(e => e.ResearchAreaId == filter.ResearchAreaId);
            }

            if (filter.EducationalQualificationId.HasValue)
            {
                items = items.Where(e => e.EducationalQualificationId == filter.EducationalQualificationId);
            }

            if (filter.EducationFormId.HasValue)
            {
                items = items.Where(e => e.EducationFormTypeId == filter.EducationFormId);
            }

            if (filter.Limit.HasValue && filter.Offset.HasValue && filter.Offset == 0)
            {
                items = items.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
            }
            else
            {
                items = items.ApplyPagination(filter.Limit.Value, filter.Limit.Value);
            }

            var result = items
                .Select(SpecialitySelectDto.SelectExpression)
                .ToList();

            return result.GroupBy(s => s.Name).Select(e => e.First());
        }

        public async Task<IEnumerable<NomenclatureDto<Institution>>> SelectUniversitiesAsync(SpecialityFilterDto filter, CancellationToken cancellationToken)
        {
            var query = base.context.Set<SpecialityListItem>()
                .AsNoTracking()
                .Include(e => e.Institution)
                .Where(e => e.SpecialityList.SchoolYearId == filter.EntityId && e.SpecialityList.IsPublished)
                .GetFiltered(filter, false, true);

            filter.Limit = await query.CountAsync(cancellationToken);

            if (filter.Limit.HasValue && filter.Offset.HasValue && filter.Offset == 0)
            {
                query = query.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
            }
            else
            {
                query = query.ApplyPagination(filter.Limit.Value, filter.Limit.Value);
            }

            var result = await query
                .Select(e => new NomenclatureDto<Institution> {
                    Id = e.Institution.Id,
                    Name = e.Institution.Name
                })
                .Distinct()
                .OrderBy(e => e.Name)
                .ToListAsync(cancellationToken);

            return result;
        }

        public async Task<IEnumerable<NomenclatureDto<ResearchArea>>> SelectResearchAreasAsync(SpecialityFilterDto filter, CancellationToken cancellationToken)
        {
            var query = base.context.Set<SpecialityListItem>()
                .AsNoTracking()
                .Include(e => e.ResearchArea)
                .Where(e => e.SpecialityList.SchoolYearId == filter.EntityId && e.SpecialityList.IsPublished)
                .GetFiltered(filter, true, false);

            filter.Limit = await query.CountAsync(cancellationToken);

            if (filter.Limit.HasValue && filter.Offset.HasValue && filter.Offset == 0)
            {
                query = query.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
            }
            else
            {
                query = query.ApplyPagination(filter.Limit.Value, filter.Limit.Value);
            }

            var result = await query
                .Select(e => new NomenclatureDto<ResearchArea> {
                    Id = e.ResearchArea.Id,
                    Name = e.ResearchArea.Name
                })
                .Distinct()
                .OrderBy(e => e.Name)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
