using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Handlers.SalesHandlers;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure_Layer.DataTransferObjects;
using WebApplication1.Utils;

namespace EstablishmentProject.test.Application_Test.Handlers_Test.Entities_Test
{
    public class GetSalesHandler_test : IntegrationTest
    {
        private IUnitOfWork unitOfWork;
        private GetSalesCommand getSalesCommand_WithSaleIds;
        private GetSalesCommand getSalesCommand_WithSalesSorting;
        private Establishment establishment;
        private Sale sale;


        public GetSalesHandler_test() : base(new List<ITestService> { DatabaseTestContainer.CreateAsync().Result })
        {
            //Inject services
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            createTestData();

            getSalesCommand_WithSaleIds = new GetSalesCommand
            {
                EstablishmentId = establishment.Id,
                SalesIds = establishment.Sales.Select(x => x.Id).ToList()
            };

            getSalesCommand_WithSalesSorting = new GetSalesCommand
            {
                EstablishmentId = establishment.Id,
                SalesSorting = new SalesSorting(withinTimeperiods: new List<(DateTime start, DateTime end)> { (DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1)) })
            };
        }

        private void createTestData()
        {
            establishment = new Establishment("Test establishment");
            Item item = establishment.CreateItem("Test item", 0.0);
            establishment.AddItem(item);
            sale = establishment.CreateSale(DateTime.Now, itemAndQuantity: new List<(Item, int)> { (item, 1) });
            establishment.AddSale(sale);

            using (var uow = unitOfWork)
            {
                uow.establishmentRepository.Add(establishment);
            }
        }

        [Fact]
        public async Task GetSales_WithSalesIds_WithGetSalesReturn_ShouldReturnIds()
        {
            //Arrange
            IHandler<GetSalesCommand, GetSalesReturn> handler = scope.ServiceProvider.GetRequiredService<IHandler<GetSalesCommand, GetSalesReturn>>();

            //Act
            GetSalesReturn result = await handler.Handle(getSalesCommand_WithSaleIds);

            //Assert
            Assert.IsType<Guid>(result.Sales.First());
            Assert.Equal(sale.Id, result.Sales.First());
        }

        [Fact]
        public async Task GetSales_WithSalesIds_WithGetSalesRawReturn_ShouldReturnEntity()
        {
            //Arrange
            IHandler<GetSalesCommand, GetSalesRawReturn> handler = scope.ServiceProvider.GetRequiredService<IHandler<GetSalesCommand, GetSalesRawReturn>>();

            //Act
            GetSalesRawReturn result = await handler.Handle(getSalesCommand_WithSaleIds);

            //Assert
            Assert.IsType<Sale>(result.Sales.First());
            Assert.Equal(sale.Id, result.Sales.First().Id);
        }


        [Fact]
        public async Task GetSales_WithSalesIds_WithGetSalesDTOReturn_ShouldReturnDTO()
        {
            //Arrange
            IHandler<GetSalesCommand, GetSalesDTOReturn> handler = scope.ServiceProvider.GetRequiredService<IHandler<GetSalesCommand, GetSalesDTOReturn>>();

            //Act
            GetSalesDTOReturn result = await handler.Handle(getSalesCommand_WithSaleIds);

            //Assert
            Assert.IsType<SaleDTO>(result.Sales.First());
            Assert.Equal(sale.Id, result.Sales.First().id);
        }

        [Fact]
        public async Task GetSales_WithSalesSorting_WithGetSalesReturn_ShouldReturnIds()
        {
            //Arrange
            IHandler<GetSalesCommand, GetSalesReturn> handler = scope.ServiceProvider.GetRequiredService<IHandler<GetSalesCommand, GetSalesReturn>>();

            //Act
            GetSalesReturn result = await handler.Handle(getSalesCommand_WithSalesSorting);

            //Assert
            Assert.IsType<Guid>(result.Sales.First());
            Assert.Equal(sale.Id, result.Sales.First());
        }
    }

}