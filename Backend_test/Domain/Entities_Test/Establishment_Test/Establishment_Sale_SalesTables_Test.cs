using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test
{
    public class Establishment_Sale_SalesTables_Test
    {
        private Establishment establishment;
        private Table table;
        private Sale sale;

        public Establishment_Sale_SalesTables_Test()
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
        public void CreateSalesTables_WithSaleAndTableForEstablishment_ShouldCreateSalesTables()
        {
            //Arrange
            var salesTables = establishment.CreateSalesTables(sale, table);
            //Act
            establishment.AddSalesTables(sale, salesTables);
            //Assert
            Assert.Equal(salesTables, sale.SalesTables[0]);
        }

        [Fact]
        public void CreateSalesTables_WithTableCreatedForDifferentEstalishment_ShouldNotCreateSalesTables()
        {
            //Arrange
            var otherEstablishment = new Establishment("Other establishment");
            var otherTable = otherEstablishment.CreateTable("Other table");
            otherEstablishment.AddTable(otherTable);
            //var otherSale = otherEstablishment.CreateSale(DateTime.Now);

            //Act
            Action act = () => otherEstablishment.CreateSalesTables(sale, otherTable);

            //Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.Empty(sale.SalesTables);
        }

        [Fact]
        public void CreateSalesTables_WithSaleCreatedForDifferentEstalishment_ShouldNotCreateSalesTables()
        {
            //Arrange
            var otherEstablishment = new Establishment("Other establishment");
            var otherTable = otherEstablishment.CreateTable("Other table");
            otherEstablishment.AddTable(otherTable);
            var otherSale = otherEstablishment.CreateSale(DateTime.Now);

            //Act
            Action act = () => establishment.CreateSalesTables(otherSale, table);

            //Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.Empty(sale.SalesTables);
        }


        [Fact]
        public void AddSalesTables_WithSalesTablesCreatedForEstablishment_ShouldAddSalesTables()
        {
            //Arrange
            var salesTables = establishment.CreateSalesTables(sale, table);
            //Act
            establishment.AddSalesTables(sale, salesTables);
            //Assert
            Assert.Equal(salesTables, sale.SalesTables[0]);
        }


        [Fact]
        public void AddSalesTables_WithSalesTablesCreatedForDifferentEstalishment_ShouldNotAddSalesTables()
        {
            //Arrange
            var otherEstablishment = new Establishment("Other establishment");
            var otherTable = otherEstablishment.CreateTable("Other table");
            otherEstablishment.AddTable(otherTable);
            var otherSale = otherEstablishment.CreateSale(DateTime.Now);
            otherEstablishment.AddSale(otherSale);
            var salesTables = otherEstablishment.CreateSalesTables(otherSale, otherTable);

            //Act
            Action act = () => establishment.AddSalesTables(sale, salesTables);

            //Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.Empty(sale.SalesTables);
        }

        public void AddSalesTables_WithTableThatAlreadyExistingForSale_SholdNotAddSalesTables()
        {
            //Arrange
            var salesTables1 = establishment.CreateSalesTables(sale, table);
            establishment.AddSalesTables(sale, salesTables1);
            var salesTables2 = establishment.CreateSalesTables(sale, table);


            //Act
            Action act = () => establishment.AddSalesTables(sale, salesTables2);

            //Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.Single(sale.SalesTables);
        }


        [Fact]
        public void RemoveSalesTables_WithSalesTableForSale_ShouldRemoveSalesTables()
        {
            //Arrange
            var salesTables = establishment.CreateSalesTables(sale, table);
            establishment.AddSalesTables(sale, salesTables);

            //Act
            sale.RemoveSalesTables(salesTables);

            //Assert
            Assert.Empty(sale.SalesTables);
        }
        [Fact]
        public void RemoveSalesTables_WithSalesTableFromDifferentFromSale_ShouldRemoveSalesTables()
        {
            //Arrange
            var otherEstablishment = new Establishment("Other establishment");
            var otherTable = otherEstablishment.CreateTable("Other table");
            otherEstablishment.AddTable(otherTable);
            var otherSale = otherEstablishment.CreateSale(DateTime.Now);
            otherEstablishment.AddSale(otherSale);
            var salesTables = otherEstablishment.CreateSalesTables(otherSale, otherTable);

            //Act
            Action act = () => sale.RemoveSalesTables(salesTables);

            //Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.Empty(sale.SalesTables);
        }
    }
}

