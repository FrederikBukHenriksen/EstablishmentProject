using DMIOpenData;
using EstablishmentProject.test.TestingCode;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Handlers.Correlation;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure.Data;
using WebApplication1.Utils;

namespace EstablishmentProject.test.Application.Handlers.Correlation
{
    public class Correlation_NumberOfSales_Vs_Temperature : IntegrationTest
    {
        private IUnitOfWork unitOfWork;
        private IHandler<Correlation_NumberOfSales_Vs_Temperature_Command, CorrelationReturn> handler;
        private Establishment establishment;
        private Item testItem;


        public Correlation_NumberOfSales_Vs_Temperature() : base(new List<ITestService> { DatabaseTestContainer.CreateAsync().Result })
        {
            //Inject services
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            handler = scope.ServiceProvider.GetRequiredService<IHandler<Correlation_NumberOfSales_Vs_Temperature_Command, CorrelationReturn>>();

            establishment = new Establishment("Cafe 1");
            testItem = establishment.CreateItem("test", 1);
            establishment.AddItem(testItem);
            establishment = Correlation_NumberOfSales_Vs_Temperature_Helper.CreateTestData(establishment, testItem);

            using (var uow = unitOfWork)
            {
                uow.establishmentRepository.Add(establishment);
            }
        }

        [Fact]
        public async Task CrossCorrelation_WithActualWeatherDataForToday_ShouldReturnCorrelation()
        {
            //Arrange
            var command = new Correlation_NumberOfSales_Vs_Temperature_Command
            {
                EstablishmentId = establishment.Id,
                SalesIds = establishment.GetSales().Select(x => x.Id).ToList(),
                TimePeriod = new DateTimePeriod(DateTime.Today.AddDays(-1).AddHours(8), DateTime.Today.AddDays(-1).AddHours(16)),
                TimeResolution = TimeResolution.Hour,
                Coordinates = new Coordinates(55.6761, 12.5683),
                UpperLag = 3,
                LowerLag = 3
            };

            //Act
            CorrelationReturn result = await handler.Handle(command);

            //Assert
            Assert.Equal(7, result.LagAndCorrelation.Count);
            Assert.True(result.LagAndCorrelation.Any(x => x.Item2 > 0.5));
            Assert.Equal(8 + 6, result.calculationValues.Count);
        }
    }

    public class Correlation_NumberOfSales_Vs_Temperature_Test_WithMockWeather : IntegrationTest
    {
        private IUnitOfWork unitOfWork;
        private IHandler<Correlation_NumberOfSales_Vs_Temperature_Command, CorrelationReturn> handler;
        private Establishment establishment;
        private Item testItem;


        public Correlation_NumberOfSales_Vs_Temperature_Test_WithMockWeather() : base(new List<ITestService> { DatabaseTestContainer.CreateAsync().Result, new WeatherMock(Correlation_NumberOfSales_Vs_Temperature_Helper.testWeatherDataThatMatchSalesNumbers()) })
        {
            //Inject services
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            handler = scope.ServiceProvider.GetRequiredService<IHandler<Correlation_NumberOfSales_Vs_Temperature_Command, CorrelationReturn>>();

            establishment = new Establishment("Cafe 1");
            testItem = establishment.CreateItem("test", 1);
            establishment.AddItem(testItem);
            establishment = Correlation_NumberOfSales_Vs_Temperature_Helper.CreateTestData(establishment, testItem);

            using (var uow = unitOfWork)
            {
                uow.establishmentRepository.Add(establishment);
            }
        }


        [Fact]
        public async Task CrossCorrelation_WithTestWeatherData_ShouldReturnCorrelation()
        {
            //Arrange
            var command = new Correlation_NumberOfSales_Vs_Temperature_Command
            {
                EstablishmentId = establishment.Id,
                SalesIds = establishment.GetSales().Select(x => x.Id).ToList(),
                TimePeriod = new DateTimePeriod(DateTime.Today.AddDays(-1).AddHours(8), DateTime.Today.AddDays(-1).AddHours(16)),
                TimeResolution = TimeResolution.Hour,
                Coordinates = new Coordinates(55.6761, 12.5683),
                UpperLag = 2,
                LowerLag = 2
            };

            var weatherMock = (WeatherMock)testServices[1];

            //Act
            CorrelationReturn result = await handler.Handle(command);

            //Assert
            Assert.Equal(5, result.LagAndCorrelation.Count);
            Assert.Equal(12, result.calculationValues.Count);
            Assert.Equal(1, result.LagAndCorrelation[2].Item2);

        }
    }



    public static class Correlation_NumberOfSales_Vs_Temperature_Helper
    {
        public static Establishment CreateTestData(Establishment establishment, Item item)
        {
            var testDataBuilder = new TestDataBuilder();

            Func<double, double> linearFirstDistribution = TestDataBuilder.GetLinearFuncition(2, -8 * 2);
            Func<double, double> linearSecondDistribution = TestDataBuilder.GetLinearFuncition(-2, 32);

            var firstSalesDistribution = testDataBuilder.FINALgenerateDistrubution(DateTime.Today.AddDays(-1), DateTime.Today, linearFirstDistribution, TimeResolution.Hour);
            var firstSales = testDataBuilder.FINALFilterOnOpeningHours(8, 12, firstSalesDistribution);
            var secondSalesDistribution = testDataBuilder.FINALgenerateDistrubution(DateTime.Today.AddDays(-1), DateTime.Today, linearSecondDistribution, TimeResolution.Hour);
            var secondSales = testDataBuilder.FINALFilterOnOpeningHours(12, 16, secondSalesDistribution);

            var aggregate = testDataBuilder.FINALAggregateDistributions([firstSales, secondSales]);

            var normalRandomSeed = new SystemRandomSource(1);

            Normal normal = new Normal(0, 5, normalRandomSeed);
            foreach (var distribution in aggregate.ToList())
            {
                for (int i = 0; i < distribution.Value; i++)
                {
                    var randomNormalDistributionNumber = normal.RandomSource.Next(0, 100);
                    var sale = establishment.CreateSale(distribution.Key);
                    establishment.AddSale(sale);
                    var salesItems = establishment.CreateSalesItem(sale, item, randomNormalDistributionNumber);
                    establishment.AddSalesItems(sale, salesItems);
                }
            }
            return establishment;


        }

        public static List<(DateTime, double)> testWeatherDataThatMatchSalesNumbers()
        {
            List<(DateTime, double)> values = new List<(DateTime, double)>();

            for (int i = 6; i <= 12; i++)
            {
                values.Add((DateTime.Today.AddDays(-1).AddHours(i), i * 2));

            }
            for (int i = 13; i < 18; i++)
            {
                values.Add((DateTime.Today.AddDays(-1).AddHours(i), i * (-2) + (26) + 22));
            }


            return values.OrderBy(x => x.Item1).ToList();
        }




    }

}