using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure_Layer.Data.EntityTypeConfiguration;

namespace WebApplication1.Data
{

    public interface IDbContext
    {

    }

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //this.ChangeTracker.LazyLoadingEnabled = false;

            this.ChangeTracker.LazyLoadingEnabled = false;  // Enable lazy loading
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;



        }

        //Entities
        public DbSet<Establishment> Establishment { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<User> User { get; set; }
        //public DbSet<Price> Price { get; set; }


        public DbSet<EstablishmentInformation> information { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new EstablishmentConfiguration());
            modelBuilder.ApplyConfiguration(new UserRolesConfiguration());
            modelBuilder.ApplyConfiguration(new SaleConfiguration());
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new EstablishmentInformationConfiguration());




            modelBuilder.Entity<OpeningHours>()
                .Property(e => e.open)
                .HasConversion(
                    v => new DateTime(1, 1, 1, v.Hour, v.Minute, v.Second), // Adjust to the desired date
                    v => new NodaTime.LocalTime(v.Hour, v.Minute, v.Second)
                );

            modelBuilder.Entity<OpeningHours>()
                .Property(e => e.close)
                .HasConversion(
                    v => new DateTime(1, 1, 1, v.Hour, v.Minute, v.Second), // Adjust to the desired date
                    v => new NodaTime.LocalTime(v.Hour, v.Minute, v.Second)
                );






        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
