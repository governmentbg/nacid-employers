using Microsoft.EntityFrameworkCore;
using Resc.Application.Common.Interfaces;
using Resc.Application.DomainValidations;
using Resc.Application.DomainValidations.Enums;
using Resc.Application.Lists.Dtos;
using Resc.Data.Lists;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Resc.Application.Common.Dtos;
using Resc.Application.InstitutionSpecialities.Dtos;
using Resc.Application.Lists.Extensions;

namespace Resc.Application.Lists.Employer
{
    public class EmployerListService : ListService<EmployerList, EmployerListItem>
    {
        public EmployerListService(IAppDbContext context, DomainValidationService validation)
            : base(context, validation)
        {

        }

        public async Task<EmployerList> GetListAsync(BaseListFilter filter, CancellationToken cancellationToken)
        {
            var list = await base.context.Set<EmployerList>()
                .Include(e => e.Items)
                    .ThenInclude(e => e.Specialities)
                        .ThenInclude(e => e.Speciality)
                .Include(e => e.Items)
                    .ThenInclude(e => e.City)
                .Include(e => e.SchoolYear)
                .Where(el => filter.SchoolYearId.HasValue ? el.SchoolYear.Id == filter.SchoolYearId : el.SchoolYear.IsCurrent)
                .SingleOrDefaultAsync(cancellationToken);

            return list;
        }

        public async Task<EmployerListItem> GetItemAsync(int employerId, CancellationToken cancellationToken)
        {
            var employer = await base.context.Set<EmployerListItem>()
                .Include(e => e.Specialities)
                    .ThenInclude(e => e.Speciality)
                .Include(e => e.City)
                .Where(e => e.Id == employerId)
                .SingleOrDefaultAsync(cancellationToken);

            return employer;
        }

        public async Task<EmployerListItem> AddItemAsync(EmployerListItem model, CancellationToken cancellationToken)
        {
            var item = await base.context.Set<EmployerListItem>()
                .Where(e => e.Bulstat == model.Bulstat && e.EmployerListId == model.EmployerListId)
                .SingleOrDefaultAsync(cancellationToken);

            if(item != null)
            {
                base.validation.ThrowErrorMessage(ListsErrorCode.Item_Already_Exists);
            }

            model.FullAddress = string.Join(", ", new List<string> { model.City.Name, model.Address }.Where(e => !string.IsNullOrWhiteSpace(e)));

            base.context.Entry(model).State = EntityState.Added;
            foreach (var spec in model.Specialities)
            {
                base.context.Entry(spec).State = EntityState.Added;
            }

            await base.context.SaveChangesAsync(cancellationToken);

            return model;
        }

        public async Task EditItemAsync(EmployerListItem model, CancellationToken cancellationToken)
        {
            var specialities = await base.context.Set<EmployerSpeciality>()
                .AsNoTracking()
                .Where(e => e.ItemId == model.Id)
                .ToListAsync(cancellationToken);

            var specialitiesForAdd = model.Specialities.Where(e => !specialities.Select(ss => ss.Id).Contains(e.Id));
            foreach (var spec in specialitiesForAdd)
            {
                base.context.Entry(spec).State = EntityState.Added;
            }

            var specialitiesForUpdate = model.Specialities.Where(e => specialities.Select(ss => ss.Id).Contains(e.Id));
            foreach (var spec in specialitiesForUpdate)
            {
                spec.SpecialityId = spec.Speciality.Id;

                base.context.Entry(spec).State = EntityState.Modified;
            }

            var specialitiesForRemove = specialities.Where(e => !model.Specialities.Select(ss => ss.Id).Contains(e.Id));
            foreach (var spec in specialitiesForRemove)
            {
                base.context.Entry(spec).State = EntityState.Deleted;
            }

            model.CityId = model.City.Id;

            base.context.Entry(model).State = EntityState.Modified;
            await base.context.SaveChangesAsync(cancellationToken);
        }

        public async Task<SearchResultItemDto<EmployerListItem>> GetItemsFiltered(EmployerListFilter filter, CancellationToken cancellationToken)
        {
            IQueryable<EmployerListItem> query = base.context.Set<EmployerListItem>()
                .AsNoTracking()
                .Include(s => s.EmployerList)
                .Include(s => s.Specialities)
                    .ThenInclude(s => s.Speciality)
                .Include(s => s.City)
                .Where(s => s.EmployerList.SchoolYearId == filter.SchoolYearId && s.EmployerList.IsPublished);

            if (!string.IsNullOrWhiteSpace(filter.SpecialityName))
            {
                query = query.Where(s => s.Specialities.Any(s => s.Speciality.Name.Trim().ToLower().Contains(filter.SpecialityName.Trim().ToLower())));
            }

            if (!string.IsNullOrWhiteSpace(filter.Bulstat))
            {
                query = query.Where(s => s.Bulstat.Trim().ToLower().Contains(filter.Bulstat.Trim().ToLower()));
            }

            if (filter.CompanyId.HasValue)
            {
                query = query.Where(s => s.Id == filter.CompanyId);
            }

            var items = await query
                .ToListAsync(cancellationToken);

            var result = new SearchResultItemDto<EmployerListItem> {
                Items = items,
                TotalCount = null
            };

            return result;
        }

        public async Task<IEnumerable<EmployerListItem>> SelectItemsAsync(EmployerFilterDto filter, CancellationToken cancellationToken)
        {
            var query = await base.context.Set<EmployerList>()
                .AsNoTracking()
                .Include(e => e.Items)
                    .ThenInclude(i => i.City)
                .Include(e => e.Items)
                    .ThenInclude(e => e.Specialities)
                        .ThenInclude(e => e.Speciality)
                .Where(e => e.IsPublished)
                .OrderByDescending(e => e.Id)
                .FirstOrDefaultAsync(cancellationToken);

            var items = query.Items.AsQueryable().GetFiltered(filter);

            if(!string.IsNullOrWhiteSpace(filter.Speciality))
            {
                items = items.Where(e => e.Specialities.Any(s => s.Speciality.Name.Trim().ToLower().Contains(filter.Speciality.Trim().ToLower())));
            }

            var result = items.ToList();

            return result;
        }
    }
}
