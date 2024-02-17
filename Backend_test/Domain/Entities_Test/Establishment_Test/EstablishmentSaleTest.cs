using EstablishmentProject.test.TestingCode;
using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test.Establishment_Test
{
    public class EstablishmentSaleTest : IntegrationTest
    {
        private Establishment establishment;

        public EstablishmentSaleTest() : base()
        {
            CommonArrange();
        }

        private void CommonArrange()
        {
            establishment = new Establishment("Test establishment");
        }

        [Fact]
        public void CreateSale_ShouldReturnSale()
        {
            // Arrange
            var timestampPayment = DateTime.Now;

            // Act
            Sale sale = establishment.CreateSale(timestampPayment);

            // Assert
            Assert.NotNull(sale);
            Assert.Equal(timestampPayment, sale.TimestampPayment);
        }

        [Fact]
        public void CreateSale_WithItemsRegistered()
        {
            // Arrange
            var timestampPayment = DateTime.Now;
            var item = establishment.AddItem(establishment.CreateItem("Item1", 10.00));
            var itemAndQuantity = new List<(Item, int)>() { (item, 1) };

            // Act
            var sale = establishment.CreateSale(timestampPayment, itemAndQuantity: itemAndQuantity);

            // Assert
            Assert.Contains(sale, establishment.GetSales());
        }

        [Fact]
        public void CreateSale_WithItemsNotRegistered_ThrowsException()
        {
            // Arrange
            var otherEstablishment = new Establishment();
            var item = otherEstablishment.CreateItem("Item1", 10.00);

            // Act
            Action act = () => establishment.CreateSale(timestampPayment: DateTime.Now, itemAndQuantity: new List<(Item, int)>() { (item, 1) });

            // Assert
            Assert.Throws<Exception>(act);
        }

        [Fact]
        public void AddSale_WithSaleCreatedForEstablishment_ShouldAddSale()
        {
            // Arrange
            var sale = establishment.CreateSale(DateTime.Now);

            // Act
            establishment.AddSale(sale);

            // Assert
            Assert.Contains(sale, establishment.GetSales());
        }

        [Fact]
        public void AddSale_WithSaleNotCreatedForEstablishment_ShouldNotAddSale()
        {
            // Arrange
            var otherEstablishment = new Establishment();
            var sale = otherEstablishment.CreateSale(DateTime.Now);

            // Act
            Action act = () => establishment.AddSale(sale);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }


        [Fact]
        public void GetSales()
        {
            // Arrange
            var timestampPayment = DateTime.Now;
            establishment.CreateSale(timestampPayment);

            // Act
            var sales = establishment.GetSales();

            // Assert
            Assert.Single(sales);
            Assert.Equal(timestampPayment, sales.First().TimestampPayment);
        }


        [Fact]
        public void RemoveSale_WithExistingSale_RemovesSale()
        {
            // Arrange
            var timestampPayment = DateTime.Now;
            establishment.CreateSale(timestampPayment);
            var sale = establishment.GetSales().First();

            // Act
            establishment.RemoveSale(sale);

            // Assert
            var sales = establishment.GetSales();
            Assert.Empty(sales);
        }

        [Fact]
        public void RemoveSale_WithNonExistingSale_DoesNotRemoveAnySale()
        {
            // Arrange
            var otherEstablishment = new Establishment();
            var otherSale = otherEstablishment.CreateSale(DateTime.Now);

            establishment.CreateSale(DateTime.Now);

            // Act
            establishment.RemoveSale(otherSale);

            // Assert
            Assert.Single(establishment.GetSales());
        }

        [Fact]
        public void UpdateSale_WithExistingSale_UpdatesSale()
        {
            // Arrange
            var initialTimestampPayment = DateTime.Now;
            establishment.CreateSale(initialTimestampPayment);
            var sale = establishment.GetSales().First();
            var updatedTimestampPayment = DateTime.Now.AddDays(1);
            sale.setTimeOfPayment(updatedTimestampPayment);

            // Act
            establishment.UpdateSale(sale);

            // Assert
            var sales = establishment.GetSales();
            Assert.Single(sales);
            Assert.Equal(updatedTimestampPayment, sales.First().TimestampPayment);
        }

        [Fact]
        public void UpdateSale_WithNonExistingSale_DoesNotAddSale()
        {
            // Arrange
            var nonExistingSale = new Sale(DateTime.Now);

            // Act
            establishment.UpdateSale(nonExistingSale);

            // Assert
            var sales = establishment.GetSales();
            Assert.Empty(sales);
        }
    }
}
