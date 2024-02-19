using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test
{
    public class Establishment_Table_Test
    {
        private Establishment establishment;
        public Establishment_Table_Test()
        {
            CommonArrange();
        }

        private void CommonArrange()
        {
            establishment = new Establishment("Test establishment");
        }

        [Fact]
        public void CreateTable_WithValidAndUniqueName_ShouldCreateTable()
        {
            // Arrange
            string tableName = "Table1";

            // Act
            Table table = establishment.CreateTable(tableName);

            // Assert
            Assert.Equal(table.Name, tableName);
        }

        [Fact]
        public void SetName_WithValidName_ShouldSetName()
        {
            // Arrange
            var table = establishment.CreateTable("Table1");
            string newName = "Table2";

            // Act
            establishment.SetTableName(table, newName);

            // Assert
            Assert.Equal(newName, table.Name);
        }

        [Fact]
        public void SetName_WithTableNameAlreadyInUse_ShouldNotSetName()
        {
            // Arrange
            var table1 = establishment.CreateTable("Table1");
            establishment.AddTable(table1);
            var table2 = establishment.CreateTable("Table2");
            establishment.AddTable(table2);

            // Act
            Action act = () => establishment.SetTableName(table2, "Table1");

            // Assert
            Assert.Throws<ArgumentException>(act);
            Assert.Equal("Table2", table2.Name);
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
        public void AddTable_WithTableCreatedForDifferentEstablishment_ShouldNotAddTable()
        {
            // Arrange
            var otherEstablishment = new Establishment("Other establishment");
            Table table = otherEstablishment.CreateTable("Table1");

            // Act
            Action act = () => establishment.AddTable(table);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.DoesNotContain(table, establishment.GetTables());
        }

        [Fact]
        public void AddTable_WithTableAlreadyInEstablishment_ShouldNotAddTable()
        {
            // Arrange
            var table = establishment.CreateTable("Table1");
            establishment.AddTable(table);

            // Act
            Action act = () => establishment.AddTable(table);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }


        [Fact]
        public void RemoveTable_WhenTableNotUsed_ShouldRemoveTable()
        {
            // Arrange
            var table = establishment.CreateTable("Table1");
            establishment.AddTable(table);

            // Act
            establishment.RemoveTable(table);

            // Assert
            Assert.DoesNotContain(table, establishment.GetTables());
        }

        [Fact]
        public void RemoveTable_WithTableUsedInSales_ShouldNotRemoveTable()
        {
            // Arrange
            var table = establishment.CreateTable("Table1");
            establishment.AddTable(table);
            var sale = establishment.CreateSale(DateTime.Now, tables: [table]);
            establishment.AddSale(sale);

            // Act
            Action act = () => establishment.RemoveTable(table);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.Contains(table, establishment.GetTables());
        }

        [Fact]
        public void GetTables_WithMultipleTables_ShouldReturnAllTables()
        {
            // Arrange
            var table1 = establishment.CreateTable("Table1");
            establishment.AddTable(table1);
            var table2 = establishment.CreateTable("Table2");
            establishment.AddTable(table2);

            // Act
            var tables = establishment.GetTables();

            // Assert
            Assert.Equal(2, tables.Count);
            Assert.Contains(table1, tables);
            Assert.Contains(table2, tables);
        }

















    }
}
