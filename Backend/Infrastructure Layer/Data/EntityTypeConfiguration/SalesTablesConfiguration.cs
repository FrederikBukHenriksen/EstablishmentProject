using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Infrastructure_Layer.Data.EntityTypeConfiguration
{
    [ExcludeFromCodeCoverage]
    public class SalesTablesConfiguration : IEntityTypeConfiguration<SalesTables>
    {
        public void Configure(EntityTypeBuilder<SalesTables> builder)
        {


            builder.Property<Guid>("SalesId");
            builder.Property<Guid>("TableId");

            builder.HasKey(new string[] { "SalesId", "TableId" });

            builder.HasIndex("SalesId");
            builder.HasIndex("TableId");

            builder.HasOne(e => e.Sale)
                .WithMany(e => e.SalesTables)
                .HasForeignKey("SalesId");

            builder.HasOne(e => e.Table)
                .WithMany()
                .HasForeignKey("TableId");
        }

    }
}
