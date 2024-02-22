using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resc.Data.Applications;

namespace Resc.Persistence.Configurations
{
	public class ApplicationModificationConfiguration : IEntityTypeConfiguration<ApplicationModification>
	{
		public void Configure(EntityTypeBuilder<ApplicationModification> builder)
		{
			builder
				.HasOne(e => e.AnnexFile)
				.WithOne(x => x.ApplicationModification)
				.HasForeignKey<AnnexFile>(x => x.ApplicationModificationId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
