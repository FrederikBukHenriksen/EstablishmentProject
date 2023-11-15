using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Item : EntityBase
    {
        public Establishment Establishment { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable(nameof(Item));

            builder.Property<Guid>("EstablishmentId");

            builder.HasIndex("EstablishmentId");

            builder.HasOne(e => e.Establishment)
                .WithOne()
                .HasForeignKey("EstablishmentId");

            builder.Property(e => e.Name).IsRequired();

            builder.Property(e => e.Price).IsRequired();

        }
    }
}
