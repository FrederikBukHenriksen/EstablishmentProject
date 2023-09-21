using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Namotion.Reflection;
using System.Net;
using System.Reflection.Emit;

namespace WebApplication1.Models
{
    public class Establishment : EntityBase
    {
        public string Name { get; set; }
        public Location? LocationId { get; set; }
    }

    public class EstablishmentConfiguration : IEntityTypeConfiguration<Establishment>
    {
        public void Configure(EntityTypeBuilder<Establishment> builder)
        {
            builder.ToTable(nameof(Establishment));
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name);

        }
    }

}
