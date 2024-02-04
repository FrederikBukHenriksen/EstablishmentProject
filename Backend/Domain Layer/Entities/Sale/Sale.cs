using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data.DataModels;

namespace WebApplication1.Domain_Layer.Entities
{
    public class Sale : EntityBase
    {
        public Guid EstablishmentId { get; set; }
        public DateTime? TimestampArrival { get; set; } = null;
        public DateTime TimestampPayment { get; set; }
        public virtual List<SalesItems>? SalesItems { get; set; } = new List<SalesItems>();
        public virtual Table? Table { get; set; } = null;
        public Sale()
        {
        }

        public Sale(
            DateTime timestampPayment,
            DateTime? timestampArrival = null,
            List<(Item item, int quantity)>? salesItems = null,
            Table? table = null)
        {
            this.TimestampArrival = timestampArrival;
            this.TimestampPayment = timestampPayment;
            this.SalesItems = salesItems?.Select(x => new SalesItems(x.item, x.quantity)).ToList();
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

        public DateTime? getTimeOfArrival()
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

        public DateTime getTimeOfPayment() { return this.TimestampPayment; }

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

        public int? GetNumberOfSoldItems()
        {
            if (this.SalesItems == null)
            {
                return null;
            }
            return this.SalesItems.Count();
        }
    }


}
