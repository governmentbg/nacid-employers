using FileStorageNetCore.Models;
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
	public class ContractPartService : PartService<ContractPart, Contract>
	{
		public ContractPartService(IAppDbContext context)
			:base(context)
		{

		}

		public async Task<PartDto<ContractDto>> GetContractPart(int partId, CancellationToken cancellationToken)
		{
			var part = await base.context.Set<ContractPart>()
				.AsNoTracking()
				.Select(e => new PartDto<ContractDto> {
                    Id = e.Id,
                    State = e.State,
                    Entity = new ContractDto {
                        SigningDate = e.Entity.SigningDate,
                        EndDate = e.Entity.EndDate,
                        Number = e.Entity.Number,
                        Term = e.Entity.Term,
                        EmploymentTerm = e.Entity.EmploymentTerm,
                        TaxType = e.Entity.TaxType,
                        AttachedFile = new AttachedFile {
                            Key = e.Entity.ContractFile.Key,
                            Hash = e.Entity.ContractFile.Hash,
                            Size = e.Entity.ContractFile.Size,
                            Name = e.Entity.ContractFile.Name,
                            MimeType = e.Entity.ContractFile.MimeType,
                            DbId = e.Entity.ContractFile.DbId,
                        },
                        Contacts = e.Entity.Contacts.Select(s => new ContactPersonDto {
                            Name = s.Name,
                            Email = s.Email,
                            PhoneNumber = s.PhoneNumber,
                            Type = s.Type
                        })
                    }
                })
				.SingleOrDefaultAsync(e => e.Id == partId, cancellationToken);

			return part;
		}

        public async Task UpdateContractPart(int partId, ContractDto dto, CancellationToken cancellationToken)
		{
            var part = await base.context.Set<ContractPart>()
                .Include(e => e.Entity)
                    .ThenInclude(e => e.ContractFile)
                .Include(e => e.Entity.Contacts)
                .SingleOrDefaultAsync(e => e.Id == partId, cancellationToken);

            part.Entity.Update(dto.SigningDate, dto.EndDate, dto.Number, dto.Term, dto.EmploymentTerm, dto.TaxType, dto.AttachedFile);

			foreach (var contact in dto.Contacts)
			{
                part.Entity.UpdateContact(contact.Id, contact.Name, contact.PhoneNumber, contact.Email, contact.Type);
			}

            await base.context.SaveChangesAsync(cancellationToken);
        }

		protected override IQueryable<ContractPart> LoadPart()
		{
            return base.context.Set<ContractPart>()
                .Include(e => e.Entity)
                .Include(e => e.Entity.Contacts)
                .Include(e => e.Entity.ContractFile);
		}
	}
}
