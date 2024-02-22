using Microsoft.EntityFrameworkCore;
using Resc.Application.Common.Dtos;
using Resc.Application.Common.Extensions;
using Resc.Application.Common.Interfaces;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Application.Nomenclatures.Extensions;
using Resc.Data.Nomenclatures;
using Resc.Data.Nomenclatures.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resc.Application.InstitutionSpecialities.Services
{
	public class InstitutionService
	{
		private readonly IAppDbContext context;

		public InstitutionService(IAppDbContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<TDto>> GetUniversities<TFilter, TDto>(NomenclatureFilterDto<Institution> filter)
			where TDto : IMapping<Institution, TDto>, new()
		{
			IQueryable<Institution> query;

			if (filter.EntityId == null)
			{
				query = this.context.Set<Institution>()
				.AsNoTracking()
				.Where(u => u.Level == Level.First && u.InstitutionOwnershipTypeId == 1 && u.IsActive == true && (u.InstitutionTypeId == 7 || u.InstitutionTypeId == 33))
				.GetFiltered(filter)
				.ApplyOrder(filter.Orders);
			}
			else
			{
				var faculties = this.context.Set<Institution>()
				.AsNoTracking()
				.Include(i => i.InstitutionSpecialities)
				.Where(u => u.InstitutionSpecialities.Any(i => i.ResearchAreaId == filter.EntityId));

				query = this.context.Set<Institution>()
						.Where(u => faculties.Any(f => f.ParentId == u.Id))
						.GetFiltered(filter)
						.ApplyOrder(filter.Orders);
			}

			if (filter.Limit.HasValue && filter.Offset.HasValue)
			{
				query = query.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
			}

			var result = await query.Select(new TDto().Map())
				.ToListAsync();

			return result;
		}

		public async Task<IEnumerable<TDto>> GetFaculties<TFilter, TDto>(NomenclatureFilterDto<Institution> filter)
			where TDto : IMapping<Institution, TDto>, new()
		{

			IQueryable<int> uniQuery = this.context.Set<Institution>()
				.AsNoTracking()
				.Where(u => u.Id == filter.EntityId)
				.Select(e => e.RootId);
			var query = context.Set<Institution>()
				.Where(f => uniQuery.Contains(f.RootId) && (f.Level == Level.Second || f.Level == Level.Third))
				.GetFiltered(filter)
				.ApplyOrder(filter.Orders);

			if (filter.Limit.HasValue && filter.Offset.HasValue)
			{
				query = query.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
			}

			var result = await query.Select(new TDto().Map())
				.ToListAsync();

			return result;
		}

		public async Task<IEnumerable<TDto>> GetFacultiesByEducationalQualification<TFilter, TDto>(NomenclatureFilterDto<Institution> filter, int educationalQualificationId)
			where TDto : IMapping<Institution, TDto>, new()
		{
			IQueryable<int> uniQuery = this.context.Set<Institution>()
				.AsNoTracking()
				.Where(u => u.Id == filter.EntityId)
				.Select(e => e.RootId);
			var query = context.Set<Institution>()
				.Where(f => uniQuery.Contains(f.RootId) && (f.Level == Level.Second || f.Level == Level.Third) && f.InstitutionSpecialities.Any(x => x.Speciality.EducationalQualificationId == educationalQualificationId))
				.GetFiltered(filter)
				.ApplyOrder(filter.Orders);

			if (filter.Limit.HasValue && filter.Offset.HasValue)
			{
				query = query.ApplyPagination(filter.Offset.Value, filter.Limit.Value);
			}

			var result = await query.Select(new TDto().Map())
				.ToListAsync();

			return result;
		}

		public async Task<Institution> GetSingleInstitutionByName(string institutionName)
		{
			var institution = await this.context.Set<Institution>()
				.AsNoTracking()
				.SingleOrDefaultAsync(u => u.Name == institutionName);

			return institution;
		}

		public async Task<TDto> GetSingle<TFilter, TDto>(NomenclatureFilterDto<Institution> filter, string institutionName)
			where TDto : IMapping<Institution, TDto>, new()
		{
			var query = context.Set<Institution>()
				.Where(u => u.Name == institutionName)
				.GetFiltered(filter)
				.ApplyOrder(filter.Orders);

			var result = await query.Select(new TDto().Map())
				.SingleOrDefaultAsync();

			return result;
		}

	}
}
