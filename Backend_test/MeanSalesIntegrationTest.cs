//using Microsoft.Extensions.DependencyInjection;
//using NodaTime;
//using WebApplication1.CommandHandlers;
//using WebApplication1.Domain_Layer.Entities;
//using WebApplication1.Domain_Layer.Services.Entity_builders;
//using WebApplication1.Infrastructure.Data;
//using WebApplication1.Utils;

//namespace EstablishmentProject.test.CommandHandlers.MeanSales
//{
//    public class MeanSalesIntegrationTest : BaseIntegrationTest
//    {

//        private ITestDataCreatorService testDataCreatorService;
//        private IFactoryServiceBuilder factoryServiceBuilder;
//        public MeanSalesIntegrationTest(IntegrationTestWebAppFactory factory) : base(factory)
//        {
//            //Services
//            testDataCreatorService = scope.ServiceProvider.GetRequiredService<ITestDataCreatorService>();
//            factoryServiceBuilder = scope.ServiceProvider.GetRequiredService<IFactoryServiceBuilder>();

//            //Arrange
//            var totalDataTimePeriod = new DateTimePeriod(new DateTime(2021, 1, 1, 0, 0, 0), new DateTime(2021, 12, 30, 23, 0, 0)); //Leave out last day of the year to test null values
//            var timelineAllDays = TimeHelper.CreateTimelineAsList(totalDataTimePeriod, TimeResolution.Hour);
//            var openingHours = testDataCreatorService.CreateSimpleOpeningHoursForWeek(new LocalTime(8, 0), new LocalTime(16, 0));
//            var timelineDaysWithinOpeningHours = testDataCreatorService.FilterDistrubutionBasedOnOpeningHours(timelineAllDays, openingHours);

//            Dictionary<DateTime, int> distributionHourly = testDataCreatorService.GenerateDistributionFromTimeline(timelineDaysWithinOpeningHours, x => x.Hour, TestDataCreatorService.GetLinearFuncition(0, 1));
//            Dictionary<DateTime, int> distributionDaily = testDataCreatorService.GenerateDistributionFromTimeline(timelineDaysWithinOpeningHours, x => x.Day, TestDataCreatorService.GetLinearFuncition(0, 0));
//            Dictionary<DateTime, int> distributionMonthly = testDataCreatorService.GenerateDistributionFromTimeline(timelineDaysWithinOpeningHours, x => x.Month, TestDataCreatorService.GetLinearFuncition(0, 0));

//            List<Dictionary<DateTime, int>> allDistributions = new List<Dictionary<DateTime, int>> { distributionHourly, distributionDaily, distributionMonthly };

//            bool doesDictionariesHaveTheSameKeys = allDistributions.All(x => x.Keys.SequenceEqual(allDistributions.First().Keys));

//            var aggregatedDistribution = testDataCreatorService.AggregateDistributions(allDistributions);

//            var water = factoryServiceBuilder.ItemBuilder().WithName("Water").WithPrice(10).Build();
//            var coffee = factoryServiceBuilder.ItemBuilder().WithName("Coffee").WithPrice(25).Build();
//            var bun = factoryServiceBuilder.ItemBuilder().WithName("Bun").WithPrice(50).Build();

//            var salesDistribution = testDataCreatorService.SaleGenerator(new List<(Item, int)> { (coffee, 1), (water, 1) }, aggregatedDistribution);


//            var establishment = factoryServiceBuilder
//                .EstablishmentBuilder()
//                .AddItems(new List<Item> { coffee, bun, water })
//                .AddSales(salesDistribution)
//                .Build();

//            dbContext.Add(establishment);
//            dbContext.SaveChanges();
//        }

//        [Fact]
//        public async void MeanSales_Success()
//        {
//            //Services
//            var generalHandler = scope.ServiceProvider.GetRequiredService<IHandler<SalesMeanOverTime, SalesMeanQueryReturn>>();

//            //Arrange
//            var timePeriod = new DateTimePeriod(new DateTime(2021, 1, 1, 0, 0, 0), new DateTime(2021, 12, 31, 23, 0, 0));

//            SalesMeanOverTimeAverageSpend command = new SalesMeanOverTimeAverageSpend
//            {
//                salesSortingParameters = new SalesSorting { UseDataFromTimeframePeriods = new List<DateTimePeriod> { timePeriod } },
//                TimeResolution = TimeResolution.Month
//            };

//            //Act
//            SalesMeanQueryReturn res = await generalHandler.Handle(command);

//            //Assert

//        }

//        [Fact]
//        public async void NumberOfSales()
//        {
//            //Arrange
//            SalesMeanOverTime command = new SalesMeanOverTimeAverageNumberOfSales { TimeResolution = TimeResolution.Date };
//            var handler = scope.ServiceProvider.GetRequiredService<IHandler<SalesMeanOverTime, SalesMeanQueryReturn>>();
//            //Act
//            SalesMeanQueryReturn res = await handler.Handle(command);

//            //Assert
//            var todayTimeIdentifier = DateTime.UtcNow.PlainIdentifierBasedOnTimeResolution(command.TimeResolution);
//            //double valueOfToday = (double)res.Data.Find(x => x.Item1 == todayTimeIdentifier).Item2!;
//            //Assert.Equal(2, valueOfToday);
//        }
//    }
//}
