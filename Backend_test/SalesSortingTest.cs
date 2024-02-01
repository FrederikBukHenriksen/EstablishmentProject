using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Entity_builders;
using WebApplication1.Utils;

namespace EstablishmentProject.test
{
    public class SalesSortingTest : BaseIntegrationTest
    {
        private IUnitOfWork unitOfWork;
        private Establishment establishment;
        private IFactoryServiceBuilder factoryServiceBuilder;
        private List<Sale> sales;

        private Item coffee;
        private Item tea;
        private Item water;
        private Table table;
        private Sale sale_empty;
        private Sale sale_coffee;
        private Sale sale_coffee_tea;
        private Sale sale_coffee_tea_water;

        public SalesSortingTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            clearDatabase();
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            establishment = new Establishment("Cafe 1");

            coffee = establishment.AddItem(establishment.CreateItem("coffee", 0, Currency.DKK));
            tea = establishment.AddItem(establishment.CreateItem("tea", 0, Currency.DKK));
            water = establishment.AddItem(establishment.CreateItem("water", 0, Currency.DKK));

            table = establishment.AddTable(establishment.CreateTable("table 1"));

            sale_empty = establishment.AddSale(establishment.CreateSale(DateTime.Today));
            sale_coffee = establishment.AddSale(establishment.CreateSale(DateTime.Today.AddDays(-1), itemAndQuantity: new List<(Item, int)> { (coffee, 1) }, table: table));
            sale_coffee_tea = establishment.AddSale(establishment.CreateSale(DateTime.Today.AddDays(-2), itemAndQuantity: new List<(Item, int)> { (coffee, 1), (tea, 1) }, timestampArrival: DateTime.Today.AddDays(-2).AddHours(-1)));
            sale_coffee_tea_water = establishment.AddSale(establishment.CreateSale(DateTime.Today.AddDays(-3), itemAndQuantity: new List<(Item, int)> { (coffee, 1), (tea, 1), (water, 1) }, timestampArrival: DateTime.Today.AddDays(-3).AddHours(-1)));

            //ARRANGE
            sales = [
                sale_empty,
                sale_coffee,
                sale_coffee_tea,
                sale_coffee_tea_water
            ];

            using (unitOfWork)
            {
                unitOfWork.establishmentRepository.Add(establishment);
            }
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

            Assert.Contains(sale_coffee, sortedSales);
            Assert.Contains(sale_coffee_tea, sortedSales);
            Assert.Contains(sale_coffee_tea_water, sortedSales);

            List<List<Item>> sortedSalesItems = sortedSales.Select(sale => sale.SalesItems.Select(salesItem => salesItem.Item).ToList()).ToList();
            Assert.All(sortedSalesItems, salesItems => Assert.Contains(coffee, salesItems));
        }

        [Fact]
        public void All()
        {
            //ARRANGE
            var salesSorting = new SalesSorting { All = (new List<Guid> { coffee.Id, tea.Id }) };

            //ACT
            var sortedSales = SalesSortingParametersExecute.SortSales(sales, salesSorting);

            //ASSERT
            Assert.Equal(2, sortedSales.Count());

            Assert.Contains(sale_coffee_tea, sortedSales);
            Assert.Contains(sale_coffee_tea_water, sortedSales);
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

            Assert.Contains(sale_coffee_tea, sortedSales);

            List<List<Item>> sortedSalesItems = sortedSales.Select(sale => sale.SalesItems.Select(salesItem => salesItem.Item).ToList()).ToList();
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

            Assert.Contains(sale_coffee, sortedSales);
            Assert.Contains(sale_coffee_tea, sortedSales);
        }

        [Theory]
        [InlineData(SaleAttributes.TimestampArrival, 2)]
        [InlineData(SaleAttributes.Table, 1)]
        [InlineData(SaleAttributes.Items, 3)]
        [InlineData(SaleAttributes.TimestampPayment, 4)]

        public void Attributes(SaleAttributes saleAttributes, int numberOfExptectedElements)
        {
            //ARRANGE
            var salesSorting = new SalesSorting { MustContainAllAttributes = new List<SaleAttributes> { saleAttributes } };

            //ACT
            var sortedSales = SalesSortingParametersExecute.SortSales(sales, salesSorting);

            //ASSERT
            Assert.Equal(numberOfExptectedElements, sortedSales.Count());
        }
    }
}
