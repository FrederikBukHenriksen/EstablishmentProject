using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Namotion.Reflection;
using WebApplication1.Data.DataModels;

namespace WebApplication1.Models
{
    public class Sale : EntityBase
    {
        public Establishment Establishment { get; set; }
        public DateTime TimestampStart { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime TimestampEnd { get; set; } = DateTime.Now.ToUniversalTime();
        public List<SalesItems> SalesItems { get; set; } = new List<SalesItems>();
        public Table? Table { get; set; }
    }

    public class CheckConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable(nameof(Sale));

            builder.Property(e => e.TimestampStart);


            builder.Property(e => e.TimestampEnd);


            builder.HasOne(e => e.Table);
        }
    }
}
