using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Data.DataModels;

namespace WebApplication1.Infrastructure_Layer.Data.EntityTypeConfiguration
{
    [ExcludeFromCodeCoverage]
    public class SalesItemsConfiguration : IEntityTypeConfiguration<SalesItems>
    {
        public void Configure(EntityTypeBuilder<SalesItems> builder)
        {


            builder.Property<Guid>("SalesId");
            builder.Property<Guid>("ItemId");

            builder.HasKey(new string[] { "SalesId", "ItemId" });

            builder.HasIndex("SalesId");
            builder.HasIndex("ItemId");

            builder.HasOne(e => e.Sale)
                .WithMany(e => e.SalesItems)
                .HasForeignKey("SalesId");

            builder.HasOne(e => e.Item)
                .WithMany()
                .HasForeignKey("ItemId");
        }
    }
}
