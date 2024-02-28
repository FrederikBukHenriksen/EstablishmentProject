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
            this.SaleMustNotAlreadyIncludeTable(sale, salesTables.Table);
            sale.AddSalesTables(salesTables);
        }

        public SalesTables CreateSalesTables(Sale sale, Table table)
        {
            this.TableMustExist(table);
            this.SaleMustExist(sale);
            return sale.CreateSalesTables(table);
        }

        protected void SalesTableMustBeCreatedForSale(SalesTables salesTables, Sale sale)
        {
            if (!this.IsSalesTableCreatedForSale(salesTables, sale))
            {
                throw new InvalidOperationException("SalesTables is not created for sale");
            }
        }

        protected bool IsSalesTableCreatedForSale(SalesTables salesTables, Sale sale)
        {
            return sale.GetSalesTables().Contains(salesTables);
        }

        protected void SaleMustNotAlreadyIncludeTable(Sale sale, Table table)
        {
            if (this.doesSaleAlreadyContainTable(sale, table))
            {
                throw new InvalidOperationException("Sale already contains table");
            }
        }

        protected bool doesSaleAlreadyContainTable(Sale sale, Table table)
        {
            return sale.GetSalesTables().Any(x => x.Table == table);
        }
    }
}
