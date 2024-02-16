using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data.DataModels;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface ISale : ISale_SalesItems
    {
        DateTime setTimeOfArrival(DateTime datetime);
        DateTime? GetTimeOfArrival();
        DateTime setTimeOfPayment(DateTime datetime);
        DateTime GetTimeOfPayment();
        DateTime GetTimeOfSale();
        TimeSpan? GetTimespanOfVisit();
        double GetTotalPrice();
        int GetNumberOfSoldItems();
    }

    public partial class Sale : EntityBase, ISale
    {
        public Guid EstablishmentId { get; set; }
        public DateTime? TimestampArrival { get; set; } = null;
        public DateTime TimestampPayment { get; set; }
        public virtual List<SalesItems> SalesItems { get; set; } = new List<SalesItems>();
        public virtual Table? Table { get; set; } = null;
        public Sale()
        {
        }

        public Sale(
            DateTime timestampPayment,
            DateTime? timestampArrival = null,
            List<(Item item, int quantity)>? ItemAndQuantity = null,
            Table? table = null)
        {
            this.TimestampPayment = timestampPayment;

            this.TimestampArrival = timestampArrival != null ? this.setTimeOfArrival((DateTime)timestampArrival!) : null;

            if (!(ItemAndQuantity.IsNullOrEmpty()))
            {
                foreach (var element in ItemAndQuantity)
                {
                    SalesItems salesItem = this.CreateSalesItem(element.item, element.quantity);
                    this.SalesItems.Add(salesItem);
                }
            }

            this.Table = table;
        }


        public DateTime setTimeOfArrival(DateTime datetime)
        {
            if (this.TimestampPayment > datetime)
            {
                this.TimestampArrival = datetime;
                return datetime;
            }
            else
            {
                throw new Exception("Time of arrival must be before time of payment");
            }
        }

        public DateTime? GetTimeOfArrival()
        {
            return this.TimestampArrival;
        }

        public DateTime setTimeOfPayment(DateTime datetime)
        {
            if (this.TimestampArrival != null && this.TimestampArrival < datetime)
            {
                this.TimestampArrival = datetime;
                return datetime;
            }
            else
            {
                throw new Exception("Time of payment must be before time of arrival");
            }
        }

        public DateTime GetTimeOfPayment() { return this.TimestampPayment; }

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
                        totalPrice += salesItem.Item.Price.Amount * salesItem.quantity;
                    }
                }
            }
            return totalPrice;
        }

        public int GetNumberOfSoldItems()
        {
            return this.SalesItems.Count();
        }
    }


}
