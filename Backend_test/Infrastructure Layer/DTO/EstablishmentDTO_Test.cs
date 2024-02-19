using WebApplication1.Controllers;
using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Infrastructure_Layer.DTO
{
    public class EstablishmentDTO_Test
    {
        [Fact]
        public void EstablishmentDTO_ShouldCreateEstablishmentDTO()
        {
            //Arrange
            var establishment = new Establishment("Test establishment");
            var item = establishment.CreateItem("Test Item", 0.0);
            establishment.AddItem(item);
            var table = establishment.CreateTable("Test table");
            establishment.AddTable(table);
            var sale = establishment.CreateSale(DateTime.Now);
            establishment.AddSale(sale);

            //Act
            var DTO = new EstablishmentDTO(establishment);

            //Assert
            Assert.IsType<EstablishmentDTO>(DTO);
            Assert.Equal(establishment.Id, DTO.Id);
            Assert.Equal(establishment.Name, DTO.Name);
            Assert.Equal(establishment.GetItems()[0].Id, DTO.Items[0]);
            Assert.Equal(establishment.GetTables()[0].Id, DTO.Tables[0]);
            Assert.Equal(establishment.GetSales()[0].Id, DTO.Sales[0]);
        }
    }
}
