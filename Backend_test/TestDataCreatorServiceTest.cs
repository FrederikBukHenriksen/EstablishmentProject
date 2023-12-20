using EstablishmentProject.test;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain_Layer.Entities.Establishment;
using WebApplication1.Domain_Layer.Services.Entity_builders;
using WebApplication1.Infrastructure.Data;
using WebApplication1.Utils;

namespace EstablishmentProject.test
{
    public class TestDataCreatorServiceTest : BaseIntegrationTest
    {
        private ITestDataCreatorService testDataCreatorService;
        private IFactoryServiceBuilder factoryServiceBuilder;


        public TestDataCreatorServiceTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            this.testDataCreatorService = this.scope.ServiceProvider.GetRequiredService<ITestDataCreatorService>();
            this.factoryServiceBuilder = this.scope.ServiceProvider.GetRequiredService<IFactoryServiceBuilder>();
        }

        [Fact]
        public void CreateSimpleOpeningHours()
        {
            //Arrange
            var open = new LocalTime(8, 0);
            var close = new LocalTime(16, 0);

            //Act
            var openingHours = testDataCreatorService.CreateSimpleOpeningHoursForWeek(open: open, close: close);

            //Assert
            List<DayOfWeek> allDaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };

            Assert.Equal(7, openingHours.Count()); //Correct number of entries
            Assert.All(allDaysOfWeek, day => Assert.Contains(openingHours, item => item.dayOfWeek == day)); //One openingHours for each day of week
            Assert.All(openingHours, item => Assert.Equal(item.open, open)); //Correct open time
            Assert.All(openingHours, item => Assert.Equal(item.close, close)); //Correct close time
        }

        [Fact]
        public void GenerateHourlyDistibutionFromTimeline()
        {
            //Arrange
            List<DateTime> timeline = TimeHelper.CreateTimelineAsList(new DateTimePeriod(new DateTime(2021, 1, 1, 0, 0, 0), new DateTime(2021, 1, 7, 23, 0, 0)), TimeResolution.Hour);

            var DistributionFunction = TestDataCreatorService.GetLinearFuncition(1, 1);

            //Act
            var distribution = this.testDataCreatorService.GenerateDistributionFromTimeline(timeline, x => x.Hour, DistributionFunction);

            //Assert
            Assert.Equal(24 * 7, distribution.Count()); //Correct number of entries

            Assert.True(distribution.Keys.All(x => timeline.Contains(x))); //Coccrect dateTime objects

            int expectedSum = 24 / 2 * (1 + 24); //Sum of an arithmetic series
            Assert.Equal(expectedSum * 7, distribution.Sum(x => x.Value)); //Correct sum of distribution values
        }

        //[Fact]
        //public void GenerateDatelyDistibutionFromTimeline()
        //{
        //    //Arrange
        //    List<DateTime> timeline = TimeHelper.CreateTimelineAsList(new DateTimePeriod(new DateTime(2021, 1, 1, 0, 0, 0), new DateTime(2021, 1, 31, 23, 0, 0)), TimeResolution.Hour);

        //    var DistributionFunction = TestDataCreatorService.GetLinearFuncition(1, 0);

        //    //Act
        //    var distribution = this.testDataCreatorService.GenerateDistributionFromTimeline(timeline, x => x.Day, DistributionFunction);

        //    //Assert
        //    Assert.Equal(31 * 24, distribution.Count()); //Correct number of entries

        //    Assert.True(distribution.Keys.All(x => timeline.Contains(x))); //Coccrect dateTime objects

        //    var sum = distribution.Select(x => x.Value).ToList();

        //    int expectedSum = 31 / 2 * (1*24 + 31*24); //Sum of an arithmetic series
        //    Assert.Equal(expectedSum, distribution.Sum(x => x.Value)); //Correct sum of distribution values
        //}

        [Fact]
        public void SalesDistributionFromDistribution()
        {
            //Arrange
            Dictionary<DateTime, int> distribution = new Dictionary<DateTime, int>
        {
            { new DateTime(2021, 1, 1,8,0,0), 1 },
            { new DateTime(2021, 1, 1,9,0,0), 2 },
            { new DateTime(2021, 1, 1,10,0,0), 3 },
            { new DateTime(2021, 1, 1,11,0,0), 4 },
            { new DateTime(2021, 1, 1,12,0,0), 5 },
            { new DateTime(2021, 1, 1,13,0,0), 4 },
            { new DateTime(2021, 1, 1,14,0,0), 3 },
            { new DateTime(2021, 1, 1,15,0,0), 2 },
            { new DateTime(2021, 1, 1,16,0,0), 1 },

            { new DateTime(2021, 1, 2,8,0,0), 1 },
            { new DateTime(2021, 1, 2,9,0,0), 2 },
            { new DateTime(2021, 1, 2,10,0,0), 3 },
            { new DateTime(2021, 1, 2,11,0,0), 4 },
            { new DateTime(2021, 1, 2,12,0,0), 5 },
            { new DateTime(2021, 1, 2,13,0,0), 4 },
            { new DateTime(2021, 1, 2,14,0,0), 3 },
            { new DateTime(2021, 1, 2,15,0,0), 2 },
            { new DateTime(2021, 1, 2,16,0,0), 1 },
        };

            List<(Item, int)> soldItems = new List<(Item, int)> {
                (this.factoryServiceBuilder.ItemBuilder().WithName("Coffee").WithPrice(25).Build(), 1),
                (this.factoryServiceBuilder.ItemBuilder().WithName("Bun").WithPrice(50).Build(), 1)
            };

            //Act
            List<Sale> sales = this.testDataCreatorService.SaleGenerator(soldItems, distribution);

            //Assert
            Assert.Equal((1 + 2 + 3 + 4 + 5 + 4 + 3 + 2 + 1) * 2, sales.Count()); //Correct number of entries

            IEnumerable<IGrouping<DateTime, Sale>> groupedBySaleTime = sales.GroupBy(x => x.GetTimeOfSale());
            List<int> salesPerHour = groupedBySaleTime.Select(group => group.Count()).ToList();
            Assert.Equal(18, salesPerHour.Count());
            Assert.True(distribution.All(kv => groupedBySaleTime.Any(group => group.Key == kv.Key ))); //DateTime in sales distribution match used distribution
            Assert.True(distribution.All(kv => groupedBySaleTime.Any(group => group.Count() == kv.Value))); //Number of sales per dateTime match used distribution
        }

        [Fact]
        public void OpeningHoursFilter()
        {
            //Arrange
            List<DateTime> fullWeekTimeline = TimeHelper.CreateTimelineAsList(new DateTimePeriod(start: new DateTime(2021,1,1), end: new DateTime(2021, 1, 7)),TimeResolution.Hour);

            List<OpeningHours> openingHours = new List<OpeningHours>
            {
                new OpeningHours(dayOfWeek: DayOfWeek.Monday, open: new LocalTime(8,0), close: new LocalTime(16,0) ),
                new OpeningHours(dayOfWeek: DayOfWeek.Tuesday, open: new LocalTime(8,0), close: new LocalTime(16,0) ),
                new OpeningHours(dayOfWeek: DayOfWeek.Wednesday, open: new LocalTime(8,0), close: new LocalTime(16,0) ),
                new OpeningHours(dayOfWeek: DayOfWeek.Thursday, open: new LocalTime(8,0), close: new LocalTime(16,0) ),
                new OpeningHours(dayOfWeek: DayOfWeek.Friday, open: new LocalTime(8,0), close: new LocalTime(18,0) ),
                new OpeningHours(dayOfWeek: DayOfWeek.Saturday, open: new LocalTime(10,0), close: new LocalTime(22,0) ),
                new OpeningHours(dayOfWeek: DayOfWeek.Sunday, open: new LocalTime(10,0), close: new LocalTime(14,0) ),
            };

            //Act
            List<DateTime> timelineOpeningHours = this.testDataCreatorService.FilterDistrubutionBasedOnOpeningHours(fullWeekTimeline, openingHours);

            //Assert
            Dictionary<DayOfWeek,List<DateTime>> groupedByOpenHours = timelineOpeningHours.GroupBy(x => x.DayOfWeek).ToDictionary(x => x.Key, x => x.ToList());
            foreach (KeyValuePair<DayOfWeek, List<DateTime>> entry in groupedByOpenHours)
            {
                var resultDayOfWeek = entry.Key;
                var resultListOfOpenHours = entry.Value;
                LocalDate resultLocalDate = new LocalDate(2021, 1, resultListOfOpenHours.First().Day);

                OpeningHours? testDataOpeningHoursForGivenDay = openingHours.Find(x => x.dayOfWeek == resultDayOfWeek);
                DateTime testDataOpen = TimeHelper.LocalDateAndLocalTimeToDateTime(localDate: resultLocalDate, localTime: testDataOpeningHoursForGivenDay.open);
                DateTime testDataClose = TimeHelper.LocalDateAndLocalTimeToDateTime(localDate: resultLocalDate, localTime: testDataOpeningHoursForGivenDay.close);

                Assert.All(resultListOfOpenHours, x => TimeHelper.IsTimeWithinPeriod_EndNotIncluded<DateTime>(x, testDataOpen, testDataClose));
            }


        }
        [Fact]
        public void aggregateDistributions()
        {
            //Arrange
            var distribution1 = new Dictionary<DateTime, int>
            {
                { new DateTime(2021, 1, 1,8,0,0), 1 },
                { new DateTime(2021, 1, 1,9,0,0), 2 },
                { new DateTime(2021, 1, 1,10,0,0), 3 },
                { new DateTime(2021, 1, 1,11,0,0), 4 },
                { new DateTime(2021, 1, 1,12,0,0), 5 },
                { new DateTime(2021, 1, 1,13,0,0), 4 },
                { new DateTime(2021, 1, 1,14,0,0), 3 },
                { new DateTime(2021, 1, 1,15,0,0), 2 },
                { new DateTime(2021, 1, 1,16,0,0), 1 },
            };

            var distribution2 = new Dictionary<DateTime, int>
            {
                { new DateTime(2021, 1, 1,8,0,0), 1 },
                { new DateTime(2021, 1, 1,9,0,0), 1 },
                { new DateTime(2021, 1, 1,10,0,0), 1 },
                { new DateTime(2021, 1, 1,11,0,0), 1 },
                { new DateTime(2021, 1, 1,12,0,0), 1 },
                { new DateTime(2021, 1, 1,13,0,0), 1 },
                { new DateTime(2021, 1, 1,14,0,0), 1 },
                { new DateTime(2021, 1, 1,15,0,0), 1 },
                { new DateTime(2021, 1, 1,16,0,0), 1 },
            };

            var distribution3 = new Dictionary<DateTime, int>
            {
                { new DateTime(2021, 1, 1,8,0,0), 3 },
                { new DateTime(2021, 1, 1,9,0,0), 2 },
                { new DateTime(2021, 1, 1,10,0,0), 2 },
                { new DateTime(2021, 1, 1,11,0,0), 1 },
                { new DateTime(2021, 1, 1,12,0,0), 1 },
                { new DateTime(2021, 1, 1,13,0,0), 1 },
                { new DateTime(2021, 1, 1,14,0,0), 2 },
                { new DateTime(2021, 1, 1,15,0,0), 2 },
                { new DateTime(2021, 1, 1,16,0,0), 3 },
            };

            //Act
            Dictionary<DateTime, int> aggregatedDistribution = this.testDataCreatorService.AggregateDistributions(new List<Dictionary<DateTime, int>> { distribution1, distribution2, distribution3 });

            //Assert
            var expectedValues = new Dictionary<DateTime, int>
            {
                { new DateTime(2021, 1, 1, 8, 0, 0), 5 },
                { new DateTime(2021, 1, 1, 9, 0, 0), 5 },
                { new DateTime(2021, 1, 1, 10, 0, 0), 6 },
                { new DateTime(2021, 1, 1, 11, 0, 0), 6 },
                { new DateTime(2021, 1, 1, 12, 0, 0), 7 },
                { new DateTime(2021, 1, 1, 13, 0, 0), 6 },
                { new DateTime(2021, 1, 1, 14, 0, 0), 6 },
                { new DateTime(2021, 1, 1, 15, 0, 0), 5 },
                { new DateTime(2021, 1, 1, 16, 0, 0), 5 },
            };

            Assert.Equal(9, aggregatedDistribution.Count()); //Correct number of entries

            Assert.Equal(expectedValues, aggregatedDistribution); //The entires in the distribuiton match the expected output

            Assert.Empty(aggregatedDistribution.Except(expectedValues)); //No other entries are in distribution
        }

    }
}
