using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Handlers.SalesHandlers;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Application_Test.Handlers_Test.Entities_Test
{
    public class GetSalesStatisticsHandler_Test : IntegrationTest
    {
        private IUnitOfWork unitOfWork;
        private Establishment establishment;
        private IHandler<GetSalesStatisticsCommand, GetSalesStatisticsReturn> handler;

        public GetSalesStatisticsHandler_Test() : base(new List<ITestService> { DatabaseTestContainer.CreateAsync().Result })
        {
            //Inject services
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            CommonArrange();
            handler = scope.ServiceProvider.GetRequiredService<IHandler<GetSalesStatisticsCommand, GetSalesStatisticsReturn>>();
        }

        private void CommonArrange()
        {
            establishment = new Establishment("Test establishment");
            Item coffee = establishment.CreateItem("coffee", 10.0);
            establishment.AddItem(coffee);
            var sale1 = establishment.CreateSale(DateTime.Now);
            establishment.AddSale(sale1);

            establishment.AddSalesItems(sale1, establishment.CreateSalesItem(sale1, coffee, 1));
            sale1.setTimeOfArrival(DateTime.Now.AddDays(-3));

            var sale2 = establishment.CreateSale(DateTime.Now);
            establishment.AddSale(sale2);
            establishment.AddSalesItems(sale2, establishment.CreateSalesItem(sale2, coffee, 2));
            sale2.setTimeOfArrival(DateTime.Now.AddDays(-2));

            var sale3 = establishment.CreateSale(DateTime.Now);
            establishment.AddSale(sale3);
            establishment.AddSalesItems(sale3, establishment.CreateSalesItem(sale3, coffee, 3));
            sale3.setTimeOfArrival(DateTime.Now.AddDays(-1));

            using (var uow = unitOfWork)
            {
                uow.establishmentRepository.Add(establishment);
            }
        }

        [Fact]
        public async Task GetSalesAverageSpend_ShouldReturnAverageSpend()
        {
            //Arrange
            GetSalesAverageSpend getSalesAverageSpend = new GetSalesAverageSpend
            {
                EstablishmentId = establishment.Id,
                SalesIds = establishment.Sales.Select(x => x.Id).ToList()
            };
            //IHandler<GetSalesAverageSpend, GetSalesStatisticsReturn> handler = scope.ServiceProvider.GetRequiredService<IHandler<GetSalesAverageSpend, GetSalesStatisticsReturn>>();

            //Act
            GetSalesStatisticsReturn result = await handler.Handle(getSalesAverageSpend);

            //Assert
            Assert.Equal(20, result.metric);
        }

        [Fact]
        public async Task GetSalesAverageNumberOfItems_ShouldReturnAverageNumberOfItems()
        {
            //Arrange
            GetSalesAverageNumberOfItems getSalesAverageNumberOfItems = new GetSalesAverageNumberOfItems
            {
                EstablishmentId = establishment.Id,
                SalesIds = establishment.Sales.Select(x => x.Id).ToList()
            };
            //IHandler<GetSalesAverageNumberOfItems, GetSalesStatisticsReturn> handler = scope.ServiceProvider.GetRequiredService<IHandler<GetSalesAverageNumberOfItems, GetSalesStatisticsReturn>>();

            //Act
            GetSalesStatisticsReturn result = await handler.Handle(getSalesAverageNumberOfItems);

            //Assert
            Assert.Equal(2, result.metric);
        }

        [Fact]
        public async Task GetSalesAverageTimeOfArrival_ShouldReturnAverageTimeOfArrival()
        {
            //Arrange
            GetSalesAverageTimeOfArrival getSalesAverageTimeOfArrival = new GetSalesAverageTimeOfArrival
            {
                EstablishmentId = establishment.Id,
                SalesIds = establishment.Sales.Select(x => x.Id).ToList()
            };
            //IHandler<GetSalesAverageTimeOfArrival, GetSalesStatisticsReturn> handler = scope.ServiceProvider.GetRequiredService<IHandler<GetSalesAverageTimeOfArrival, GetSalesStatisticsReturn>>();

            //Act
            GetSalesStatisticsReturn result = await handler.Handle(getSalesAverageTimeOfArrival);

            //Assert
            Assert.Equal(DateTime.Now.AddDays(-2).TimeOfDay.TotalMinutes, result.metric, 0.01);
        }

        [Fact]
        public async Task GetSalesAverageTimeOfPayment_ShouldReturnAverageTimeOfPayment()
        {
            //Arrange
            GetSalesAverageTimeOfPayment getSalesAverageTimeOfPayment = new GetSalesAverageTimeOfPayment
            {
                EstablishmentId = establishment.Id,
                SalesIds = establishment.Sales.Select(x => x.Id).ToList()
            };
            //IHandler<GetSalesAverageTimeOfPayment, GetSalesStatisticsReturn> handler = scope.ServiceProvider.GetRequiredService<IHandler<GetSalesAverageTimeOfPayment, GetSalesStatisticsReturn>>();

            //Act
            GetSalesStatisticsReturn result = await handler.Handle(getSalesAverageTimeOfPayment);

            //Assert
            Assert.Equal(DateTime.Now.TimeOfDay.TotalMinutes, result.metric, 1);
        }

        [Fact]
        public async Task GetSalesAverageSeatTime_ShouldReturnAverageSeatTime()
        {
            //Arrange
            GetSalesAverageSeatTime getSalesAverageSeatTime = new GetSalesAverageSeatTime
            {
                EstablishmentId = establishment.Id,
                SalesIds = establishment.Sales.Select(x => x.Id).ToList()
            };
            //IHandler<GetSalesAverageSeatTime, GetSalesStatisticsReturn> handler = scope.ServiceProvider.GetRequiredService<IHandler<GetSalesAverageSeatTime, GetSalesStatisticsReturn>>();

            //Act
            GetSalesStatisticsReturn result = await handler.Handle(getSalesAverageSeatTime);

            //Assert
            Assert.Equal(24 * 60 * 2, result.metric, 0.0001);
        }

    }
}
