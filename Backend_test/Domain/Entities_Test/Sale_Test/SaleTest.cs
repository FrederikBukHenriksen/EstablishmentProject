using EstablishmentProject.test.TestingCode;
using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test
{
    public class SaleTest : IntegrationTest
    {

        public SaleTest() : base()
        {

        }

        [Fact]
        public void Constructor_ShouldSetProperties()
        {
            // Arrange
            var timestampPayment = DateTime.Now;
            var timestampArrival = DateTime.Now.AddHours(-1);
            var salesItems = new List<(Item item, int quantity)>
            {
                (new Item("Coffee",20), 2),
                (new Item("Tea",10), 1)
            };
            Table table = new Table();

            // Act
            var sale = new Sale(timestampPayment, timestampArrival, salesItems, [table]);

            // Assert
            Assert.Equal(timestampArrival, sale.TimestampArrival);
            Assert.Equal(timestampPayment, sale.TimestampPayment);
            Assert.Equal(salesItems.Count, sale.SalesItems.Count);
            Assert.Equal(table, sale.GetSalesTables().First().Table);
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
                (new Item("tiem1",10), 2),
                (new Item("item2",5), 3),
            };

            var sale = new Sale(DateTime.Now, ItemAndQuantity: salesItems);

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
        public void GetNumberOfSoldItems_ShouldReturnNumberOfItems()
        {
            // Arrange
            var salesItems = new List<(Item item, int quantity)>
            {
                (new Item("tiem1",10), 2),
                (new Item("item2",5), 1),
            };
            var sale = new Sale(DateTime.Now, ItemAndQuantity: salesItems);

            // Act
            var numberOfSoldItems = sale.GetNumberOfSoldItems();

            // Assert
            Assert.Equal(salesItems.Count, 3);
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

        [Fact]
        public void SetTimeOfArrival_ValidArrivalTime_SetsArrivalTime()
        {
            // Arrange
            var sale = new Sale(DateTime.Now);
            var newTimeOfArrival = sale.TimestampPayment.AddHours(-1);

            // Act
            sale.setTimeOfArrival(newTimeOfArrival);

            // Assert
            Assert.Equal(newTimeOfArrival, sale.GetTimeOfArrival());
        }

        [Fact]
        public void SetTimeOfArrival_InvalidArrivalTime_ThrowsException()
        {
            // Arrange
            var sale = new Sale(DateTime.Now);
            var invalidTimeOfArrival = sale.TimestampPayment.AddHours(1);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => sale.setTimeOfArrival(invalidTimeOfArrival));
            Assert.Equal("Time of arrival must be before time of payment", exception.Message);
        }

        [Fact]
        public void SetTimeOfPayment_ValidPaymentTime_SetsPaymentTime()
        {
            // Arrange
            var dateTimeArrival = DateTime.Now.AddHours(-2);
            var sale = new Sale(DateTime.Now, dateTimeArrival);

            // New payment time that is valid (after arrival time)
            var newPaymentTime = dateTimeArrival.AddHours(1);

            // Act
            sale.SetTimeOfPayment(newPaymentTime);

            // Assert
            Assert.Equal(dateTimeArrival, sale.GetTimeOfArrival());
            Assert.Equal(newPaymentTime, sale.GetTimeOfPayment());
        }

        [Fact]
        public void SetTimeOfPayment_NoArrivalTime_ThrowsException()
        {
            // Arrange
            var sale = new Sale(DateTime.Now);

            // New payment time
            var newPaymentTime = DateTime.Now;

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => sale.SetTimeOfPayment(newPaymentTime));
            Assert.Equal("Time of payment must be before time of arrival", exception.Message);
        }

        [Fact]
        public void SetTimeOfPayment_InvalidPaymentTime_ThrowsException()
        {
            // Arrange
            var dateTimeArrival = DateTime.Now;
            var sale = new Sale(dateTimeArrival.AddHours(2), dateTimeArrival);

            // Invalid payment time that is before arrival time
            var invalidPaymentTime = dateTimeArrival.AddMinutes(-1);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => sale.SetTimeOfPayment(invalidPaymentTime));
            Assert.Equal("Time of payment must be before time of arrival", exception.Message);
        }

        [Fact]
        public void GetTimeOfSale_ReturnsTimestampPayment()
        {
            // Arrange
            var paymentTime = DateTime.Now;
            var sale = new Sale(paymentTime);

            // Act
            var timeOfSale = sale.GetTimeOfSale();

            // Assert
            Assert.Equal(paymentTime, timeOfSale);
        }
        [Fact]
        public void GetTimeOfPayment_ShouldReturnTimestampPayment()
        {
            // Arrange
            var paymentTime = DateTime.Now;
            var sale = new Sale(paymentTime);

            // Act
            var timeOfPayment = sale.GetTimeOfSale();

            // Assert
            Assert.Equal(paymentTime, timeOfPayment);
        }

        [Fact]
        public void GetTimeOfArrival_ReturnsTimestampArrival()
        {
            // Arrange
            var paymentTime = DateTime.Now;
            var arrivalTime = DateTime.Now.AddHours(1);

            var sale = new Sale(paymentTime, arrivalTime);

            // Act
            var timeOfArrival = sale.GetTimeOfArrival();

            // Assert
            Assert.Equal(arrivalTime, timeOfArrival);
        }
    }


}
