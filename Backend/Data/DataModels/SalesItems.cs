using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Namotion.Reflection;

namespace WebApplication1.Data.DataModels
{

    public class SalesItems : EntityBase
    {
        public Sale Sale { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }

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
}
