using WebApplication1.Data.DataModels;
using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test
{
    public class Sale_SalesItems_Test
    {
        private Establishment establishment;
        private Item item;
        private Sale sale;

        public Sale_SalesItems_Test()
        {
            CommonArrange();
        }
        private void CommonArrange()
        {
            establishment = new Establishment("Test Establishment");
            item = establishment.CreateItem("Item", 0);
            establishment.AddItem(item);
            sale = establishment.CreateSale(DateTime.Now);
            establishment.AddSale(sale);
        }

        [Fact]
        public void CreateSalesItem_ShouldCreateSalesItem()
        {
            // Arrange
            int quantity = 1;

            // Act
            SalesItems salesItem = establishment.CreateSalesItem(sale, item, quantity);

            // Assert
            Assert.NotNull(salesItem);
            Assert.Equal(item, salesItem.Item);
            Assert.Equal(1, salesItem.quantity);
        }

        [Fact]
        public void CreateSalesItem_WithItemFromDifferentEstablishment_ShouldNotCreateSalesItem()
        {
            // Arrange
            var otherEstalishment = new Establishment("Other establishment");
            var otherItem = otherEstalishment.CreateItem("Other item", 0);
            otherEstalishment.AddItem(otherItem);
            var otherSale = otherEstalishment.CreateSale(DateTime.Now);
            otherEstalishment.AddSale(otherSale);

            // Act
            Action act = () => establishment.CreateSalesItem(otherSale, otherItem, 1);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void CreateSalesItem_WithSaleFromDifferentEstablishment_ShouldNotCreateSalesItem()
        {
            // Arrange
            var otherEstalishment = new Establishment("Other establishment");
            var otherItem = otherEstalishment.CreateItem("Other item", 0);
            otherEstalishment.AddItem(otherItem);
            var otherSale = otherEstalishment.CreateSale(DateTime.Now);
            otherEstalishment.AddSale(otherSale);

            // Act
            Action act = () => establishment.CreateSalesItem(otherSale, otherItem, 1);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void AddSalesItem_ShouldAddSalesItem()
        {
            // Arrange
            SalesItems salesItem = establishment.CreateSalesItem(sale, item, 1);

            // Act
            establishment.AddSalesItem(sale, salesItem);

            // Assert
            Assert.Contains(salesItem, sale.GetSalesItems());
        }

        [Fact]
        public void AddSalesItem_WithSalesItemsCreatedForDifferentSale_ShouldNotAddSalesItem()
        {
            // Arrange
            var otherSale = establishment.CreateSale(DateTime.Now);
            establishment.AddSale(otherSale);
            SalesItems salesItem = establishment.CreateSalesItem(otherSale, item, 1);

            // Act
            Action act = () => establishment.AddSalesItem(sale, salesItem);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.True(sale.GetSalesItems().Count == 0);
        }

        [Fact]
        public void AddSalesItem_WithSalesItemsAlreadyAdded_ShouldNotAddSalesItem()
        {
            // Arrange
            SalesItems salesItem = establishment.CreateSalesItem(sale, item, 1);
            establishment.AddSalesItem(sale, salesItem);

            // Act
            Action act = () => establishment.AddSalesItem(sale, salesItem);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.True(sale.GetSalesItems().Count == 1);
        }

        [Fact]
        public void RemoveSalesItem_ShouldRemoveSalesItem()
        {
            // Arrange
            SalesItems salesItem = establishment.CreateSalesItem(sale, item, 1);
            establishment.AddSalesItem(sale, salesItem);

            // Act
            sale.RemoveSalesItems(salesItem);

            // Assert
            Assert.DoesNotContain(salesItem, sale.GetSalesItems());
        }

        [Fact]
        public void RemoveSalesItem_WithItemFromDifferentEstablishment_ShouldNotRemoveSalesItem()
        {
            // Arrange
            SalesItems salesItem = establishment.CreateSalesItem(sale, item, 1);
            establishment.AddSalesItem(sale, salesItem);

            var otherEstalishment = new Establishment("Other establishment");
            var otherItem = otherEstalishment.CreateItem("Other item", 0);
            otherEstalishment.AddItem(otherItem);
            var otherSale = otherEstalishment.CreateSale(DateTime.Now);
            otherEstalishment.AddSale(otherSale);

            SalesItems otherSalesItem = establishment.CreateSalesItem(sale, otherItem, 1);

            // Act
            Action act = () => sale.RemoveSalesItems(salesItem);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.Equal(1, sale.GetSalesItems().Count);
        }

        [Fact]
        public void GetSalesItems_ShouldGetSalesItems()
        {
            // Arrange
            SalesItems salesItem = establishment.CreateSalesItem(sale, item, 1);
            establishment.AddSalesItem(sale, salesItem);

            // Act
            var fetchedSalesItems = sale.GetSalesItems();

            // Assert
            Assert.Contains(salesItem, fetchedSalesItems);
        }

    }
}
