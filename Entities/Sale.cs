using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Models
{
    public class Sale : EntityBase
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Establishment Establishment { get; set; }

        public DateTime Timestamp { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();

        public Table? Table { get; set; }

        public float GetTotalPrice()
        {
            float total = 0;
            foreach (Item item in Items)
            {
                total += item.Price;
            }
            return total;
        }

    }

    public class CheckConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable(nameof(Sale));

            builder.Property(e => e.Timestamp).IsRequired();

            builder.Property(e => e.Items).IsRequired();

            builder.HasOne(e => e.Table);
        }
    }
}
