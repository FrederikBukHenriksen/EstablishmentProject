using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Entity_builders;
using WebApplication1.Utils;

namespace EstablishmentProject.test
{
    public class SalesSortingTest : BaseIntegrationTest
    {
        private Establishment establishment;
        private IFactoryServiceBuilder factoryServiceBuilder;
        private List<Sale> sales;

        private Item coffee;
        private Item tea;
        private Item water;

        private Sale sale_empty;
        private Sale sale_coffee;
        private Sale sale_coffee_tea;
        private Sale sale_coffee_tea_water;


        public SalesSortingTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            factoryServiceBuilder = scope.ServiceProvider.GetRequiredService<IFactoryServiceBuilder>();
            coffee = factoryServiceBuilder.ItemBuilder().withName("coffee").withPrice(0, Currency.DKK).Build();
            tea = factoryServiceBuilder.ItemBuilder().withName("tea").withPrice(0, Currency.DKK).Build();
            water = factoryServiceBuilder.ItemBuilder().withName("water").withPrice(0, Currency.DKK).Build();

            sale_empty = factoryServiceBuilder.SaleBuilder().WithTimestampPayment(DateTime.Today).Build();
            sale_coffee = factoryServiceBuilder.SaleBuilder().WithSoldItems([(coffee, 1)]).WithTimestampPayment(DateTime.Today.AddDays(-1)).Build();
            sale_coffee_tea = factoryServiceBuilder.SaleBuilder().WithSoldItems([(coffee, 1), (tea, 1)]).WithTimestampPayment(DateTime.Today.AddDays(-2)).Build();
            sale_coffee_tea_water = factoryServiceBuilder.SaleBuilder().WithSoldItems([(coffee, 1), (tea, 1), (water, 1)]).WithTimestampPayment(DateTime.Today.AddDays(-3)).Build();


            //ARRANGE
            sales = [
                sale_empty,
                sale_coffee,
                sale_coffee_tea,
                sale_coffee_tea_water
            ];
        }

        [Fact]
        public void Any()
        {
            //ARRANGE
            SalesSorting salesSorting = new SalesSorting { Any = (new List<Guid> { coffee.Id }) };

            //ACT
            var sortedSales = SalesSortingParametersExecute.SortSales(sales, salesSorting);

            //ASSERT
            Assert.Equal(3, sortedSales.Count());

            List<List<Item>> sortedSalesItems = sortedSales.Select(sale => sale.SalesItems.Select(salesItem => salesItem.Item).ToList()).ToList();
            Assert.All(sortedSalesItems, salesItems => Assert.Contains(coffee, salesItems));
        }

        [Fact]
        public void All()
        {
            //ARRANGE
            var salesSorting = new SalesSorting { All = (new List<Guid> { coffee.Id, water.Id }) };

            //ACT
            var sortedSales = SalesSortingParametersExecute.SortSales(sales, salesSorting);

            //ASSERT
            Assert.Equal(3, sortedSales.Count());

            List<List<Item>> sortedSalesItems = sortedSales.Select(sale => sale.SalesItems.Select(salesItem => salesItem.Item).ToList()).ToList();
            Assert.All(sortedSalesItems, salesItems =>
            {
                Assert.True(salesItems.Any(item => item == coffee || item == water));
            });
        }

        [Fact]
        public void Excatly()
        {
            //ARRANGE
            var salesSorting = new SalesSorting { Excatly = (new List<Guid> { coffee.Id, tea.Id }) };

            //ACT
            var sortedSales = SalesSortingParametersExecute.SortSales(sales, salesSorting);

            //ASSERT
            Assert.Equal(1, sortedSales.Count());

            List<List<Item>> sortedSalesItems = sortedSales.Select(sale => sale.SalesItems.Select(salesItem => salesItem.Item).ToList()).ToList();
            Assert.All(sortedSalesItems, salesItems => Assert.Contains(coffee, salesItems));
            Assert.All(sortedSalesItems, salesItems => Assert.Contains(tea, salesItems));
        }

        [Fact]
        public void TimePeriods()
        {
            //ARRANGE
            DateTime start = DateTime.Today.AddDays(-2);
            DateTime end = DateTime.Today.AddDays(-1);
            var salesSorting = new SalesSorting { WithinTimeperiods = new List<(DateTime start, DateTime end)> { (start, end) } };

            //ACT
            var sortedSales = SalesSortingParametersExecute.SortSales(sales, salesSorting);

            //ASSERT
            Assert.Equal(2, sortedSales.Count());

            Assert.All(sortedSales, sale => Assert.True(sale.TimestampPayment >= start && sale.TimestampPayment <= end));

        }

        [Theory]
        [InlineData(SaleAttributes.TimestampArrival, 1)]
        [InlineData(SaleAttributes.TimestampPayment, 4)]
        [InlineData(SaleAttributes.Table, 1)]
        [InlineData(SaleAttributes.Items, 4)]



        public void Attributes(SaleAttributes saleAttributes, int numberOfExptectedElements)
        {
            sale_empty.TimestampArrival = DateTime.Today.AddDays(-1);
            sale_coffee.Table = new Table();

            //ARRANGE
            var salesSorting = new SalesSorting { MustContainAllAttributes = new List<SaleAttributes> { saleAttributes } };

            //ACT
            var sortedSales = SalesSortingParametersExecute.SortSales(sales, salesSorting);

            //ASSERT
            Assert.Equal(numberOfExptectedElements, sortedSales.Count());
        }
    }
}
