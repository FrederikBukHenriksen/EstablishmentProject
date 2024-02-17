using WebApplication1.Data.DataModels;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface ISale_SalesItems
    {
        SalesItems AddSalesItems(SalesItems salesItem);
        SalesItems CreateSalesItems(Item item, int quantity);
        List<SalesItems> GetSalesItems();
        void RemoveSalesItems(SalesItems salesItem);
    }

    public partial class Sale : ISale_SalesItems
    {
        public SalesItems AddSalesItems(SalesItems salesItem)
        {
            this.SalesItemsMustBeCreatedForSale(salesItem);
            this.SalesItemsMustNotAlreadyExist(salesItem);
            this.SalesItems.Add(salesItem);
            return salesItem;
        }

        public SalesItems CreateSalesItems(Item item, int quantity)
        {
            return new SalesItems(item, quantity);
        }

        public List<SalesItems> GetSalesItems()
        {
            return this.SalesItems.ToList();
        }

        public void RemoveSalesItems(SalesItems salesItem)
        {
            this.SalesItemsMustExist(salesItem);
            this.SalesItems.Remove(salesItem);

        }

        //Checkers and validators

        private void SalesItemsMustExist(SalesItems salesItem)
        {
            if (!this.DoesSalesItemExistInSale(salesItem))
            {
                throw new InvalidOperationException("SalesItem does not exist within sale");
            }
        }

        private void SalesItemsMustNotAlreadyExist(SalesItems salesItem)
        {
            if (this.DoesSalesItemExistInSale(salesItem))
            {
                throw new InvalidOperationException("SalesItem already exists within sale");
            }
        }

        private bool DoesSalesItemExistInSale(SalesItems salesItem)
        {
            return this.GetSalesItems().Any(x => x.Item == salesItem.Item);
        }

        protected void SalesItemsMustBeCreatedForSale(SalesItems salesItems)
        {
            if (!this.IsSalesItemsCreatedForSale(salesItems))
            {
                throw new InvalidOperationException("SalesItems is not created for sale");
            }
        }

        private bool IsSalesItemsCreatedForSale(SalesItems salesItems)
        {
            return salesItems.Sale == this;
        }
    }
}
