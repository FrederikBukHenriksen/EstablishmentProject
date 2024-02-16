using WebApplication1.Data.DataModels;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface ISale_SalesItems
    {
        SalesItems AddSalesItem(SalesItems salesItem);
        SalesItems CreateSalesItem(Item item, int quantity);
        List<SalesItems> GetSalesItems();
        void RemoveSalesItem(SalesItems salesItem);
        SalesItems UpdateSalesItem(SalesItems salesItem);
    }

    public partial class Sale : ISale_SalesItems
    {
        public SalesItems AddSalesItem(SalesItems salesItem)
        {
            this.SalesItems.Add(salesItem);
            return salesItem;
        }

        public SalesItems CreateSalesItem(Item item, int quantity)
        {
            SalesItems salesItem = new SalesItems(item, quantity);
            return salesItem;
        }

        public List<SalesItems> GetSalesItems()
        {
            return this.SalesItems.ToList();
        }

        public void RemoveSalesItem(SalesItems salesItem)
        {
            if (this.DoesSalesItemAlreadyExist(salesItem))
            {
                this.SalesItems.Remove(salesItem);
            }
        }

        public SalesItems UpdateSalesItem(SalesItems salesItem)
        {
            if (this.DoesSalesItemAlreadyExist(salesItem))
            {
                this.RemoveSalesItem(salesItem);
                this.AddSalesItem(salesItem);
            }
            return salesItem;
        }

        private bool DoesSalesItemAlreadyExist(SalesItems salesItem)
        {
            return this.SalesItems.Any(x => x.Item == salesItem.Item);
        }
    }
}
