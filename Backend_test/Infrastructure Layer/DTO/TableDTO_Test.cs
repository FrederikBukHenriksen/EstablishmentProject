using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure_Layer.DataTransferObjects;

namespace EstablishmentProject.test.Infrastructure_Layer.DTO
{
    public class TableDTO_Test
    {
        [Fact]
        public void TableDTO_ShouldCreateTableDTO()
        {
            //Arrange
            var establishment = new Establishment("Test establishment");
            var table = establishment.CreateTable("Test table");

            //Act
            var DTO = new TableDTO(table);

            //Assert
            Assert.IsType<TableDTO>(DTO);
            Assert.Equal(table.Id, DTO.Id);
            Assert.Equal(table.Name, DTO.Name);
        }

    }
}
