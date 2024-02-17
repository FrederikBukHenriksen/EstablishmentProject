using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test
{
    public class TableTest
    {
        [Fact]
        public void Constructor_ShouldCreateTable()
        {
            // Arrange
            string name = "Table";

            // Act
            Table table = new Table(name);


            // Assert
            Assert.NotNull(table);
            Assert.Equal(name, table.Name);
        }

        [Fact]
        public void SetName_ShouldSetName()
        {
            // Arrange
            Table table = new Table();
            string name = "New name";

            // Act
            table.SetName(name);

            // Assert
            Assert.Equal(name, table.Name);
        }

        public void SetName_WithEmptyName_ShouldThrowException()
        {
            // Arrange
            Table table = new Table();
            string name = "";

            // Act
            Action act = () => table.SetName(name);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }
    }
}
