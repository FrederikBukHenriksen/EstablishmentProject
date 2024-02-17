using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Handlers.ItemHandler;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure_Layer.DataTransferObjects;

namespace EstablishmentProject.test.Application_Test.Handlers_Test.Entities_Test
{


    public class GetItemsHandler_Test : IntegrationTest
    {
        private IUnitOfWork unitOfWork;
        private GetItemsCommand getItemsCommand;
        private Establishment establishment;
        private Item item;


        public GetItemsHandler_Test() : base(new List<ITestService> { DatabaseTestContainer.CreateAsync().Result })
        {
            //Inject services
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            createTestData();

            getItemsCommand = new GetItemsCommand { EstablishmentId = establishment.Id };
        }

        private void createTestData()
        {
            establishment = new Establishment("Test establishment");
            item = establishment.AddItem(establishment.CreateItem("Test item", 0.0));

            using (var uow = unitOfWork)
            {
                uow.establishmentRepository.Add(establishment);
            }
        }

        [Fact]
        public async Task GetItems_WithGetItemsIdReturn_ShouldReturnId()
        {
            //Arrange
            var handler = scope.ServiceProvider.GetRequiredService<IHandler<GetItemsCommand, GetItemsIdReturn>>();

            //Act
            GetItemsIdReturn result = await handler.Handle(getItemsCommand);

            //Assert
            Assert.IsType<Guid>(result.id.First());
            Assert.Equal(item.Id, result.id.First());
        }


        [Fact]
        public async Task GetItems_WithGetItemsEntityReturn_ShouldReturnEntity()
        {
            //Arrange
            var handler = scope.ServiceProvider.GetRequiredService<IHandler<GetItemsCommand, GetItemsEntityReturn>>();

            //Act
            GetItemsEntityReturn result = await handler.Handle(getItemsCommand);

            //Assert
            Assert.IsType<Item>(result.entity.First());
            Assert.Equal(item.Id, result.entity.First().Id);
        }


        [Fact]
        public async Task GetItems_WithGetItemsDTOReturn_ShouldReturnDTO()
        {
            //Arrange
            var handler = scope.ServiceProvider.GetRequiredService<IHandler<GetItemsCommand, GetItemsDTOReturn>>();

            //Act
            GetItemsDTOReturn result = await handler.Handle(getItemsCommand);

            //Assert
            Assert.IsType<ItemDTO>(result.dto.First());
            Assert.Equal(item.Id, result.dto.First().Id);
        }
    }

}