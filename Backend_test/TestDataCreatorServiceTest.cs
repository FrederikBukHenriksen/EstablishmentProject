using WebApplication1.Infrastructure.Data;
using WebApplication1.Utils;

namespace EstablishmentProject.test
{
    public class TestDataCreatorServiceTest
    {
        private ITestDataBuilder testDataCreator;

        public TestDataCreatorServiceTest()
        {
            testDataCreator = new TestDataBuilder();
        }

        [Fact]
        public void GenerateHourlyDistibutionFromTimeline()
        {
            //Arrange
            DateTime start = new DateTime(2021, 1, 1, 0, 0, 0);
            DateTime end = new DateTime(2021, 1, 8, 0, 0, 0);
            var DistributionFunction = TestDataBuilder.GetLinearFuncition(1, 1);

            //Act
            //var distribution = testDataCreatorService.GenerateDistributionFromTimeline(timeline, x => x.Hour, DistributionFunction);
            var distribution = testDataCreator.generateDistrubution(start, end, DistributionFunction, TimeResolution.Hour);


            //Assert
            Assert.Equal(24 * 7, distribution.Count()); //Correct number of entries

            int expectedSum = 24 / 2 * (1 + 24); //Sum of an arithmetic series
            Assert.Equal(expectedSum * 7, distribution.Sum(x => x.Value)); //Correct sum of distribution values
        }

        [Fact]
        public void FINALFilterOnOpeningHours()
        {
            //Arrange
            var distribution = new Dictionary<DateTime, int>
            {
                { new DateTime(2021, 1, 1, 0, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 1, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 2, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 3, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 4, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 5, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 6, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 7, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 8, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 9, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 10, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 11, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 12, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 13, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 14, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 15, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 16, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 17, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 18, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 19, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 20, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 21, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 22, 0, 0), 0 },
                { new DateTime(2021, 1, 1, 23, 0, 0), 0 }
            };

            //Act
            var filteredDistribution = testDataCreator.FINALFilterOnOpeningHours(8, 16, distribution);

            //Assert
            Assert.Equal(8, filteredDistribution.Count()); //Correct number of entries
            Assert.All(filteredDistribution, kvp =>
            {
                var timeOfDayHours = kvp.Key.TimeOfDay.TotalHours;
                if (timeOfDayHours < 8 || timeOfDayHours >= 16)
                {
                    throw new Exception("Assertion failed");
                }
            });
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
            Dictionary<DateTime, int> aggregatedDistribution = testDataCreator.FINALAggregateDistributions(new List<Dictionary<DateTime, int>> { distribution1, distribution2, distribution3 });

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
