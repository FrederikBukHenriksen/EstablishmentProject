using EstablishmentProject.test.TestingCode;
using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test.Establishment_Test
{
    public class EstablishmentItemTest : IntegrationTest
    {
        private Establishment establishment;

        public EstablishmentItemTest() : base()
        {
            CommonArrange();
        }

        private void CommonArrange()
        {
            establishment = new Establishment("Test establishment");
        }

        [Fact]
        public void CreateItem_ShouldCreateItem()
        {
            // Arrange
            string itemName = "New Item";
            double itemPrice = 10.00;

            // Act
            var item = establishment.CreateItem(itemName, itemPrice);

            // Assert
            Assert.NotNull(item);
            Assert.Equal(itemName, item.GetName());
            Assert.Equal(itemPrice, item.GetPrice().Amount);
        }

        [Fact]
        public void CreateItem_WithNameAlreadyInUse_ShouldNotCreateItem()
        {
            // Arrange
            string itemName = "New Item";
            double itemPrice = 10.00;
            establishment.AddItem(establishment.CreateItem(itemName, itemPrice));

            // Act
            Action act = () => establishment.CreateItem(itemName, itemPrice);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        public void AddItem_WithItemCreatedForEstablishment_ShouldAddItem()
        {
            // Arrange
            Item item = establishment.CreateItem("Item1", 10.00);

            // Act
            establishment.AddItem(item);

            // Assert
            Assert.Contains(item, establishment.GetItems());
        }

        public void AddItem_WithItemNotCreatedForEstablishment_ShouldNotAddItem()
        {
            // Arrange
            var otherEstablishment = new Establishment();
            var item = otherEstablishment.CreateItem("Item1", 10.00);

            // Act
            Action act = () => establishment.AddItem(item);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void GetItems_ShouldReturnAllAddedItems()
        {
            // Arrange
            var item1 = establishment.AddItem(establishment.CreateItem("Item1", 10.00));
            var item2 = establishment.AddItem(establishment.CreateItem("Item2", 20.00));
            // Act
            var items = establishment.GetItems();

            // Assert
            Assert.Equal(2, items.Count);
            Assert.Contains(item1, items);
            Assert.Contains(item2, items);
        }

        [Fact]
        public void RemoveItem_ShouldRemoveItemFromList()
        {
            // Arrange
            var item = establishment.AddItem(establishment.CreateItem("Item1", 10.00));
            // Act
            establishment.RemoveItem(item);

            // Assert
            var items = establishment.GetItems();
            Assert.DoesNotContain(item, items);
        }

        [Fact]
        public void RemoveItem_WhenItemUsedInSales_ShouldThrowException()
        {
            // Arrange
            var item = establishment.AddItem(establishment.CreateItem("Item1", 10.00));
            establishment.CreateSale(DateTime.Now, itemAndQuantity: new List<(Item, int)> { (item, 1) });

            // Act
            Action act = () => establishment.RemoveItem(item);

            // Assert
            Assert.Throws<Exception>(act);
        }



    }
}
