using Microsoft.EntityFrameworkCore;
using Resc.Application.Common.Interfaces;
using Resc.Data.Logs;

namespace Resc.Persistence
{
    public class AppLogContext : DbContext, IAppLogContext
    {
		public AppLogContext(DbContextOptions<AppLogContext> options)
			: base(options)
		{
		}

		public DbSet<Log> Logs { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (var entity in modelBuilder.Model.GetEntityTypes())
			{
				entity.SetTableName(entity.ClrType.Name.ToLower());

				foreach (var property in entity.GetProperties())
				{
					property.SetColumnName(property.Name.ToLower());
				}
			}

			base.OnModelCreating(modelBuilder);
		}
	}
}
