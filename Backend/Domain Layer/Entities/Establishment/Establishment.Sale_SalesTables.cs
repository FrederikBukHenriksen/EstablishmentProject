namespace WebApplication1.Domain_Layer.Entities
{
    public interface IEstablishment_Sale_SalesTables
    {
        void AddSalesTables(Sale sale, SalesTables salesTables);
        SalesTables CreateSalesTables(Sale sale, Table table);
    }

    public partial class Establishment : IEstablishment_Sale_SalesTables
    {
        public void AddSalesTables(Sale sale, SalesTables salesTables)
        {
            this.TableMustExist(salesTables.Table);
            this.SaleMustExist(sale);
            sale.SalesTables.Add(salesTables);
        }

        public SalesTables CreateSalesTables(Sale sale, Table table)
        {
            this.TableMustExist(table);
            this.SaleMustExist(sale);
            return sale.CreateSalesTables(table);
        }
    }
}
