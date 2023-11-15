using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Models
{
    public class Sale : EntityBase
    {
        public Establishment Establishment { get; set; }

        public DateTime TimeStamp { get; set; }

        //public List<Item> Items { get; set; } = new List<Item>();

        public Table? Table { get; set; }
    }

    public class CheckConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable(nameof(Sale));

            builder.Property(e => e.TimeStamp).IsRequired();

            //builder.Property(e => e.Items).IsRequired();

            builder.HasOne(e => e.Table);
        }
    }
}
