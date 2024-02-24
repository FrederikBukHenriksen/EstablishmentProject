using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Handlers.SalesHandlers;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure_Layer.DataTransferObjects;

namespace EstablishmentProject.test.Application_Test.Handlers_Test.Entities_Test
{
    public class GetTablesHandler_Test : IntegrationTest
    {
        private IUnitOfWork unitOfWork;
        private Establishment establishment;
        private Sale sale;
        private GetTablesCommand command;

        public GetTablesHandler_Test() : base()
        {
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            createTestData();
            command = new GetTablesCommand
            {
                EstablishmentId = establishment.Id
            };
        }

        private void createTestData()
        {
            establishment = new Establishment("Test establishment");
            var table = establishment.CreateTable("Test table");
            establishment.AddTable(table);
            using (var uow = unitOfWork)
            {
                uow.establishmentRepository.Add(establishment);
            }
        }

        [Fact]
        public async Task GetTables_WithGetTablesIdReturn_ShouldReturnId()
        {
            //Arrange
            IHandler<GetTablesCommand, GetTablesIdReturn> handler = scope.ServiceProvider.GetRequiredService<IHandler<GetTablesCommand, GetTablesIdReturn>>();

            //Act
            GetTablesIdReturn result = await handler.Handle(command);

            //Assert
            Assert.Equal(1, result.Tables.Count);
            Assert.IsType<Guid>(result.Tables.First());

        }

        [Fact]
        public async Task GetTables_WithGetTablesDTOReturn_ShouldReturnDTO()
        {
            //Arrange
            IHandler<GetTablesCommand, GetTablesDTOReturn> handler = scope.ServiceProvider.GetRequiredService<IHandler<GetTablesCommand, GetTablesDTOReturn>>();

            //Act
            GetTablesDTOReturn result = await handler.Handle(command);

            //Assert
            Assert.Equal(1, result.Tables.Count);
            Assert.IsType<TableDTO>(result.Tables.First());
        }
        [Fact]
        public async Task GetTables_WithGetTablesRawReturn_ShouldReturnEntity()
        {
            //Arrange
            IHandler<GetTablesCommand, GetTablesRawReturn> handler = scope.ServiceProvider.GetRequiredService<IHandler<GetTablesCommand, GetTablesRawReturn>>();

            //Act
            GetTablesRawReturn result = await handler.Handle(command);

            //Assert
            Assert.Equal(1, result.Tables.Count);
            Assert.IsType<Table>(result.Tables.First());
        }
    }
}

