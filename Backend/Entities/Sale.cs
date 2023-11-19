using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Namotion.Reflection;

namespace WebApplication1.Models
{
    public class Sale : EntityBase
    {
        public Establishment Establishment { get; set; }
        public DateTime? TimestampStart { get; set; }
        public DateTime TimestampEnd { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
        public Table? Table { get; set; }

        public Sale AddItem(Item item)
        {
            this.Items.Add(item);
            return this;
        }
    }

    public class CheckConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable(nameof(Sale));

            builder.Property(e => e.TimestampStart);


            builder.Property(e => e.TimestampEnd).IsRequired();

            //builder.Property(e => e.Items).IsRequired();

            builder.HasOne(e => e.Table);
        }
    }
}
