using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Utils;

namespace EstablishmentProject.test.Application_Test.FilterSales_Test
{
    public class FilterSales_Test
    {
        private Establishment establishment;
        private Sale sale_NoArrival;
        private Sale sale_2;
        private Sale sale_1;
        private Sale sale_3;

        public FilterSales_Test()
        {
            establishment = new Establishment("Cafe 1");


            sale_1 = establishment.CreateSale(DateTime.Today.AddDays(-1));
            sale_1.setTimeOfArrival(DateTime.Today.AddDays(-2));
            establishment.AddSale(sale_1);

            sale_2 = establishment.CreateSale(DateTime.Today.AddDays(0));
            sale_2.setTimeOfArrival(DateTime.Today.AddDays(-1));
            establishment.AddSale(sale_2);

            sale_3 = establishment.CreateSale(DateTime.Today.AddDays(1));
            sale_3.setTimeOfArrival(DateTime.Today.AddDays(0));
            establishment.AddSale(sale_3);

            sale_NoArrival = establishment.CreateSale(DateTime.Today.AddDays(2));
            establishment.AddSale(sale_NoArrival);
        }

        [Fact]
        public void ArrivalTimeframe_ShouldReturnSalesWithArrivalWithinTimeframe()
        {
            //Arrange
            FilterSales filterSales = new FilterSales(arrivalTimeframe: [(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(0))]);

            //Act
            var sales = SalesFilterHelper.FilterSales(establishment.GetSales(), filterSales);

            //Arrange
            Assert.Equal(2, sales.Count);
            Assert.Contains(sale_2, sales);
            Assert.Contains(sale_3, sales);
        }

        [Fact]
        public void PaymentTimeframe_ShouldReturnSalesWithPaymentWithinTimeframe()
        {
            FilterSales filterSales = new FilterSales(paymentTimeframe: [(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(0))]);

            //Act
            var sales = SalesFilterHelper.FilterSales(establishment.GetSales(), filterSales);

            //Arrange
            Assert.Equal(2, sales.Count);
            Assert.Contains(sale_1, sales);
            Assert.Contains(sale_2, sales);

        }

        [Theory]
        [InlineData(SaleAttributes.TimestampArrival, 3)]
        [InlineData(SaleAttributes.Tables, 1)]
        [InlineData(SaleAttributes.Items, 1)]
        [InlineData(SaleAttributes.TimestampPayment, 4)]

        public void Attributes(SaleAttributes saleAttributes, int numberOfExptectedElements)
        {
            //ARRANGE
            Item item = establishment.CreateItem("Test item", 0.0);
            establishment.AddItem(item);
            Table table = establishment.CreateTable("Test Table");
            establishment.AddTable(table);

            establishment.AddSalesItems(sale_NoArrival, establishment.CreateSalesItem(sale_NoArrival, item, 1));
            establishment.AddSalesTables(sale_NoArrival, establishment.CreateSalesTables(sale_NoArrival, table));

            var salesSorting = new FilterSales(mustContainAllAttributes: [saleAttributes]);

            //ACT
            var sortedSales = SalesFilterHelper.FilterSales(establishment.GetSales(), salesSorting);

            //ASSERT
            Assert.Equal(numberOfExptectedElements, sortedSales.Count());
        }
    }

}
