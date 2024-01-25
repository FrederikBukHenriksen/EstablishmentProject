using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Entity_builders;

namespace EstablishmentProject.test.Domain.Builders
{
    public class EstablishmentBuilderTest : BaseIntegrationTest
    {
        private IFactoryServiceBuilder factoryServiceBuilder;

        public EstablishmentBuilderTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            factoryServiceBuilder = scope.ServiceProvider.GetRequiredService<IFactoryServiceBuilder>();
        }

        [Fact]
        public void WithName_Should_Set_Name()
        {
            // Arrange
            var establishmentBuilder = factoryServiceBuilder.EstablishmentBuilder();
            var name = "Test Establishment";

            // Act
            var establishment = establishmentBuilder.withName(name).Build();

            // Assert
            Assert.Equal(name, establishment.Name);
        }

        [Fact]
        public void WithItems_Should_Set_Items()
        {
            // Arrange
            var establishmentBuilder = factoryServiceBuilder.EstablishmentBuilder().withName("Establishment");
            var itemBuilder = factoryServiceBuilder.ItemBuilder();
            var items = new List<Item> { itemBuilder.withName("Coffee").withPrice(25).Build() };

            // Act
            var establishment = establishmentBuilder.withItems(items).Build();

            // Assert
            Assert.Equal(items, establishment.GetItems());
        }

        [Fact]
        public void WithTables_Should_Set_Tables()
        {
            // Arrange
            var establishmentBuilder = factoryServiceBuilder.EstablishmentBuilder().withName("Establishment");
            var tables = new List<Table> { new Table { }, new Table { } }; //TODO USE TABLE BUILDER

            // Act
            var establishment = establishmentBuilder.withTables(tables).Build();

            // Assert
            Assert.Equal(tables, establishment.Tables);
        }

        [Fact]
        public void WithSales__Success()
        {
            // Arrange
            var establishmentBuilder = factoryServiceBuilder.EstablishmentBuilder().withName("Establishment");
            var saleBuilder = factoryServiceBuilder.SaleBuilder();
            var sales = new List<Sale> { saleBuilder.WithTimestampPayment(DateTime.Now).Build() };

            // Act
            var establishment = establishmentBuilder.withSales(sales).Build();

            // Assert
            Assert.Equal(sales, establishment.Sales);
        }

        [Fact]
        public void WithSales__Success__With_Contained_Item()
        {
            // Arrange
            var coffee = factoryServiceBuilder.ItemBuilder().withName("Coffee").withPrice(25).Build();

            var establishmentBuilder = factoryServiceBuilder.EstablishmentBuilder().withName("Establishment").withItems(new List<Item> { coffee });
            var saleBuilder = factoryServiceBuilder.SaleBuilder();
            var sales = new List<Sale> { saleBuilder.WithSoldItems(new List<(Item item, int quantity)> { (coffee, 1) }).WithTimestampPayment(DateTime.Now).Build() };

            // Act
            var establishment = establishmentBuilder.withSales(sales).Build();

            // Assert
            Assert.Equal(sales, establishment.Sales);
        }

        [Fact]
        public void WithSales__Failure__Without_Contained_Item()
        {
            // Arrange
            var coffee = factoryServiceBuilder.ItemBuilder().withName("Coffee").withPrice(25).Build();

            var establishmentBuilder = factoryServiceBuilder.EstablishmentBuilder().withName("Establishment");
            var saleBuilder = factoryServiceBuilder.SaleBuilder();
            var sales = new List<Sale> { saleBuilder.WithSoldItems(new List<(Item item, int quantity)> { (coffee, 1) }).WithTimestampPayment(DateTime.Now).Build() };

            Establishment? establishment = null;

            // Act
            var exception = Record.Exception(() =>
            {
                establishment = establishmentBuilder.withSales(sales).Build();
            });

            // Assert
            Assert.Null(establishment);
            Assert.NotNull(exception);
            Assert.IsType(typeof(System.Exception), exception);
            Assert.Equal("Sold items in sales must exist in the establishment", exception.Message);
        }

        [Fact]
        public void Builders_Should_Not_Suffer_From_spillover()
        {
            // Arrange
            var establishmentBuilder1 = factoryServiceBuilder.EstablishmentBuilder();
            var establishment1 = establishmentBuilder1.withName("Establishment 1").Build();
            var establishmentBuilder2 = factoryServiceBuilder.EstablishmentBuilder();

            // Act
            var establishment2 = establishmentBuilder2.withName("Establishment 2").Build();

            // Assert
            Assert.NotEqual(establishment1, establishment2);
        }

    }
}
