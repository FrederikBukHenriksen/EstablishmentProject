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
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>()
                .HasOne<Establishment>()
                .WithMany(x => x.Items)
                .HasForeignKey("EstablishmentId")
                .IsRequired();

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            //modelBuilder.ApplyConfiguration(new EstablishmentConfiguration());
            modelBuilder.ApplyConfiguration(new UserRolesConfiguration());
            //TestDataSeeder.SeedDataBase(modelBuilder);

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
