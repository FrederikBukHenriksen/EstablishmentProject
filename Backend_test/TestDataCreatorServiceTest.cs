using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure.Data;
using WebApplication1.Utils;

namespace EstablishmentProject.test
{
    public class TestDataCreatorServiceTest : BaseIntegrationTest
    {
        private ITestDataCreatorService testDataCreatorService;
        private Establishment establishment;

        public TestDataCreatorServiceTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            testDataCreatorService = scope.ServiceProvider.GetRequiredService<ITestDataCreatorService>();

            establishment = new Establishment("Cafe 1");
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
            List<DateTime> timeline = TimeHelper.CreateTimelineAsList(new DateTime(2021, 1, 1, 0, 0, 0), new DateTime(2021, 1, 8, 0, 0, 0), TimeResolution.Hour);

            var DistributionFunction = TestDataCreatorService.GetLinearFuncition(1, 1);

            //Act
            var distribution = testDataCreatorService.GenerateDistributionFromTimeline(timeline, x => x.Hour, DistributionFunction);

            //Assert
            Assert.Equal(24 * 7, distribution.Count()); //Correct number of entries

            Assert.True(distribution.Keys.All(x => timeline.Contains(x))); //Coccrect dateTime objects

            int expectedSum = 24 / 2 * (1 + 24); //Sum of an arithmetic series
            Assert.Equal(expectedSum * 7, distribution.Sum(x => x.Value)); //Correct sum of distribution values
        }

        [Fact]
        public void OpeningHoursFilter()
        {
            //Arrange
            List<DateTime> fullWeekTimeline = TimeHelper.CreateTimelineAsList(start: new DateTime(2021, 1, 1), end: new DateTime(2021, 1, 7), TimeResolution.Hour);

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
            List<DateTime> timelineOpeningHours = testDataCreatorService.SLETTES_DistrubutionBasedOnTimlineAndOpeningHours(fullWeekTimeline, openingHours);

            //Assert
            Dictionary<DayOfWeek, List<DateTime>> groupedByOpenHours = timelineOpeningHours.GroupBy(x => x.DayOfWeek).ToDictionary(x => x.Key, x => x.ToList());
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
            Dictionary<DateTime, int> aggregatedDistribution = testDataCreatorService.AggregateDistributions(new List<Dictionary<DateTime, int>> { distribution1, distribution2, distribution3 });

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
