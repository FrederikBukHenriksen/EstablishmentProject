using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain
{
    public class EstablishmentItemTest : BaseIntegrationTest
    {

        public EstablishmentItemTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public void CreateItem_ShouldCreateNewItem()
        {
            // Arrange
            var establishment = new Establishment();
            string itemName = "New Item";
            double itemPrice = 10.00;
            Currency itemCurrency = Currency.DKK;

            // Act
            var item = establishment.CreateItem(itemName, itemPrice, itemCurrency);

            // Assert
            Assert.NotNull(item);
            Assert.Equal(itemName, item.GetName());
            Assert.Equal(itemPrice, item.GetPrice().Amount);
            Assert.Equal(itemCurrency, item.GetPrice().Currency);
        }

        [Fact]
        public void GetItems_ShouldReturnAllAddedItems()
        {
            // Arrange
            var establishment = new Establishment();
            var item1 = establishment.AddItem(establishment.CreateItem("Item1", 10.00, Currency.DKK));
            var item2 = establishment.AddItem(establishment.CreateItem("Item2", 20.00, Currency.DKK));
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
            var establishment = new Establishment();
            var item = establishment.AddItem(establishment.CreateItem("Item1", 10.00, Currency.DKK));
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
            var establishment = new Establishment();
            var item = establishment.AddItem(establishment.CreateItem("Item1", 10.00, Currency.DKK));
            establishment.CreateSale(DateTime.Now, itemAndQuantity: new List<(Item, int)> { (item, 1) });

            // Act
            Action act = () => establishment.RemoveItem(item);

            // Assert
            Assert.Throws<Exception>(act);
        }



    }
}
