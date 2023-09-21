﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Models
{
    public class Location : EntityBase
    {
        public string Address { get; set; }

        public Establishment Establishment { get; set; }
    }

    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable(nameof(Location));  
            builder.HasKey(l => l.Id);

            builder.Property(e => e.Address);

            builder.HasOne(s => s.Establishment)
            .WithOne(a => a.LocationId)
            .HasPrincipalKey<Location>(a => a.Id)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }


}