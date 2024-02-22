using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resc.Data.Lists;

namespace Resc.Persistence.Configurations
{
    public class EmployerListConfiguration : IEntityTypeConfiguration<EmployerList>
    {
        public void Configure(EntityTypeBuilder<EmployerList> builder)
        {
            builder
                .HasMany(x => x.Items)
                .WithOne(e => e.EmployerList)
                .HasForeignKey(e => e.EmployerListId);
        }
    }

    public class EmployerDataConfiguration : IEntityTypeConfiguration<EmployerListItem>
    {
        public void Configure(EntityTypeBuilder<EmployerListItem> builder)
        {
            builder
                .HasMany(x => x.Specialities)
                .WithOne(x => x.Item)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
