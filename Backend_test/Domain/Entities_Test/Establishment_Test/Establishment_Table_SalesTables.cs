using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test
{
    public class Establishment_Table_SalesTables
    {
        private Establishment establishment;
        private Table table;
        private Sale sale;

        public Establishment_Table_SalesTables()
        {
            CommonArrange();
        }

        private void CommonArrange()
        {
            establishment = new Establishment("Test establishment");
            table = establishment.CreateTable("Test table");
            establishment.AddTable(table);
            sale = establishment.CreateSale(DateTime.Now);
            establishment.AddSale(sale);
        }

        [Fact]
        public void CreateSalesTables_ShouldCreateSalesTables()
        {
            //Arrange
            var salesTables = establishment.CreateSalesTables(sale, table);
            //Act
            establishment.AddSalesTables(sale, salesTables);
            //Assert
            Assert.Equal(salesTables, sale.SalesTables[0]);
        }

        [Fact]
        public void AddSalesTables_WithTableCreatedForEstalishment_ShouldAddSalesTables()
        {
            //Arrange
            var salesTables = establishment.CreateSalesTables(sale, table);
            //Act
            establishment.AddSalesTables(sale, salesTables);
            //Assert
            Assert.Equal(salesTables, sale.SalesTables[0]);
        }

        [Fact]
        public void AddSalesTables_WithSalesTableCreatedForDifferentEstalishment_ShouldNotAddSalesTables()
        {
            //Arrange
            var otherEstablishment = new Establishment("Other establishment");
            var otherTable = otherEstablishment.CreateTable("Other table");
            var salesTables = otherEstablishment.CreateSalesTables(sale, otherTable);

            //Act
            Action act = () => establishment.AddSalesTables(sale, salesTables);

            //Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.Empty(sale.SalesTables);

        }

    }
}
