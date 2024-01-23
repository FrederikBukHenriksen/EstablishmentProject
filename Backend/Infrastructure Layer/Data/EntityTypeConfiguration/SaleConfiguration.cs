using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Infrastructure_Layer.Data.EntityTypeConfiguration
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            //builder.HasIndex(e => e.Id).IsUnique();

            builder.ToTable(nameof(Sale));

            builder.Property(e => e.TimestampArrival);

            builder.Property(e => e.TimestampPayment).IsRequired();

            builder.HasOne(e => e.Table);



        }
    }
}

