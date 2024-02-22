using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resc.Data.Applications;

namespace Resc.Persistence.Configurations
{
	public class ApplicationTerminationConfiguration : IEntityTypeConfiguration<ApplicationTermination>
	{
		public void Configure(EntityTypeBuilder<ApplicationTermination> builder)
		{
			builder
				.HasOne(e => e.AnnexFile)
				.WithOne(x => x.ApplicationTermination)
				.HasForeignKey<AnnexFile>(x => x.ApplicationTerminationId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
