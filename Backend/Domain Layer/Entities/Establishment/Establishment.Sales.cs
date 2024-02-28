using Microsoft.IdentityModel.Tokens;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface IEstablishment_Sale
    {
        void AddSale(Sale sale);
        Sale CreateSale(DateTime timestampPayment, List<Table>? table = null, List<(Item, int)>? itemAndQuantity = null, DateTime? timestampArrival = null);
        List<Sale> GetSales();
        void RemoveSale(Sale sale);
        void SetSaleTimeOfArrival(Sale sale, DateTime datetime);
        void SetSaleTimeOfPayment(Sale sale, DateTime datetime);
    }

    public partial class Establishment : EntityBase, IEstablishment_Sale
    {
        public Sale CreateSale(DateTime timestampPayment, List<Table>? tables = null, List<(Item, int)>? itemAndQuantity = null, DateTime? timestampArrival = null)
        {

            if (!itemAndQuantity.IsNullOrEmpty())
            {

                foreach (var item in itemAndQuantity!)
                {
                    this.ItemMustExist(item.Item1);
                }
            }
            if (!tables.IsNullOrEmpty())
            {
                foreach (var table in tables!)
                {
                    this.TableMustExist(table);
                }
            }

            return new Sale(timestampPayment, this, timestampArrival, tables: tables, ItemAndQuantity: itemAndQuantity);
        }

        public List<Sale> GetSales()
        {
            return this.Sales.ToList();
        }

        public void RemoveSale(Sale sale)
        {
            this.SaleMustExist(sale);
            this.Sales.Remove(sale);
        }


        public void AddSale(Sale sale)
        {
            this.SaleMustBeCreatedForEstablishment(sale);
            this.SaleMustNotAlreadyExist(sale);
            this.Sales.Add(sale);
        }

        public void SetSaleTimeOfArrival(Sale sale, DateTime datetime)
        {
            sale.setTimeOfArrival(datetime);
        }

        public void SetSaleTimeOfPayment(Sale sale, DateTime datetime)
        {
            sale.SetTimeOfPayment(datetime);
        }

        //Checkers and validators
        protected void SaleMustExist(Sale sale)
        {
            if (!this.DoesSaleExist(sale))
            {
                throw new InvalidOperationException("Sale does not exist within establishment");
            }
        }

        protected void SaleMustNotAlreadyExist(Sale sale)
        {
            if (this.DoesSaleExist(sale))
            {
                throw new InvalidOperationException("Sale already exists within establishment");
            }
        }

        protected bool DoesSaleExist(Sale sale)
        {
            return this.Sales.Any(x => x.Id == sale.Id);
        }

        protected void SaleMustBeCreatedForEstablishment(Sale sale)
        {
            if (!this.IsSaleCreatedForEstablishment(sale))
            {
                throw new InvalidOperationException("Sale is not created wihtin establishment");
            }
        }

        protected bool IsSaleCreatedForEstablishment(Sale sale)
        {
            return sale.EstablishmentId == this.Id;
        }
    }
}
