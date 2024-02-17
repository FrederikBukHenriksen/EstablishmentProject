using EstablishmentProject.test.TestingCode;
using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test.Establishment_Test
{
    public class EstablishmentTableTest : IntegrationTest
    {
        private Establishment establishment;
        public EstablishmentTableTest() : base()
        {
            CommonArrange();
        }

        private void CommonArrange()
        {
            establishment = new Establishment("Test establishment");
        }

        [Fact]
        public void CreateTable_WithValidName_ShouldCreateTable()
        {
            // Arrange
            string tableName = "Table1";

            // Act
            Table table = establishment.CreateTable(tableName);

            // Assert
            Assert.Equal(table.Name, tableName);
        }

        [Fact]
        public void CreateTable_WithNameAlreadyInUse_ShouldNotCreateTable()
        {
            // Arrange
            establishment.AddTable(establishment.CreateTable("Table1"));
            string tableName = "Table1";

            // Act
            Action act = () => establishment.CreateTable(tableName);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }


        [Fact]
        public void AddTable_WithTableCreatedForEstablishment_ShouldAddTable()
        {
            // Arrange
            Table table = establishment.CreateTable("Table1");

            // Act
            establishment.AddTable(table);

            // Assert
            Assert.Contains(table, establishment.GetTables());

        }

        [Fact]
        public void AddTable_WithTableNotCreatedForEstablishment_ShouldNotAddTable()
        {
            // Arrange
            var otherEstablishment = new Establishment("Other establishment");
            Table table = new Table("Table1");

            // Act
            Action act = () => otherEstablishment.AddTable(table);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.DoesNotContain(table, otherEstablishment.Tables);
        }


        [Fact]
        public void RemoveTable_WhenTableNotUsed_ShouldRemoveTable()
        {
            // Arrange
            var table = establishment.AddTable(establishment.CreateTable("Table1"));

            // Act
            establishment.RemoveTable(table);

            // Assert
            Assert.DoesNotContain(table, establishment.GetTables());
        }

        [Fact]
        public void RemoveTable_WithTableUsedInSales_ShouldThrowException()
        {
            // Arrange
            var table = new Table("Table1");
            establishment.CreateTable(table.Name);
            establishment.CreateSale(DateTime.Now, tables: [table]);

            // Act & Assert
            Assert.Throws<Exception>(() => establishment.RemoveTable(table));
        }

        [Fact]
        public void GetTables_WhenTablesPresent_ShouldReturnListOfTables()
        {
            // Arrange
            establishment.AddTable(establishment.CreateTable("Table1"));
            establishment.AddTable(establishment.CreateTable("Table2"));

            // Act
            var tables = establishment.GetTables();

            // Assert
            Assert.Equal(2, tables.Count);
            Assert.Contains(tables, t => t.Name == "Table1");
            Assert.Contains(tables, t => t.Name == "Table2");
        }

















    }
}
