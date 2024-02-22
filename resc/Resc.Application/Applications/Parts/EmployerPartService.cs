using Microsoft.EntityFrameworkCore;
using Resc.Application.Applications.Dtos.Create;
using Resc.Application.Common.Dtos;
using Resc.Application.Common.Interfaces;
using Resc.Data.Applications;
using Resc.Data.Applications.Register;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Application.Applications.Parts
{
	public class EmployerPartService : PartService<EmployerPart, Employer>
	{
		public EmployerPartService(IAppDbContext context)
			:base(context)
		{

		}

		public async Task<PartDto<EmployerDto>> GetEmployerPart(int partId, CancellationToken cancellationToken)
		{
			var part = await base.context.Set<EmployerPart>()
				.AsNoTracking()
				.Select(e => new PartDto<EmployerDto> {
					Id = e.Id,
					State = e.State,
					Entity = new EmployerDto {
						Representative = e.Entity.Representative,
						Email = e.Entity.Email,
						PhoneNumber = e.Entity.PhoneNumber,
						EmployerListItem = e.Entity.EmployerListItem
					}
				})
				.SingleOrDefaultAsync(e => e.Id == partId, cancellationToken);

			return part;
		}

		public async Task UpdateEmployerPart(int partId, EmployerDto dto, CancellationToken cancellationToken)
		{
			var part = await base.context.Set<EmployerPart>()
				.Include(e => e.Entity)
				.SingleOrDefaultAsync(e => e.Id == partId, cancellationToken);

			part.Entity.Update(dto.EmployerListItem.Id, dto.Representative, dto.Email, dto.PhoneNumber);

			await base.context.SaveChangesAsync(cancellationToken);
		}
	}
}
