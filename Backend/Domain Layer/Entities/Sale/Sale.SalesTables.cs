namespace WebApplication1.Domain_Layer.Entities
{
    public interface ISale_SalesTables
    {
        void AddSalesTables(SalesTables salesTables);
        SalesTables CreateSalesTables(Table table);
        List<SalesTables> GetSalesTables();
        void RemoveSalesTables(SalesTables salesTables);
    }

    public partial class Sale : ISale_SalesTables
    {
        public void AddSalesTables(SalesTables salesTables)
        {
            this.SalesTablesMustBeCreatedForSale(salesTables);
            this.SalesTablesMustNotAlreadyExist(salesTables);
            this.SalesTables.Add(salesTables);
        }

        public SalesTables CreateSalesTables(Table table)
        {
            return new SalesTables(this, table);
        }

        public List<SalesTables> GetSalesTables()
        {
            return this.SalesTables.ToList();
        }


        public void RemoveSalesTables(SalesTables salesTables)
        {
            this.SalesTablesMustExist(salesTables);
            this.SalesTables.Remove(salesTables);

        }

        //Checkers and validators

        private void SalesTablesMustExist(SalesTables salesTables)
        {
            if (!this.DoesSalesTablesExistInSale(salesTables))
            {
                throw new InvalidOperationException("SalesTables does not exist within sale");
            }
        }

        private void SalesTablesMustNotAlreadyExist(SalesTables salesTables)
        {
            if (this.DoesSalesTablesExistInSale(salesTables))
            {
                throw new InvalidOperationException("SalesTables already exists within sale");
            }
        }

        private bool DoesSalesTablesExistInSale(SalesTables salesTables)
        {
            return this.SalesTables.Any(x => x.Table == salesTables.Table);
        }

        protected void SalesTablesMustBeCreatedForSale(SalesTables salesTables)
        {
            if (!this.IsSalesTablesCreatedForSale(salesTables))
            {
                throw new InvalidOperationException("SalesTables is not created for sale");
            }
        }

        private bool IsSalesTablesCreatedForSale(SalesTables salesTables)
        {
            return salesTables.Sale == this;
        }
    }
}
