using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resc.Data.Lists;

namespace Resc.Persistence.Configurations
{
	public class SpecialityListItemConfiguration : IEntityTypeConfiguration<SpecialityListItem>
	{
		public void Configure(EntityTypeBuilder<SpecialityListItem> builder)
		{
			builder
				.HasMany(e => e.SpecialityMinisters)
				.WithOne(x => x.SpecialityListItem)
				.HasForeignKey(x => x.SpecialityListItemId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
