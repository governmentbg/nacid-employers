using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Resc.Application.Applications;
using Resc.Application.Common.Interfaces;
using Resc.Data;
using Resc.Data.Applications;
using Resc.Data.Applications.Register;
using Resc.Data.Common.Interfaces;
using Resc.Data.Emails;
using Resc.Data.Lists;
using Resc.Data.Nomenclatures;
using Resc.Data.Users;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resc.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {

		private readonly IUserContext userContext;

		public AppDbContext(
			IUserContext userContext,
			DbContextOptions<AppDbContext> options)
			: base(options)
		{
			this.userContext = userContext;
		}

		#region Lists
		public DbSet<SpecialityListItem> SpecialityListItem { get; set; }
		public DbSet<SpecialityList> SpecialityList { get; set; }
		public DbSet<EmployerListItem> EmployerListItem { get; set; }
		public DbSet<EmployerList> EmployerList { get; set; }
		public DbSet<EmployerSpeciality> EmployerSpecialities { get; set; }
		public DbSet<SpecialityMinister> SpecialityMinisters { get; set; }
		#endregion

		#region Users

		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<PasswordToken> PasswordTokens { get; set; }

		public DbSet<Email> Emails { get; set; }
		public DbSet<EmailAddressee> EmailAddressees { get; set; }

		#endregion

		#region Nomenclatures

		public DbSet<EducationFormType> EducationFormTypes { get; set; }
		public DbSet<EmailType> EmailTypes { get; set; }
		public DbSet<Institution> Institutions { get; set; }
		public DbSet<Speciality> Specialties { get; set; }
		public DbSet<InstitutionSpeciality> InstitutionSpecialities { get; set; }
		public DbSet<EducationalQualification> EducationalQualifications { get; set; }
		public DbSet<ResearchArea> ResearchAreas { get; set; }
		public DbSet<Minister> Minister { get; set; }
		public DbSet<SchoolYear> SchoolYear { get; set; }
		public DbSet<City> Cities { get; set; }
		public DbSet<InstitutionOwnershipType> InstitutionOwnershipTypes { get; set; }
		public DbSet<TerminationReason> TerminationReasons { get; set; }
		public DbSet<RegisterIndex> RegisterIndices { get; set; }
		public DbSet<RegisterIndexCounter> RegisterIndexCounters { get; set; }
		public DbSet<FileTemplate> FileTemplates { get; set; }

		#endregion

		#region Applications

		public DbSet<Student> StudentParts { get; set; }
		public DbSet<University> Universities { get; set; }
		public DbSet<Employer> Employers { get; set; }
		public DbSet<Contract> Contracts { get; set; }
		public DbSet<ApplicationModification> ApplicationModifications { get; set; }
		public DbSet<ApplicationTermination> ApplicationTerminations { get; set; }
		public DbSet<ApplicationStatusHistory> ApplicationStatusHistories { get; set; }

		#endregion

		#region Register Applications

		public DbSet<ApplicationLot> ApplicationLots { get; set; }
		public DbSet<ApplicationCommit> ApplicationCommits { get; set; }
		public DbSet<StudentPart> StudentsParts { get; set; }
		public DbSet<UniversityPart> UniversityParts { get; set; }
		public DbSet<EmployerPart> EmployerParts { get; set; }
		public DbSet<ContractPart> ContractParts { get; set; }

		#endregion

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (var entity in modelBuilder.Model.GetEntityTypes())
			{
				// Configure name mappings
				entity.SetTableName(entity.ClrType.Name.ToLower());

				if (typeof(IEntity).IsAssignableFrom(entity.ClrType))
				{
					modelBuilder.Entity(entity.ClrType)
						.HasKey(nameof(IEntity.Id));
				}

				if (typeof(IConcurrency).IsAssignableFrom(entity.ClrType))
				{
					modelBuilder.Entity(entity.ClrType)
						.Property(nameof(IConcurrency.Version))
						.IsConcurrencyToken()
						.HasDefaultValue(0);
				}

				entity.GetProperties()
					.ToList()
					.ForEach(e => e.SetColumnName(e.Name.ToLower()));

				entity.GetForeignKeys()
					.Where(e => !e.IsOwnership && e.DeleteBehavior == DeleteBehavior.Cascade)
					.ToList()
					.ForEach(e => e.DeleteBehavior = DeleteBehavior.Restrict);
			}

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		}

		public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
		{
			return Database.BeginTransactionAsync(cancellationToken);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			foreach (var entry in ChangeTracker.Entries())
			{
				if (typeof(IAuditable).IsAssignableFrom(entry.Entity.GetType()) && entry.State == EntityState.Added)
				{
					var entity = entry.Entity as IAuditable;
					entity.CreateDate = DateTime.Now;
					entity.CreatorUserId = this.userContext.UserId;
				}

				if (typeof(IConcurrency).IsAssignableFrom(entry.Entity.GetType()) && entry.State == EntityState.Modified)
				{
					var entity = entry.Entity as IConcurrency;
					entity.Version++;
				}
			}

			return base.SaveChangesAsync(cancellationToken);
		}
	}
}
