using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test
{
    public class Establishment_Sale_Test
    {
        private Establishment establishment;

        public Establishment_Sale_Test()
        {
            CommonArrange();
        }

        private void CommonArrange()
        {
            establishment = new Establishment("Test establishment");
        }

        [Fact]
        public void CreateSale_ShouldCreateSale()
        {
            // Arrange
            var timestampPayment = DateTime.Now;
            var timestampArrival = DateTime.Now.AddHours(-1);
            var table = establishment.CreateTable("Table1");
            establishment.AddTable(table);
            var item = establishment.CreateItem("Item1", 10.00);
            establishment.AddItem(item);
            List<Table> tables = new List<Table>() { table };
            List<(Item, int)> itemsAndQuan = new List<(Item, int)>() { (item, 1) };

            // Act
            Sale sale = establishment.CreateSale(timestampPayment: timestampPayment, timestampArrival: timestampArrival, tables: tables, itemAndQuantity: itemsAndQuan);

            // Assert
            Assert.NotNull(sale);
            Assert.Equal(timestampPayment, sale.GetTimeOfPayment());
            Assert.Equal(timestampArrival, sale.GetTimeOfArrival());
            Assert.Equal(tables, sale.GetSalesTables().Select(x => x.Table).ToList());
            Assert.Equal(itemsAndQuan, sale.GetSalesItems().Select(x => (x.Item, x.quantity)).ToList());
        }

        [Fact]
        public void SetTimeOfPayment_WithNoTimeOfArrival_ShouldSetTimeOfPayment()
        {
            // Arrange
            var timestampPayment = DateTime.Now;
            var sale = establishment.CreateSale(timestampPayment);

            // Act
            var newTimestampPayment = DateTime.Now.AddHours(-1);
            sale.SetTimeOfPayment(newTimestampPayment);

            // Assert
            Assert.Equal(newTimestampPayment, sale.GetTimeOfPayment());
        }

        [Fact]
        public void SetTimeOfPayment_WithTimeBeingAfterTimeOfArrival_ShouldSetTimeOfPayment()
        {
            // Arrange
            var timestampPayment = DateTime.Now;
            var timestampArrival = DateTime.Now.AddHours(-1);
            var sale = establishment.CreateSale(timestampPayment, timestampArrival: timestampArrival);

            // Act
            var newTimestampPayment = DateTime.Now.AddHours(-1);
            sale.SetTimeOfPayment(newTimestampPayment);

            // Assert
            Assert.Equal(newTimestampPayment, sale.GetTimeOfPayment());
        }

        public void SetTimeOfPayment_WithTimeBeingBeforeTimeOfArrival_ShouldSetTimeOfPayment()
        {
            // Arrange
            var timestampPayment = DateTime.Now;
            var timestampArrival = DateTime.Now.AddHours(1);
            var sale = establishment.CreateSale(timestampPayment, timestampArrival: timestampArrival);

            // Act
            Action act = () => sale.SetTimeOfPayment(DateTime.Now.AddHours(-1));

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void AddSale_WithSaleCreatedForEstablishment_ShouldAddSale()
        {
            // Arrange
            var sale = establishment.CreateSale(DateTime.Now);

            // Act
            establishment.AddSale(sale);

            // Assert
            Assert.Contains(sale, establishment.GetSales());
        }

        public void AddSale_WithSaleCreatedForDifferentEstablishment_ShouldNotAddSale()
        {
            // Arrange
            var otherEstablishment = new Establishment();
            var sale = otherEstablishment.CreateSale(DateTime.Now);

            // Act
            Action act = () => establishment.AddSale(sale);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }



        [Fact]
        public void GetSales_WithMultipleSales_ShouldReturnAllSales()
        {
            // Arrange
            var sale1 = establishment.CreateSale(DateTime.Now);
            var sale2 = establishment.CreateSale(DateTime.Now);

            // Act
            establishment.AddSale(sale1);
            establishment.AddSale(sale2);

            // Assert
            var sales = establishment.GetSales();
            Assert.Equal(2, sales.Count);
            Assert.Contains(sale1, sales);
            Assert.Contains(sale2, sales);
        }

        [Fact]
        public void RemoveSale_WithSaleFromEstablishment_ShouldRemoveSale()
        {
            // Arrange
            var sale = establishment.CreateSale(DateTime.Now);
            establishment.AddSale(sale);

            // Act
            establishment.RemoveSale(sale);

            // Assert
            Assert.DoesNotContain(sale, establishment.GetSales());
        }

        [Fact]
        public void RemoveSale_WithSaleFromDifferentEstablishment_ShouldNotRemoveSale()
        {
            // Arrange
            var sale = establishment.CreateSale(DateTime.Now);
            establishment.AddSale(sale);


            var otherEstablishment = new Establishment();
            var otherSale = otherEstablishment.CreateSale(DateTime.Now);

            // Act
            Action act = () => establishment.RemoveSale(otherSale);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.Contains(sale, establishment.GetSales());
        }
    }
}
