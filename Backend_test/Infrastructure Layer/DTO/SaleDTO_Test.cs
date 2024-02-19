using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure_Layer.DataTransferObjects;

namespace EstablishmentProject.test.Infrastructure_Layer.DTO
{
    public class SaleDTO_Test
    {
        [Fact]
        public void SaleDTO_ShouldCreateSaleDTO()
        {
            //Arrange
            var establishment = new Establishment("Test establishment");
            var item = establishment.CreateItem("Test Item", 0.0);
            establishment.AddItem(item);
            var table = establishment.CreateTable("Test table");
            establishment.AddTable(table);

            var sale = establishment.CreateSale(DateTime.Now);
            sale.setTimeOfArrival(DateTime.Now.AddHours(-1));
            establishment.AddSale(sale);

            var salesItems = establishment.CreateSalesItem(sale, item, 1);
            establishment.AddSalesItems(sale, salesItems);
            var salesTales = establishment.CreateSalesTables(sale, table);
            establishment.AddSalesTables(sale, salesTales);

            //Act
            var DTO = new SaleDTO(sale);

            //Assert
            Assert.IsType<SaleDTO>(DTO);
            Assert.Equal(sale.Id, DTO.Id);
            Assert.Equal(sale.GetTimeOfArrival(), DTO.TimestampArrival);
            Assert.Equal(sale.GetTimeOfPayment(), DTO.TimestampPayment);
            Assert.Equal(sale.GetSalesItems()[0].Id, DTO.SalesItems[0].Item1);
            Assert.Equal(sale.GetSalesItems()[0].quantity, DTO.SalesItems[0].Item2);
            Assert.Equal(sale.GetSalesTables()[0].Id, DTO.SalesTables[0]);
        }
    }
}
