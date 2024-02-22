using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resc.Data.Applications;

namespace Resc.Persistence.Configurations
{
	public class ContractConfiguration : IEntityTypeConfiguration<Contract>
	{
		public void Configure(EntityTypeBuilder<Contract> builder)
		{
			builder
				.HasMany(e => e.Contacts)
				.WithOne(x => x.Contract)
				.HasForeignKey(x => x.ContractId)
				.OnDelete(DeleteBehavior.Cascade);

			builder
				.HasOne(e => e.ContractFile)
				.WithOne(x => x.Contract)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
