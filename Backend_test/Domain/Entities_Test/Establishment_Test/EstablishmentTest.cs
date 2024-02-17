using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test.Establishment_Test
{
    public class EstablishmentTest
    {
        [Fact]
        public void Constructor_ShouldReturnnEstablishment()
        {
            // Arrange
            string name = "Test establishment";
            Table table = new Table("Table");
            Item item = new Item("Item", 10.00);
            Sale sale = new Sale(DateTime.Now);

            // Act

            Establishment establishment = new Establishment(name, [item], [table], [sale]);

            // Assert
            Assert.NotNull(establishment);
            Assert.Equal(name, establishment.GetName());
            Assert.Contains(table, establishment.GetTables());
            Assert.Contains(item, establishment.GetItems());
            Assert.Contains(sale, establishment.GetSales());
        }

        [Fact]
        public void SetName_ShouldSetName()
        {
            // Arrange
            Establishment establishment = new Establishment();
            string name = "New name";

            // Act
            var result = establishment.SetName(name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(name, establishment.GetName());
        }

        public void SetName_WithEmptyName_ShouldThrowException()
        {
            // Arrange
            Establishment establishment = new Establishment();
            string name = "";

            // Act
            Action act = () => establishment.SetName(name);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }


        [Fact]
        public void GetName_ShouldReturnName()
        {
            // Arrange
            string name = "Test establishment";
            Establishment establishment = new Establishment(name);

            // Act
            var result = establishment.GetName();

            // Assert
            Assert.Equal(name, result);
        }
    }
}
