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
            this.SetName(name);
            this.Price = price;
        }

        public Item(string name, double price)
        {
            this.SetName(name);
            this.SetPrice(price, Currency.DKK);
        }

        public Item(string name, double price, Currency currency)
        {
            this.SetName(name);
            this.SetPrice(price, currency);
        }

        public string GetName()
        {
            return this.Name;
        }

        public string SetName(string name)
        {
            if (!this.IsItemNameValid(name))
            {
                throw new ArgumentException("Item name is not valid");
            }
            this.Name = name;
            return this.GetName();
        }

        public Price GetPrice()
        {
            return this.Price;
        }

        public void SetPrice(double price, Currency currency)
        {
            if (!this.IsPriceValid(price))
            {
                throw new ArgumentException("Price is not valid");
            }

            this.Price = new Price(price, currency);
        }

        //Checkers and validators
        public bool IsItemNameValid(string name)
        {
            if (name == "")
            {
                return false;
            }
            return true;
        }

        public bool IsPriceValid(double price)
        {
            if (price <= 0)
            {
                return false;
            }
            return true;
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
