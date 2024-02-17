namespace WebApplication1.Domain_Layer.Entities
{
    public interface IEstablishment_Sale_SalesTables
    {
        SalesTables AddSalesTables(Sale sale, SalesTables salesTables);
        SalesTables CreateSalesTables(Sale sale, Table table);
    }

    public partial class Establishment : IEstablishment_Sale_SalesTables
    {
        public SalesTables AddSalesTables(Sale sale, SalesTables salesTables)
        {
            this.TableMustExist(salesTables.Table);
            sale.SalesTables.Add(salesTables);
            return salesTables;
        }

        public SalesTables CreateSalesTables(Sale sale, Table table)
        {
            this.TableMustExist(table);
            return new SalesTables(table);
        }
    }
}
