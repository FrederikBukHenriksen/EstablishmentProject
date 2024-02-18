using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data.DataModels;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface ISale : ISale_SalesItems, ISale_SalesTables
    {
        //void setTimeOfArrival(DateTime datetime);
        DateTime? GetTimeOfArrival();
        void SetTimeOfPayment(DateTime datetime);
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
        public virtual List<SalesTables> SalesTables { get; set; } = new List<SalesTables>();

        public Sale()
        {
        }

        public Sale(
            DateTime timestampPayment,
            Establishment establishment,
            DateTime? timestampArrival = null,
            List<(Item item, int quantity)>? ItemAndQuantity = null,
            List<Table>? tables = null)
        {
            this.EstablishmentId = establishment.Id;

            this.SetTimeOfPayment(timestampPayment);
            if (timestampArrival != null)
            {
                this.setTimeOfArrival((DateTime)timestampArrival!);
            }

            if (!(ItemAndQuantity.IsNullOrEmpty()))
            {
                foreach (var element in ItemAndQuantity!)
                {
                    SalesItems salesItem = this.CreateSalesItems(element.item, element.quantity);
                    this.SalesItems.Add(salesItem);
                }
            }

            if (!(tables.IsNullOrEmpty()))
            {
                foreach (var table in tables!)
                {
                    SalesTables salesTables = this.CreateSalesTables(table);
                    this.SalesTables.Add(salesTables);
                }
            }
        }

        protected internal void setTimeOfArrival(DateTime datetime)
        {
            this.TimeOfArrivalMustBeBeforeTimeOfPayment(datetime);
            this.TimestampArrival = datetime;
        }

        public DateTime? GetTimeOfArrival()
        {
            return this.TimestampArrival;
        }

        public void SetTimeOfPayment(DateTime datetime)
        {
            this.TimeOfPaymentMustBeAfterTimeOfArrival(datetime);
            this.TimestampPayment = datetime;
        }

        public DateTime GetTimeOfPayment()
        {
            return this.TimestampPayment;
        }

        public TimeSpan? GetTimespanOfVisit()
        {
            this.MustHaveTimeOfArrival();
            return (this.GetTimeOfPayment() - this.GetTimeOfArrival());
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
                        totalPrice += salesItem.Item.Price * salesItem.quantity;
                    }
                }
            }
            return totalPrice;
        }

        public int GetNumberOfSoldItems()
        {
            return this.SalesItems.Aggregate(0, (acc, x) => acc + x.quantity);

        }

        //Checkers and validators

        protected void MustHaveTimeOfArrival()
        {
            if (this.TimestampArrival == null)
            {
                throw new InvalidOperationException("Time of arrival must be set");
            }
        }

        protected void TimeOfArrivalMustBeBeforeTimeOfPayment(DateTime datetime)
        {
            if (!this.IsTimeOfArrivalValid(datetime))
            {
                throw new ArgumentException("Time of arrival must be before time of payment");
            }
        }

        public bool IsTimeOfArrivalValid(DateTime datetime)
        {
            if (this.TimestampPayment >= datetime)
            {
                return true;
            }
            return false;
        }

        protected void TimeOfPaymentMustBeAfterTimeOfArrival(DateTime datetime)
        {
            if (!this.IsTimeOfPaymentValid(datetime))
            {
                throw new ArgumentException("Time of payment must be after time of arrival");
            }
        }

        public bool IsTimeOfPaymentValid(DateTime datetime)
        {
            if (this.TimestampArrival != null && this.TimestampArrival > datetime)
            {
                return false;
            }
            return true;
        }

        public DateTime GetTimeOfSale()
        {
            return this.GetTimeOfPayment();
        }
    }


}
