using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Namotion.Reflection;
using WebApplication1.Data.DataModels;

namespace WebApplication1.Domain.Entities
{
    public class Sale : EntityBase
    {
        public Establishment Establishment { get; set; }
        public DateTime? TimestampArrival { get; set; } = null;
        public DateTime TimestampPayment { get; set; }
        public List<SalesItems> SalesItems { get; set; } = new List<SalesItems>();
        public Table? Table { get; set; }

        public TimeSpan? GetTimespanOfVisit()
        {
            if (TimestampArrival == null || TimestampPayment == null)
            {
                return null;
            }
            return TimestampPayment - TimestampArrival;
        }

        public double GetTotalPrice()
        {
            double totalPrice = 0;
            foreach (SalesItems salesItem in SalesItems)
            {
                totalPrice += salesItem.Item.Price == null ? 0 : (double) salesItem.Item.Price;
            }
            return totalPrice;
        }   
    }

    public class CheckConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable(nameof(Sale));

            builder.Property(e => e.TimestampArrival);


            builder.Property(e => e.TimestampPayment).IsRequired();


            builder.HasOne(e => e.Table);
        }
    }
}
