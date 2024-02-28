using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Utils;

namespace EstablishmentProject.test.Application_Test.FilterSales_Test
{
    public class FitlerSalesBySalesTables_Test
    {
        private Establishment establishment;


        private Sale sale_empty;
        private Sale sale_t1;
        private Sale sale_t1_t2;
        private Sale sale_t1_t2_t3;
        private Table table1;
        private Table table2;
        private Table table3;

        public FitlerSalesBySalesTables_Test()
        {
            establishment = new Establishment("Cafe 1");

            table1 = establishment.CreateTable("Table 1");
            establishment.AddTable(table1);
            table2 = establishment.CreateTable("Table 2");
            establishment.AddTable(table2);
            table3 = establishment.CreateTable("Table 3");
            establishment.AddTable(table3);


            sale_empty = establishment.CreateSale(DateTime.Today);
            establishment.AddSale(sale_empty);
            sale_t1 = establishment.CreateSale(DateTime.Today, tables: [table1]);
            establishment.AddSale(sale_t1);
            sale_t1_t2 = establishment.CreateSale(DateTime.Today, tables: [table1, table2]);
            establishment.AddSale(sale_t1_t2);
            sale_t1_t2_t3 = establishment.CreateSale(DateTime.Today, tables: [table1, table2, table3]);
            establishment.AddSale(sale_t1_t2_t3);
        }

        [Fact]
        public void Any()
        {
            //ARRANGE
            FilterSalesBySalesTables salesSorting = new FilterSalesBySalesTables(any: [table1.Id]);

            //ACT
            var sales = SalesFilterHelper.FilterSalesOnSalesTables(establishment.GetSales(), salesSorting);

            ////ASSERT
            Assert.Equal(3, sales.Count());

            Assert.Contains(sale_t1, sales);
            Assert.Contains(sale_t1_t2, sales);
            Assert.Contains(sale_t1_t2_t3, sales);
        }

        [Fact]
        public void All()
        {
            //ARRANGE
            FilterSalesBySalesTables salesSorting = new FilterSalesBySalesTables(all: [table1.Id, table2.Id]);

            //ACT
            var sortedSales = SalesFilterHelper.FilterSalesOnSalesTables(establishment.GetSales(), salesSorting);

            //ASSERT
            Assert.Equal(2, sortedSales.Count());

            Assert.Contains(sale_t1_t2, sortedSales);
            Assert.Contains(sale_t1_t2_t3, sortedSales);
        }

        [Fact]
        public void Excatly()
        {
            //ARRANGE
            FilterSalesBySalesTables salesSorting = new FilterSalesBySalesTables(exactly: [table1.Id, table2.Id]);

            //ACT
            var sales = SalesFilterHelper.FilterSalesOnSalesTables(establishment.GetSales(), salesSorting);

            //ASSERT
            Assert.Equal(1, sales.Count());
            Assert.Contains(sale_t1_t2, sales);
        }
    }
}
