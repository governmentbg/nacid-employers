using Microsoft.EntityFrameworkCore;
using Resc.Application.Applications.Dtos.Create;
using Resc.Application.Common.Dtos;
using Resc.Application.Common.Interfaces;
using Resc.Data.Applications.Register;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Applications.Parts
{
	public class StudentPartService : PartService<StudentPart, Student>
	{
		public StudentPartService(IAppDbContext context)
			:base(context)
		{

		}

		public async Task<PartDto<StudentDto>> GetStudentPart(int partId, CancellationToken cancellationToken)
		{
			var part = await base.context.Set<StudentPart>()
				.AsNoTracking()
				.Select(e => new PartDto<StudentDto> {
					Id = e.Id,
					State = e.State,
					Entity = new StudentDto {
						FirstName = e.Entity.FirstName,
						MiddleName = e.Entity.MiddleName,
						LastName = e.Entity.LastName,
						UIN = e.Entity.UIN,
						Email = e.Entity.Email,
						PhoneNumber = e.Entity.PhoneNumber,
						EducationType = e.Entity.EducationType,
						Status = e.Entity.Status,
						GraduationDate = e.Entity.GraduationDate
					}
				})
				.SingleOrDefaultAsync(e => e.Id == partId, cancellationToken);

			return part;
		}

		public async Task UpdateStudentPart(int partId, StudentDto dto, CancellationToken cancellationToken)
		{
			var part = await base.context.Set<StudentPart>()
				.Include(e => e.Entity)
				.SingleOrDefaultAsync(e => e.Id == partId, cancellationToken);

			part.Entity.Update(dto.FirstName, dto.MiddleName, dto.LastName, dto.UIN, dto.Email, dto.PhoneNumber, dto.EducationType, dto.Status, dto.GraduationDate);

			await base.context.SaveChangesAsync(cancellationToken);
		}
	}
}
