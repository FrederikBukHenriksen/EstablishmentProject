using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain
{
    public class EstablishmentSaleTest : BaseIntegrationTest
    {
        public EstablishmentSaleTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public void CreateSale()
        {
            // Arrange
            var establishment = new Establishment();
            var timestampPayment = DateTime.Now;

            // Act
            establishment.CreateSale(timestampPayment);

            // Assert
            var sales = establishment.GetSales();
            Assert.Single(sales);
            Assert.Equal(timestampPayment, sales.First().TimestampPayment);
        }


        [Fact]
        public void CreateSale_WithItemsRegistered()
        {
            // Arrange
            var establishment = new Establishment();
            var timestampPayment = DateTime.Now;
            var item = establishment.CreateItem("Item1", 10.00, Currency.DKK);
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
            var item = otherEstablishment.CreateItem("Item1", 10.00, Currency.DKK);

            var establishment = new Establishment();

            // Act
            Action act = () => establishment.CreateSale(timestampPayment: DateTime.Now, itemAndQuantity: new List<(Item, int)>() { (item, 1) });

            // Assert
            Assert.Throws<Exception>(act);
        }


        [Fact]
        public void GetSales()
        {
            // Arrange
            var establishment = new Establishment();
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
            var establishment = new Establishment();
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

            var establishment = new Establishment();
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
            var establishment = new Establishment();
            var initialTimestampPayment = DateTime.Now;
            establishment.CreateSale(initialTimestampPayment);
            var sale = establishment.GetSales().First();
            var updatedTimestampPayment = DateTime.Now.AddDays(1);
            sale.TimestampPayment = updatedTimestampPayment;

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
            var establishment = new Establishment();
            var nonExistingSale = new Sale(DateTime.Now);

            // Act
            establishment.UpdateSale(nonExistingSale);

            // Assert
            var sales = establishment.GetSales();
            Assert.Empty(sales);
        }
    }
}
