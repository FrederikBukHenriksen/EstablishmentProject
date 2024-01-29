using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Data.DataModels
{

    public class SalesItems : EntityBase
    {
        public virtual Sale Sale { get; set; }
        public virtual Item Item { get; set; }
        public int quantity { get; set; }

        public SalesItems()
        {

        }

        public SalesItems(Item Item, int Quantity)
        {
            this.Item = Item;
            this.quantity = Quantity;
        }

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
