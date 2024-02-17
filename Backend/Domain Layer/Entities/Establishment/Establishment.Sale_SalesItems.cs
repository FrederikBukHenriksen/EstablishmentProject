using WebApplication1.Data.DataModels;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface IEstablishment_Sale_SalesItems
    {
        void AddSalesItem(Sale sale, SalesItems salesItem);
        SalesItems CreateSalesItem(Sale sale, Item item, int quantity);
    }

    public partial class Establishment : IEstablishment_Sale_SalesItems
    {
        public void AddSalesItem(Sale sale, SalesItems salesItem)
        {
            this.ItemMustExist(salesItem.Item);
            this.SaleMustExist(sale);
            sale.AddSalesItems(salesItem);
        }

        public SalesItems CreateSalesItem(Sale sale, Item item, int quantity)
        {
            this.ItemMustExist(item);
            this.SaleMustExist(sale);
            return sale.CreateSalesItems(item, quantity);
        }
    }
}
