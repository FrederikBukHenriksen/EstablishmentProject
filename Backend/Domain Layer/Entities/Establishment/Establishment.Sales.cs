namespace WebApplication1.Domain_Layer.Entities
{
    public partial class Establishment : EntityBase
    {
        public void AddSale(Sale sale)
        {
            foreach (var salesItem in sale.SalesItems)
            {
                if (!this.IsItemRegisteredToEstablishment(salesItem.Item)) throw new Exception("Item is not registered to establishment");
            }
            this.Sales.Add(sale);
        }

        public void RemoveSale(Sale sale)
        {
            this.Sales.Remove(sale);
        }

        public List<Sale> GetSales()
        {
            return this.Sales.ToList();
        }

        //Valdiators
        private bool IsItemRegisteredToEstablishment(Item item)
        {
            return this.Items.Contains(item);
        }
    }
}
