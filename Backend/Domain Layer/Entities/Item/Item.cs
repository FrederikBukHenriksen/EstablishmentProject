﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
