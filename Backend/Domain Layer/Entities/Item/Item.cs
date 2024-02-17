using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Domain_Layer.Entities
{
    public class Item : EntityBase
    {
        public Guid EstablishmentId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public Item() { }

        public Item(string name, double price)
        {
            this.SetName(name);
            this.SetPrice(price);
        }

        public string GetName()
        {
            return this.Name;
        }

        public string SetName(string name)
        {
            this.ItemNameMustBeValid(name);
            this.Name = name;
            return this.GetName();
        }

        public double GetPrice()
        {
            return this.Price;
        }

        public void SetPrice(double price)
        {
            this.PriceMustBeValid(price);
            this.Price = price;
        }

        //Checkers and validators

        protected void ItemNameMustBeValid(string name)
        {
            if (!this.IsItemNameValid(name))
            {
                throw new ArgumentException("Item name is not valid");
            }
        }

        public bool IsItemNameValid(string name)
        {
            if (name == "")
            {
                return false;
            }
            return true;
        }

        protected void PriceMustBeValid(double price)
        {
            if (!this.IsPricePostive(price))
            {
                throw new ArgumentException("Price is not valid");
            }
        }
        public bool IsPricePostive(double price)
        {
            if (price >= 0)
            {
                return true;
            }
            return false;
        }
    }

    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.Property(e => e.Name).IsRequired();
            builder.HasIndex(e => e.Name).IsUnique();

            //builder.OwnsOne(e => e.Price, priceBuilder =>
            //{
            //    priceBuilder.Property(p => p.Amount).HasColumnName("PriceValue").IsRequired();
            //    priceBuilder.Property(p => p.Currency)
            //        .HasConversion(
            //        currency => currency.ToString(),
            //        currencyName => (Currency)Enum.Parse(typeof(Currency), currencyName))
            //        .HasColumnName("PriceCurrency")
            //.IsRequired(true);
            //});
        }
    }
}
