using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

namespace WebApplication1.Models
{
    public class Location
    {
        public int Id { get; set; }
        public int EstablishmentId { get; set; }
        public Establishment Establishment { get; set; }
    }

    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(x => x.Establishment)
                .WithMany()
                .HasForeignKey(e => e.EstablishmentId);
        }
    }


}