﻿using DMIOpenData;
using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Infrastructure.Data;
using WebApplication1.Utils;

namespace EstablishmentProject.test.Application.Handlers.Correlation
{
    public class CrossCorrelationHandlerTest : IntegrationTest
    {
        private IUnitOfWork unitOfWork;
        private IWeatherApi yourClassUnderTest;
        private ITestDataBuilder testDataCreatorService;


        public CrossCorrelationHandlerTest() : base(new List<ITestService> { DatabaseTestContainer.CreateAsync().Result, new WeatherMock() })
        {
            //Inject services
            testDataCreatorService = scope.ServiceProvider.GetRequiredService<ITestDataBuilder>();
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            yourClassUnderTest = scope.ServiceProvider.GetRequiredService<IWeatherApi>();
            Coordinates coordinates = new Coordinates(55.676098, 12.568337);
            var ok = yourClassUnderTest.GetMeanTemperature(coordinates, new DateTime(2021, 1, 1), new DateTime(2021, 1, 1), TimeResolution.Hour);



        }

        public void createTestData()
        {
            var calendar = testDataCreatorService.OpenHoursCalendar(
                new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0),
                new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0).AddDays(1).AddTicks(-1),
                TimeResolution.Hour,
                testDataCreatorService.CreateSimpleOpeningHoursForWeek(open: new LocalTime(8, 0), close: new LocalTime(16, 0)));

            Dictionary<DateTime, int> distributionHourly = testDataCreatorService.DistributionByTimeresolution(calendar, TestDataBuilder.GetLinearFuncition(1, -7), TimeResolution.Hour);
            Dictionary<DateTime, int> distributionDaily = testDataCreatorService.GenerateDistributionFromTimeline(calendar, x => x.Day, TestDataBuilder.GetLinearFuncition(0.5, 0));
            Dictionary<DateTime, int> distributionMonthly = testDataCreatorService.GenerateDistributionFromTimeline(calendar, x => x.Month, TestDataBuilder.GetLinearFuncition(0, 0));

            List<Dictionary<DateTime, int>> allDistributions = new List<Dictionary<DateTime, int>> { distributionHourly, distributionDaily, distributionMonthly };

            var aggregatedDistribution = testDataCreatorService.FINALAggregateDistributions(allDistributions);
        }








    }

}