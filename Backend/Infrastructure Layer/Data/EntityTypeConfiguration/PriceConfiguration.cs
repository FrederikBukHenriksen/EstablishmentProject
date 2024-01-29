using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Infrastructure_Layer.Data.EntityTypeConfiguration
{
    public class PriceConfiguration : IEntityTypeConfiguration<Price>
    {
        public void Configure(EntityTypeBuilder<Price> builder)
        {
            builder.ToTable(nameof(Price));

            builder.Property(x => x.Amount).IsRequired(true);

            builder.Property(x => x.Currency)
            .HasConversion(
                currency => currency.ToString(),
                currencyName => (Currency)Enum.Parse(typeof(Currency), currencyName));

        }
    }
}


