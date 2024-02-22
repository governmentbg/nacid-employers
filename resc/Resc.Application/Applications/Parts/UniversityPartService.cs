using Microsoft.EntityFrameworkCore;
using Resc.Application.Applications.Dtos.Create;
using Resc.Application.Common.Dtos;
using Resc.Application.Common.Interfaces;
using Resc.Application.Lists.Dtos;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Data.Applications;
using Resc.Data.Applications.Register;
using Resc.Data.Nomenclatures;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Applications.Parts
{
	public class UniversityPartService : PartService<UniversityPart, University>
	{
		public UniversityPartService(IAppDbContext context)
			: base(context)
		{

		}

		public async Task<PartDto<UniversityDto>> GetUniversityPart(int partId, CancellationToken cancellationToken)
		{
			var part = await base.context.Set<UniversityPart>()
				.AsNoTracking()
				.Select(e => new PartDto<UniversityDto> {
					Id = e.Id,
					State = e.State,
					Entity = new UniversityDto {
						Institution = e.Entity.Institution != null
											? new NomenclatureDto<Institution> {
												Id = e.Entity.Institution.Id,
												Name = e.Entity.Institution.Name
											}
											: null,
						SpecialityListItem = new SpecialitySelectDto {
							Id = e.Entity.SpecialityListItem.Id,
							ResearchArea = e.Entity.SpecialityListItem.ResearchArea != null
											? new NomenclatureDto<ResearchArea> {
												Id = e.Entity.SpecialityListItem.ResearchArea.Id,
												Name = e.Entity.SpecialityListItem.ResearchArea.Name
											}
											: null,
							Speciality = e.Entity.SpecialityListItem.Speciality != null
											? new NomenclatureDto<Speciality> {
												Id = e.Entity.SpecialityListItem.Speciality.Id,
												Name = e.Entity.SpecialityListItem.Speciality.Name
											}
											: null,
							EducationalQualification = e.Entity.SpecialityListItem.EducationalQualification != null
											? new NomenclatureDto<EducationalQualification> {
												Id = e.Entity.SpecialityListItem.EducationalQualification.Id,
												Name = e.Entity.SpecialityListItem.EducationalQualification.Name
											}
											: null,
							EducationFormType = e.Entity.SpecialityListItem.EducationFormType != null
											? new NomenclatureDto<EducationFormType> {
												Id = e.Entity.SpecialityListItem.EducationFormType.Id,
												Name = e.Entity.SpecialityListItem.EducationFormType.Name
											}
											: null,
						},
						Rector = e.Entity.Rector
					}
				})
				.SingleOrDefaultAsync(e => e.Id == partId, cancellationToken);

			return part;
		}

		public async Task UpdateUniversityPart(int partId, UniversityDto dto, CancellationToken cancellationToken)
		{
			var part = await base.context.Set<UniversityPart>()
				.Include(e => e.Entity)
				.SingleOrDefaultAsync(e => e.Id == partId, cancellationToken);

			part.Entity.Update(dto.Institution?.Id, dto.SpecialityListItem?.Id, dto.Rector);

			await base.context.SaveChangesAsync(cancellationToken);
		}
	}
}
