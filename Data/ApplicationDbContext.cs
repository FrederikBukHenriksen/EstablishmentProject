using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Metadata;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.ApplyConfiguration(new LocationConfiguration());
        //}
    public DbSet<Establishment> Establishment { get; set; }
        //public DbSet<Location> Location { get; set; }


    }
}
