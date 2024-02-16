using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test.Establishment_Test
{
    public class EstablishmentTableTest : BaseIntegrationTest
    {
        public EstablishmentTableTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public void CreateTable_WithValidName_ShouldCreateTable()
        {
            // Arrange
            var establishment = new Establishment();
            string tableName = "Table1";

            // Act
            establishment.AddTable(establishment.CreateTable(tableName));

            // Assert
            Assert.Contains(establishment.GetTables(), t => t.Name == tableName);
        }

        [Fact]
        public void CreateTable_WithEmptyName_ShouldThrowArgumentException()
        {
            // Arrange
            var establishment = new Establishment();
            string tableName = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => establishment.CreateTable(tableName));
        }

        [Fact]
        public void RemoveTable_WhenTableNotUsed_ShouldRemoveTable()
        {
            // Arrange
            var establishment = new Establishment();
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
            var establishment = new Establishment();
            var table = new Table("Table1");
            establishment.CreateTable(table.Name);
            establishment.CreateSale(DateTime.Now, table: table);

            // Act & Assert
            Assert.Throws<Exception>(() => establishment.RemoveTable(table));
        }

        [Fact]
        public void GetTables_WhenTablesPresent_ShouldReturnListOfTables()
        {
            // Arrange
            var establishment = new Establishment();
            establishment.AddTable(establishment.CreateTable("Table1"));
            establishment.AddTable(establishment.CreateTable("Table2"));

            // Act
            var tables = establishment.GetTables();

            // Assert
            Assert.Equal(2, tables.Count);
            Assert.Contains(tables, t => t.Name == "Table1");
            Assert.Contains(tables, t => t.Name == "Table2");
        }

        [Fact]
        public void GetTables_WhenNoTables_ShouldReturnEmptyList()
        {
            // Arrange
            var establishment = new Establishment();

            // Act
            var tables = establishment.GetTables();

            // Assert
            Assert.Empty(tables);
        }
















    }
}
