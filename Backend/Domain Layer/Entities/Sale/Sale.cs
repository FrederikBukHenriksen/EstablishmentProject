using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.IdentityModel.Tokens;
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
        public Table? Table { get; set; } = null;

        public DateTime GetTimeOfSale()
        {
            return TimestampPayment;
        }

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
            if (!(this.SalesItems.IsNullOrEmpty()))
            {
                foreach (SalesItems salesItem in this.SalesItems)
                {
                    if (!(salesItem == null))
                    {
                        totalPrice += salesItem.Item.Price * salesItem.Quantity;
                    }
                }
            }
            return totalPrice;
        }

        public int? GetNumberOfSoldItems()
        {
            if (SalesItems == null)
            {
                return null;
            }
            return SalesItems.Count();
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
