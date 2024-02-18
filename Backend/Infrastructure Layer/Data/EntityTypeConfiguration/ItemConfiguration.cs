using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Infrastructure_Layer.Data.EntityTypeConfiguration
{
    [ExcludeFromCodeCoverage]
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



