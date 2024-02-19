using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure_Layer.DataTransferObjects;

namespace EstablishmentProject.test.Infrastructure_Layer.DTO
{
    public class ItemDTO_Test
    {
        [Fact]
        public void ItemDTO_ShouldCreateItemDTO()
        {
            //Arrange
            var establishment = new Establishment("Test establishment");
            var item = establishment.CreateItem("Test Item", 0.0);

            //Act
            var DTO = new ItemDTO(item);

            //Assert
            Assert.IsType<ItemDTO>(DTO);
            Assert.Equal(item.Id, DTO.Id);
            Assert.Equal(item.Name, DTO.Name);
        }
    }
}
