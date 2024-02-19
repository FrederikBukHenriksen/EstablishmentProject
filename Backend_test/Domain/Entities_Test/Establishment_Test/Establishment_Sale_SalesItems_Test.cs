using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test
{
    public class Establishment_Sale_SalesItems_Test
    {
        private Establishment establishment;
        private Item item;
        private Sale sale;

        public Establishment_Sale_SalesItems_Test()
        {
            CommonArrange();
        }

        private void CommonArrange()
        {
            establishment = new Establishment("Test establishment");
            item = establishment.CreateItem("Test item", 10);
            establishment.AddItem(item);
            sale = establishment.CreateSale(DateTime.Now);
            establishment.AddSale(sale);
        }

        [Fact]
        public void CreateSalesItem_WithItemCreatedForEstablishmentAndPositiveQuantity_ShouldCreateSalesItems()
        {
            //Arrange

            //Act
            var salesItems = establishment.CreateSalesItem(sale, item, 1);

            //Assert
            Assert.NotNull(salesItems);
        }

        [Fact]
        public void CreateSalesItem_WithItemCreatedForEstablishmentAndNegativeQuantity_ShouldNotCreateSalesItems()
        {
            //Arrange

            //Act
            Action act = () => establishment.CreateSalesItem(sale, item, -1);

            //Assert
            Assert.Throws<ArgumentException>(act);
            Assert.Empty(sale.GetSalesItems());
        }

        [Fact]
        public void CreateSalesItem_WithItemCreatedForDifferentEstablishmentAndPositiveQuantity_ShouldNotCreateSalesItems()
        {
            //Arrange
            var otherEstablishment = new Establishment("Other establishment");
            var otherItem = otherEstablishment.CreateItem("Other item", 10);

            //Act
            Action act = () => establishment.CreateSalesItem(sale, otherItem, 1);

            //Arrange
            Assert.Throws<InvalidOperationException>(act);
            Assert.Empty(sale.GetSalesItems());
        }

        [Fact]
        public void AddSalesItems_WithSalesItemsForSale_ShouldAddSalesItems()
        {
            //Arrange
            var salesItems = establishment.CreateSalesItem(sale, item, 1);

            //Act
            establishment.AddSalesItems(sale, salesItems);

            //Assert
            Assert.Contains(salesItems, sale.GetSalesItems());
        }

        [Fact]
        public void AddSalesItems_WithSaleCreatedForDifferentDifferentEstablishment_ShouldAddSalesItems()
        {
            //Arrange
            var otherEstablishment = new Establishment("Other establishment");
            var otherSale = otherEstablishment.CreateSale(DateTime.Now);
            otherEstablishment.AddSale(otherSale);
            var salesItems = otherEstablishment.CreateSalesItem(otherEstablishment.CreateSale(DateTime.Now), item, 1);

            //Act
            Action act = () => establishment.AddSalesItems(sale, salesItems);

            //Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.Empty(sale.GetSalesItems());
        }

        [Fact]
        public void AddSalesItems_WithSalesItemsFromDifferentSale_ShouldNotAddSalesItems()
        {
            //Arrange
            var otherEstablishment = new Establishment("Other establishment");
            var otherItem = otherEstablishment.CreateItem("Other item", 10);
            otherEstablishment.AddItem(otherItem);
            var overSale = otherEstablishment.CreateSale(DateTime.Now);
            otherEstablishment.AddSale(overSale);
            var salesItems = otherEstablishment.CreateSalesItem(overSale, otherItem, 1);

            //Act
            Action act = () => establishment.AddSalesItems(sale, salesItems);

            //Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.Empty(sale.GetSalesItems());
        }

        [Fact]
        public void AddSalesItems_WithItemThatAlreadyExistingForSale_SholdNotAddSalesItems()
        {
            //Arrange
            var salesItems1 = establishment.CreateSalesItem(sale, item, 1);
            establishment.AddSalesItems(sale, salesItems1);
            var salesItems2 = establishment.CreateSalesItem(sale, item, 2);

            //Act
            Action act = () => establishment.AddSalesItems(sale, salesItems2);

            //Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.Single(sale.GetSalesItems());
        }

    }
}
