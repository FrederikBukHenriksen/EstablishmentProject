using EstablishmentProject.test.TestingCode;
using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test
{
    public class SaleTest : IntegrationTest
    {
        private Establishment establishment;
        private Item coffee;
        private Item tea;

        public SaleTest() : base()
        {
            CommonArrange();
        }

        private void CommonArrange()
        {
            establishment = new Establishment("Test establishment");
            coffee = establishment.CreateItem("Coffee", 20);
            establishment.AddItem(coffee);
            tea = establishment.CreateItem("Tea", 10);
            establishment.AddItem(tea);
        }

        [Fact]
        public void Constructor_ShouldSetProperties()
        {
            // Arrange
            var timestampPayment = DateTime.Now;
            var timestampArrival = DateTime.Now.AddHours(-1);
            var salesItems = new List<(Item item, int quantity)>
            {
                (coffee, 2),
                (tea, 1)
            };
            Table table = establishment.CreateTable("Table1");
            establishment.AddTable(table);

            // Act
            var sale = establishment.CreateSale(timestampPayment: timestampPayment, timestampArrival: timestampArrival, itemAndQuantity: salesItems, tables: [table]);

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
            var sale = establishment.CreateSale(DateTime.Now);

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
            var sale = establishment.CreateSale(DateTime.Now, timestampArrival: timestampArrival);

            // Act
            var timespanOfVisit = sale.GetTimespanOfVisit();

            // Assert
            Assert.Equal(2, timespanOfVisit.Value.TotalHours, precision: 1);
        }

        [Fact]
        public void GetTimespanOfVisit_WithoutArrival_ShouldThrowException()
        {
            // Arrange
            var sale = establishment.CreateSale(DateTime.Now);

            // Act
            Action act = () => sale.GetTimespanOfVisit();

            // Assert
            var exception = Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void GetTotalPrice_ShouldSumItemPricesCorrectly()
        {
            // Arrange
            var salesItems = new List<(Item item, int quantity)>
            {
                (coffee, 2),
                (tea, 3),
            };

            var sale = establishment.CreateSale(timestampPayment: DateTime.Now, itemAndQuantity: salesItems);

            // Act
            var totalPrice = sale.GetTotalPrice();

            // Assert
            Assert.Equal(coffee.Price * 2 + tea.Price * 3, totalPrice);
        }

        [Fact]
        public void GetTotalPrice_NoItems_ShouldReturnZero()
        {
            // Arrange
            var sale = establishment.CreateSale(DateTime.Now);

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
                (coffee, 2),
                (tea, 3),
            };
            var sale = establishment.CreateSale(timestampPayment: DateTime.Now, itemAndQuantity: salesItems);

            // Act
            var numberOfSoldItems = sale.GetNumberOfSoldItems();

            // Assert
            Assert.Equal(numberOfSoldItems, 5);
        }

        [Fact]
        public void GetNumberOfSoldItems_WithNoSalesItemsForSale_ShouldReturnZero()
        {
            // Arrange
            var sale = establishment.CreateSale(DateTime.Now);

            // Act
            var numberOfSoldItems = sale.GetNumberOfSoldItems();

            // Assert
            Assert.Equal(0, numberOfSoldItems);
        }

        [Fact]
        public void SetTimeOfArrival_WithEarlierTimeOfPayment_ShouldSetTimeOfArrival()
        {
            // Arrange
            var sale = establishment.CreateSale(DateTime.Now);
            var newTimeOfArrival = sale.TimestampPayment.AddHours(-1);

            // Act
            sale.setTimeOfArrival(newTimeOfArrival);

            // Assert
            Assert.Equal(newTimeOfArrival, sale.GetTimeOfArrival());
        }




        [Fact]
        public void SetTimeOfArrival_WithLaterTimeOfPayment_ShouldSetTimeOfArrival()
        {
            // Arrange
            var sale = establishment.CreateSale(DateTime.Now);
            var newTimeOfArrival = sale.TimestampPayment.AddHours(1);

            // Act
            Action act = () => sale.setTimeOfArrival(newTimeOfArrival);

            // Assert
            Assert.Throws<ArgumentException>(act);
            Assert.Null(sale.GetTimeOfArrival());
        }

        [Fact]
        public void SetTimeOfPayment_WithEarlierTimeOfArrival_ShouldSetPaymentTime()
        {
            // Arrange
            var time = DateTime.Now;
            var sale = establishment.CreateSale(timestampPayment: time, timestampArrival: time.AddHours(-1));

            // New payment time that is valid (after arrival time)
            var newPaymentTime = time.AddHours(1);

            // Act
            sale.SetTimeOfPayment(newPaymentTime);

            // Assert
            Assert.Equal(newPaymentTime, sale.GetTimeOfPayment());
        }

        [Fact]
        public void SetTimeOfPayment_WithNoTimeOfArraival_ShouldSetPaymentTime()
        {
            // Arrange
            var time = DateTime.Now;
            var sale = establishment.CreateSale(time);

            // New payment time that is valid (after arrival time)
            var newPaymentTime = time.AddHours(1);

            // Act
            sale.SetTimeOfPayment(newPaymentTime);

            // Assert
            Assert.Equal(newPaymentTime, sale.GetTimeOfPayment());
        }

        [Fact]
        public void GetTimeOfSale_ShouldReturnTimesOfPayment()
        {
            // Arrange
            var paymentTime = DateTime.Now;
            var sale = establishment.CreateSale(paymentTime);

            // Act
            var timeOfSale = sale.GetTimeOfSale();

            // Assert
            Assert.Equal(paymentTime, timeOfSale);
        }

    }


}
