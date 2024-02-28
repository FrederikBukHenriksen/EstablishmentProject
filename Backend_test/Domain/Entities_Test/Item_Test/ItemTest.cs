using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test
{
    public class ItemTest
    {
        private Establishment establishment;

        public ItemTest()
        {
            CommonArrange();
        }

        private void CommonArrange()
        {
            establishment = new Establishment("Test Establishment");
        }

        [Fact]
        public void Constructor_ShouldCreateItem()
        {
            // Arrange
            string name = "Test item";
            double price = 10.00;

            // Act
            Item item = new Item(establishment, name, price);

            // Assert
            Assert.NotNull(item);
            Assert.Equal(name, item.GetName());
            Assert.Equal(price, item.GetPrice());
        }

        [Fact]
        public void SetName_ShouldSetName()
        {
            // Arrange
            Item item = new Item(establishment, "Item", 0);
            string name = "New name";

            // Act
            item.SetName(name);

            // Assert
            Assert.Equal(name, item.GetName());
        }

        [Fact]
        public void SetName_WithEmptyName_ShouldNotSetName()
        {
            // Arrange
            Item item = new Item(establishment, "Item", 0);
            string name = "";

            // Act
            Action act = () => item.SetName(name);

            // Assert
            Assert.Throws<ArgumentException>(act);
            Assert.NotEqual(name, item.GetName());
        }

        [Fact]
        public void SetPrice_ShouldSetPrice()
        {
            // Arrange
            Item item = new Item(establishment, "Item", 0);
            double price = 10.00;

            // Act
            item.SetPrice(price);

            // Assert
            Assert.Equal(price, item.GetPrice());
        }

        [Fact]
        public void SetPrice_WithNegativePrice_ShouldNotSetPrice()
        {
            // Arrange
            Item item = new Item(establishment, "Item", 0);
            double price = -10.00;

            // Act
            Action act = () => item.SetPrice(price);

            // Assert
            Assert.Throws<ArgumentException>(act);
            Assert.NotEqual(price, item.GetPrice());
        }

    }
}
