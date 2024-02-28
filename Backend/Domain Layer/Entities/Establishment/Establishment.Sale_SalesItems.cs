using WebApplication1.Data.DataModels;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface IEstablishment_Sale_SalesItems
    {
        void AddSalesItems(Sale sale, SalesItems salesItem);
        SalesItems CreateSalesItem(Sale sale, Item item, int quantity);
    }

    public partial class Establishment : IEstablishment_Sale_SalesItems
    {
        public void AddSalesItems(Sale sale, SalesItems salesItem)
        {
            this.ItemMustExist(salesItem.Item);
            this.SaleMustExist(sale);
            this.SalesItemMustBeCreatedForSale(salesItem, sale);
            this.SaleMustNotAlreadyIncludeItem(sale, salesItem.Item);
            sale.AddSalesItems(salesItem);
        }

        public SalesItems CreateSalesItem(Sale sale, Item item, int quantity)
        {
            this.ItemMustExist(item);
            this.SaleMustExist(sale);
            return sale.CreateSalesItems(item, quantity);
        }

        protected void SalesItemMustBeCreatedForSale(SalesItems salesItem, Sale sale)
        {
            if (!this.IsSalesItemCreatedForSale(salesItem, sale))
            {
                throw new InvalidOperationException("SalesItem is not created for sale");
            }
        }
        protected bool IsSalesItemCreatedForSale(SalesItems salesItem, Sale sale)
        {
            return salesItem.Sale.Id == sale.Id;

        }

        protected void SaleMustNotAlreadyIncludeItem(Sale sale, Item item)
        {
            if (this.doesSaleAlreadyContainItem(sale, item))
            {
                throw new InvalidOperationException("Sale already contains item");
            }
        }

        protected bool doesSaleAlreadyContainItem(Sale sale, Item item)
        {
            return sale.GetSalesItems().Any(x => x.Item == item);
        }
    }
}
