using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Domain_Layer.Entities
{
    public class Item : EntityBase
    {
        public string Name { get; set; }
        public Price Price { get; set; }
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
