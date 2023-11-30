using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Domain.Entities
{
    public class Item : EntityBase
    {
        //public Establishment Establishment { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
    }

    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.Property(e => e.Name).IsRequired();

            builder.HasIndex(e => e.Name).IsUnique();

            builder.Property(e => e.Price).IsRequired();
        }
    }
}
