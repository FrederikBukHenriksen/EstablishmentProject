using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Domain_Layer.Entities
{
    public class Item : EntityBase
    {
        public Guid EstablishmentId { get; set; }
        public string Name { get; set; }
        public Price Price { get; set; }

        public Item() { }
        public Item(string name, Price price)
        {
            this.Name = name;
            this.Price = price;
        }

        public Item(string name, double price, Currency currency)
        {
            this.Name = name;
            this.Price = new Price(price, currency);
        }

        public Guid GetEstablishmentId()
        {
            return this.EstablishmentId;
        }
        public string GetName()
        {
            return this.Name;
        }

        public void SetName(string name)
        {
            this.Name = name;
        }

        public Price GetPrice()
        {
            return this.Price;
        }

        public void SetPrice(double price, Currency currency)
        {
            this.Price = new Price(price, currency);
        }
    }

    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.Property(e => e.Name).IsRequired();
            builder.HasIndex(e => e.Name).IsUnique();

            builder.OwnsOne(e => e.Price, priceBuilder =>
            {
                priceBuilder.Property(p => p.Amount).HasColumnName("PriceValue").IsRequired();
                priceBuilder.Property(p => p.Currency)
                    .HasConversion(
                    currency => currency.ToString(),
                    currencyName => (Currency)Enum.Parse(typeof(Currency), currencyName))
                    .HasColumnName("PriceCurrency")
            .IsRequired(true);
            });
        }
    }
}
