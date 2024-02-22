using Microsoft.EntityFrameworkCore;
using Resc.Application.Applications.Dtos.Create;
using Resc.Application.Common.Dtos;
using Resc.Application.Common.Interfaces;
using Resc.Application.Nomenclatures.Dtos;
using Resc.Data.Applications;
using Resc.Data.Applications.Register;
using Resc.Data.Nomenclatures;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Applications.Parts
{
	public class ActualEducationPartService : PartService<ActualEducationPart, ActualEducation>
	{
		public ActualEducationPartService(IAppDbContext context)
			:base(context)
		{

		}

        public async Task<PartDto<ActualEducationDto>> GetActualEducationPart(int partId, CancellationToken cancellationToken)
        {
            var part = await base.context.Set<ActualEducationPart>()
                .AsNoTracking()
                .Select(e => new PartDto<ActualEducationDto> {
                    Id = e.Id,
                    State = e.State,
                    Entity = new ActualEducationDto {
                        EducationalQualification = e.Entity.EducationalQualification != null
                                            ? new NomenclatureDto<EducationalQualification> {
                                                Id = e.Entity.EducationalQualification.Id,
                                                Name = e.Entity.EducationalQualification.Name
                                            }
                                            : null,
                        Status = e.Entity.Status,
                        CourseYear = e.Entity.CourseYear,
                        GraduationDate = e.Entity.GraduationDate,
                        EducationType = e.Entity.EducationType
                    }
                })
                .SingleOrDefaultAsync(e => e.Id == partId, cancellationToken);

            return part;
        }

        public async Task UpdateActualEducationPart(int partId, ActualEducationDto dto, CancellationToken cancellationToken)
        {
            var part = await base.context.Set<ActualEducationPart>()
                .Include(e => e.Entity)
                .SingleOrDefaultAsync(e => e.Id == partId, cancellationToken);

            part.Entity.Update(dto.EducationalQualification.Id, dto.Status, dto.CourseYear, dto.GraduationDate, dto.EducationType);

            await base.context.SaveChangesAsync(cancellationToken);
        }
    }
}
