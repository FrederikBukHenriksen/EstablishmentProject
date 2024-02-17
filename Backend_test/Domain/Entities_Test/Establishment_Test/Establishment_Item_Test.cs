using EstablishmentProject.test.TestingCode;
using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test
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
            Assert.Equal(itemPrice, item.GetPrice());
        }


        [Fact]
        public void SetItemName_WithValidAndUniqueName_ShouldSetName()
        {
            // Arrange
            string itemName = "New Item";
            double itemPrice = 10.00;
            var item = establishment.CreateItem(itemName, itemPrice);
            string newName = "New Name";

            // Act
            item.SetName(newName);

            // Assert
            Assert.Equal(newName, item.GetName());
        }

        [Fact]
        public void SetItemName_WithNameAlreadyInUse_ShouldNotSetName()
        {
            // Arrange
            string itemName = "New Item";
            double itemPrice = 10.00;
            var item = establishment.CreateItem(itemName, itemPrice);
            establishment.AddItem(item);
            var otherItem = establishment.CreateItem("Other Item", 20.00);

            // Act
            Action act = () => establishment.SetItemName(otherItem, itemName);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }


        public void SetPrice_WithPositivePrice_ShouldSetPrice()
        {
            // Arrange
            var item = establishment.CreateItem("Item1", 10.00);
            double price = 20.00;

            // Act
            item.SetPrice(price);

            // Assert
            Assert.Equal(price, item.GetPrice());
        }

        public void SetPrice_WithNegativePrice_ShouldNotSetPrice()
        {
            // Arrange
            var item = establishment.CreateItem("Item1", 10.00);
            double price = -20.00;

            // Act
            Action act = () => item.SetPrice(price);

            // Assert
            Assert.Throws<ArgumentException>(act);
            Assert.Equal(10.00, item.GetPrice());
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

        public void AddItem_WithItemNotCreatedForAnEstablishment_ShouldNotAddItem()
        {
            // Arrange
            var otherEstablishment = new Establishment();
            var item = otherEstablishment.CreateItem("Item1", 10.00);

            // Act
            Action act = () => establishment.AddItem(item);

            // Assert
            Assert.Throws<Exception>(act);
            Assert.True(establishment.GetItems().Count == 0);
        }

        public void AddItem_WithItemCreatedForDifferentEstablishment_ShouldNotAddItem()
        {
            // Arrange
            var otherEstablishment = new Establishment();
            var item = otherEstablishment.CreateItem("Item1", 10.00);

            // Act
            Action act = () => establishment.AddItem(item);

            // Assert
            Assert.Throws<Exception>(act);
            Assert.True(establishment.GetItems().Count == 0);
        }

        [Fact]
        public void GetItems_ShouldReturnAllAddedItems()
        {
            // Arrange

            var item1 = establishment.CreateItem("Item1", 10.00);
            establishment.AddItem(item1);

            var item2 = establishment.CreateItem("Item2", 20.00);
            establishment.AddItem(item2);

            // Act
            var items = establishment.GetItems();

            // Assert
            Assert.Equal(2, items.Count);
            Assert.Contains(item1, items);
            Assert.Contains(item2, items);
        }

        [Fact]
        public void RemoveItem_WithItemNotUsed_ShouldRemoveItem()
        {
            // Arrange
            var item = establishment.CreateItem("Item1", 10.00);
            establishment.AddItem(item);

            // Act
            establishment.RemoveItem(item);

            // Assert
            var items = establishment.GetItems();
            Assert.DoesNotContain(item, items);
        }

        [Fact]
        public void RemoveItem_WhenItemInUse_ShouldNotRemoveItem()
        {
            // Arrange
            var item = establishment.CreateItem("Item1", 10.00);
            establishment.AddItem(item);
            var sale = establishment.CreateSale(DateTime.Now, itemAndQuantity: new List<(Item, int)> { (item, 1) });
            establishment.AddSale(sale);

            // Act
            Action act = () => establishment.RemoveItem(item);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }



    }
}
