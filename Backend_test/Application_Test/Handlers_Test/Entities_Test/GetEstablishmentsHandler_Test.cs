using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.EstablishmentHandlers;
using WebApplication1.Application_Layer.Handlers.SalesHandlers;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.Controllers;
using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Application_Test.Handlers_Test.Entities_Test
{
    public class GetEstablishmentsHandler_Test : IntegrationTest
    {
        private IUnitOfWork unitOfWork;
        private GetSalesCommand getSalesCommand_WithSaleIds;
        private GetSalesCommand getSalesCommand_WithSalesSorting;
        private Establishment establishment;
        private Sale sale;
        private GetEstablishmentsCommand getEstablishmentsCommand;

        public GetEstablishmentsHandler_Test() : base(new List<ITestService> { DatabaseTestContainer.CreateAsync().Result })
        {
            //Inject services
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            createTestData();


            getEstablishmentsCommand = new GetEstablishmentsCommand
            {
                EstablishmentIds = new List<Guid> { establishment.Id }
            };
        }

        private void createTestData()
        {
            establishment = new Establishment("Test establishment");
            using (var uow = unitOfWork)
            {
                uow.establishmentRepository.Add(establishment);
            }
        }

        [Fact]
        public async Task GetEstablishments_WithGetEstablishmentsIdReturn_ShouldReturnId()
        {
            //Arrange
            var handler = scope.ServiceProvider.GetRequiredService<IHandler<GetEstablishmentsCommand, GetEstablishmentsIdReturn>>();

            //Act
            GetEstablishmentsIdReturn result = await handler.Handle(getEstablishmentsCommand);

            //Assert
            Assert.IsType<Guid>(result.ids.First());
            Assert.Equal(establishment.Id, result.ids.First());
        }

        [Fact]
        public async Task GetEstablishments_WithGetEstablishmentsIdReturn_ShouldReturnDTO()
        {
            //Arrange
            var handler = scope.ServiceProvider.GetRequiredService<IHandler<GetEstablishmentsCommand, GetEstablishmentsDTOReturn>>();

            //Act
            GetEstablishmentsDTOReturn result = await handler.Handle(getEstablishmentsCommand);

            //Assert
            Assert.IsType<EstablishmentDTO>(result.dtos.First());
            Assert.Equal(establishment.Id, result.dtos.First().Id);
        }


        [Fact]
        public async Task GetEstablishments_WithGetEstablishmentsIdReturn_ShouldReturnEntity()
        {
            //Arrange
            var handler = scope.ServiceProvider.GetRequiredService<IHandler<GetEstablishmentsCommand, GetEstablishmentsEntityReturn>>();

            //Act
            GetEstablishmentsEntityReturn result = await handler.Handle(getEstablishmentsCommand);

            //Assert
            Assert.IsType<Establishment>(result.entities.First());
            Assert.Equal(establishment.Id, result.entities.First().Id);
        }
    }
}