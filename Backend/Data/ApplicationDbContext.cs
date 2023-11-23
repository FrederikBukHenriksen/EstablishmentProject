using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Namotion.Reflection;
using System.Reflection;
using WebApplication1.Data.DataModels;
using WebApplication1.Models;
using WebApplication1.Repositories;
using static WebApplication1.Data.DataModels.UserRole;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext, IDatabaseContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        //Entities
        public DbSet<Establishment> Establishment { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<User> User { get; set; }
        //public DbSet<Sale> Sale { get; set; }
        //public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>()
                .HasOne<Establishment>()
                .WithMany(x => x.Items)
                .HasForeignKey("EstablishmentId")
                .IsRequired();

            //modelBuilder.Entity<Location>(entity =>
            //{
            //    entity.OwnsOne(l => l.Coordinates, coordinates =>
            //    { 
            //        coordinates.Property(c => c.Latitude).HasColumnName("Latitude");
            //        coordinates.Property(c => c.Longitude).HasColumnName("Longitude");
            //    });
            //});

            modelBuilder.Entity<Location>()
                .OwnsOne(l => l.Coordinates);
                
            ;

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new EstablishmentConfiguration());
            modelBuilder.ApplyConfiguration(new UserRolesConfiguration());
            modelBuilder.ApplyConfiguration(new ItemConfiguration());

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
