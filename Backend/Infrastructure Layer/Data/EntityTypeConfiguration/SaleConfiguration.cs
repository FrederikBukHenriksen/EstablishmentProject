using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Infrastructure_Layer.Data.EntityTypeConfiguration
{
    [ExcludeFromCodeCoverage]
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable(nameof(Sale));

            builder.Property(e => e.TimestampArrival);

            builder.Property(e => e.TimestampPayment).IsRequired();
        }
    }
}

