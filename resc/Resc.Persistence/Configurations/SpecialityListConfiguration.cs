using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resc.Data.Lists;

namespace Resc.Persistence.Configurations
{
    public class SpecialityListConfiguration : IEntityTypeConfiguration<SpecialityList>
    {
        public void Configure(EntityTypeBuilder<SpecialityList> builder)
        {
            builder
                .HasMany(x => x.Items)
                .WithOne(e => e.SpecialityList)
                .HasForeignKey(e => e.SpecialityListId);
        }
    }
}
