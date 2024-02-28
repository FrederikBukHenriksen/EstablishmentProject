using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Data.DataModels
{
    public interface ISalesItems
    {
    }

    public class SalesItems : JoiningTableBase, ISalesItems
    {
        public virtual Sale Sale { get; set; }
        public virtual Item Item { get; set; }
        public int quantity { get; set; }

        public SalesItems()
        {

        }

        public SalesItems(Sale sale, Item Item, int Quantity)
        {
            this.QuantityMustBeValid(Quantity);
            this.Sale = sale;
            this.Item = Item;
            this.quantity = Quantity;
        }

        protected void QuantityMustBeValid(int quantity)
        {
            if (!this.IsQuantityPositive(quantity))
            {
                throw new ArgumentException("Quantity is not positive");
            }
        }

        protected bool IsQuantityPositive(int quantity)
        {
            return quantity >= 0;

        }

    }
}
