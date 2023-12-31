﻿using WebApplication1.Domain.Entities;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Entities.Establishment;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        //Entities
        public DbSet<Establishment> Establishment { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<User> User { get; set; }

        public DbSet<Information> information { get; set; }
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
