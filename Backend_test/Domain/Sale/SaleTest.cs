using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain
{
    public class SaleTest : BaseIntegrationTest
    {
        public SaleTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }
        [Fact]
        public void Constructor_ShouldSetProperties()
        {
            // Arrange
            var timestampPayment = DateTime.Now;
            var saleType = SaleType.Delivery;
            var paymentType = PaymentType.Card;
            var timestampArrival = DateTime.Now.AddHours(-1);
            var salesItems = new List<(Item item, int quantity)>
            {
                (new Item { Price = new Price { Amount = 10 } }, 2),
                (new Item { Price = new Price { Amount = 5 } }, 1)
            };
            var table = new Table();

            // Act
            var sale = new Sale(timestampPayment, saleType, paymentType, timestampArrival, salesItems, table);

            // Assert
            Assert.Equal(saleType, sale.SaleType);
            Assert.Equal(paymentType, sale.PaymentType);
            Assert.Equal(timestampArrival, sale.TimestampArrival);
            Assert.Equal(timestampPayment, sale.TimestampPayment);
            Assert.Equal(salesItems.Count, sale.SalesItems.Count);
            Assert.Equal(table, sale.Table);
        }

        [Fact]
        public void GetTimeOfSale_ShouldReturnTimestampPayment()
        {
            // Arrange
            var sale = new Sale(DateTime.Now);

            // Act
            var timeOfSale = sale.GetTimeOfSale();

            // Assert
            Assert.Equal(sale.TimestampPayment, timeOfSale);
        }

        [Fact]
        public void GetTimespanOfVisit_ShouldReturnCorrectTimespan()
        {
            // Arrange
            var timestampArrival = DateTime.Now.AddHours(-2);
            var sale = new Sale(DateTime.Now, timestampArrival: timestampArrival);

            // Act
            var timespanOfVisit = sale.GetTimespanOfVisit();

            // Assert
            Assert.Equal(2, timespanOfVisit.Value.TotalHours, precision: 1);
        }

        [Fact]
        public void GetTimespanOfVisit_WithoutArrival_ShouldReturnNull()
        {
            // Arrange
            var sale = new Sale(DateTime.Now);

            // Act
            var timespanOfVisit = sale.GetTimespanOfVisit();

            // Assert
            Assert.Null(timespanOfVisit);
        }

        [Fact]
        public void GetTotalPrice_ShouldSumItemPricesCorrectly()
        {
            // Arrange
            var salesItems = new List<(Item item, int quantity)>
            {
                (new Item { Price = new Price { Amount = 10 } }, 2),
                (new Item { Price = new Price { Amount = 5 } }, 3)
            };
            var sale = new Sale(DateTime.Now, salesItems: salesItems);

            // Act
            var totalPrice = sale.GetTotalPrice();

            // Assert
            Assert.Equal(35, totalPrice);
        }

        [Fact]
        public void GetTotalPrice_NoItems_ShouldReturnZero()
        {
            // Arrange
            var sale = new Sale(DateTime.Now);

            // Act
            var totalPrice = sale.GetTotalPrice();

            // Assert
            Assert.Equal(0, totalPrice);
        }

        [Fact]
        public void GetNumberOfSoldItems_ShouldReturnCorrectCount()
        {
            // Arrange
            var salesItems = new List<(Item item, int quantity)>
            {
                (new Item { Price = new Price { Amount = 10 } }, 2),
                (new Item { Price = new Price { Amount = 5 } }, 1)
            };
            var sale = new Sale(DateTime.Now, salesItems: salesItems);

            // Act
            var numberOfSoldItems = sale.GetNumberOfSoldItems();

            // Assert
            Assert.Equal(salesItems.Count, numberOfSoldItems);
        }

        [Fact]
        public void GetNumberOfSoldItems_NoItems_ShouldReturnNull()
        {
            // Arrange
            var sale = new Sale(DateTime.Now);

            // Act
            var numberOfSoldItems = sale.GetNumberOfSoldItems();

            // Assert
            Assert.Null(numberOfSoldItems);
        }
    }
}
