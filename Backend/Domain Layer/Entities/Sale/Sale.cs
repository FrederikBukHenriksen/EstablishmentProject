using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data.DataModels;

namespace WebApplication1.Domain_Layer.Entities
{
    public enum SaleType
    {
        EatIn,
        Delivery,
        TakeAway,
    }

    public enum PaymentType
    {
        Cash,
        Mobilepay,
        Card,
        Online,
    }

    public class Sale : EntityBase
    {
        public Guid EstablishmentId { get; set; }
        public SaleType? SaleType { get; set; }
        public PaymentType? PaymentType { get; set; }
        public DateTime? TimestampArrival { get; set; } = null;
        public DateTime TimestampPayment { get; set; }
        public virtual List<SalesItems>? SalesItems { get; set; } = new List<SalesItems>();
        public virtual Table? Table { get; set; } = null;
        public Sale()
        {
        }

        public Sale(
            DateTime timestampPayment,
            SaleType? saleType = null,
            PaymentType? paymentType = null,
            DateTime? timestampArrival = null,
            List<SalesItems>? salesItems = null,
            Table? table = null)
        {
            this.SaleType = saleType;
            this.PaymentType = paymentType;
            this.TimestampArrival = timestampArrival;
            this.TimestampPayment = timestampPayment;
            this.SalesItems = salesItems ?? new List<SalesItems>();
            this.Table = table;
        }

        public DateTime GetTimeOfSale()
        {
            return this.TimestampPayment;
        }

        public TimeSpan? GetTimespanOfVisit()
        {
            if (this.TimestampArrival == null || this.TimestampPayment == null)
            {
                return null;
            }
            return this.TimestampPayment - this.TimestampArrival;
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
                        totalPrice += salesItem.Item.Price.Value * salesItem.quantity;
                    }
                }
            }
            return totalPrice;
        }

        public int? GetNumberOfSoldItems()
        {
            if (this.SalesItems == null)
            {
                return null;
            }
            return this.SalesItems.Count();
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
