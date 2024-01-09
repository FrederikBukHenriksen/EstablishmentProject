using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Domain_Layer.Entities
{
    public class Item : EntityBase
    {
        public string Name { get; set; }
        public Price Price { get; set; }
        public virtual Establishment Establishment { get; set; }
    }

    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.Property(e => e.Name).IsRequired();

            builder.HasIndex(e => e.Name).IsUnique();

            builder.OwnsOne(e => e.Price, priceBuilder =>
            {
                priceBuilder.Property(p => p.Value).HasColumnName("PriceValue").IsRequired();
                priceBuilder.Property(p => p.Currency).HasColumnName("PriceCurrency").IsRequired();
            });

            //builder.HasOne<Establishment>()
            //.WithMany(x => x.Items)
            //.HasForeignKey("EstablishmentId")
            //.IsRequired();
        }
    }
}
