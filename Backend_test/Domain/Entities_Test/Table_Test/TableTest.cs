using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test
{
    public class TableTest
    {
        private Establishment establishment;

        public TableTest()
        {
            CommonArrange();
        }

        private void CommonArrange()
        {
            establishment = new Establishment("Test establishment");
        }
        [Fact]
        public void Constructor_ShouldCreateTable()
        {
            // Arrange
            string name = "Table";

            // Act
            Table table = new Table(establishment, name);


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
