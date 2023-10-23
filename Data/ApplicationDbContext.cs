using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.DataModels;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        //Entities
        public DbSet<Establishment> Establishment { get; set; }
        public DbSet<User> User { get; set; }

        public DbSet<Sale> Sale { get; set; }

        //Configurations
        public DbSet<UserLinkEstablishment> UserEstablishmentsLink { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new EstablishmentConfiguration());

        }

    }
}
