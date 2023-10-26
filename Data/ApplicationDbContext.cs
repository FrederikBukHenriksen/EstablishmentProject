using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.DataModels;
using WebApplication1.Models;
using WebApplication1.Repositories;
using static WebApplication1.Data.DataModels.UserRole;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext, IDatabaseContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
        }

        //Entities
        //public DbSet<Establishment> Establishment { get; set; }
        //public DbSet<User> User { get; set; }
        //public DbSet<Sale> Sale { get; set; }
        //public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new EstablishmentConfiguration());
            modelBuilder.ApplyConfiguration(new UserRolesConfiguration());

        }
    }
}
