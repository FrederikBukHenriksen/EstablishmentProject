using WebApplication1.Infrastructure.Data;
using WebApplication1.Services.Analysis;

namespace EstablishmentProject.test.Algortihm
{
    public class CrossCorrelationTest : BaseIntegrationTest
    {

        private List<(DateTime, double)> normalDistributionTestData = new List<(DateTime, double)>();
        private Func<double, double> normalDistribution;

        public CrossCorrelationTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            normalDistribution = TestDataCreatorService.GetNormalFunction(12, 2);
            for (int i = 8; i <= 16; i++)
            {
                normalDistributionTestData.Add((DateTime.Today.AddHours(i), normalDistribution(i)));
            }
        }
        public void Analysis_WithPerfectData()
        {
            // Arrange
            var list1 = new List<(DateTime, double)>(normalDistributionTestData);
            var list2 = new List<(DateTime, double)>(normalDistributionTestData);

            // Act
            var result = CrossCorrelation.DoAnalysis(list1, list2);

            // Assert
            Assert.Single(result);
            Assert.Equal(TimeSpan.Zero, result.First().Item1);
            Assert.Equal(1.0, result.First().Item2);
        }

        public void Analysis_WithPerfectData_WithLag()
        {
            // Arrange
            var list1 = new List<(DateTime, double)>(normalDistributionTestData);
            var list2 = new List<(DateTime, double)>(normalDistributionTestData)
            {
                (DateTime.Today.AddHours(7), normalDistribution(7)),
                (DateTime.Today.AddHours(17), normalDistribution(17))

            };

            // Act
            var result = CrossCorrelation.DoAnalysis(list1, list2);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(1.0, result[1].Item2);
            Assert.Equal(1.0, result[1].Item2);
        }


        [Fact]
        public void Analysis_WithPerfectConstantOffsetData()
        {
            var list1 = new List<(DateTime, double)>(normalDistributionTestData);
            var list2 = new List<(DateTime, double)>(normalDistributionTestData);
            list2 = list2.Select(x => (x.Item1, x.Item2 + 1)).ToList();

            // Act
            var result = CrossCorrelation.DoAnalysis(list1, list2);

            // Assert
            Assert.Single(result);
            Assert.Equal(TimeSpan.Zero, result[0].Item1);
            Assert.Equal(1.0, result[0].Item2);
        }

        [Fact]
        public void Analysis_WithPerfectWithScalingFactorOffset()
        {
            var list1 = new List<(DateTime, double)>(normalDistributionTestData);
            var list2 = new List<(DateTime, double)>(normalDistributionTestData);
            list2 = list2.Select(x => (x.Item1, x.Item2 * 0.5)).ToList();

            // Act
            var result = CrossCorrelation.DoAnalysis(list1, list2);

            // Assert
            Assert.Single(result);
            Assert.Equal(TimeSpan.Zero, result[0].Item1);
            Assert.Equal(1.0, result[0].Item2);
        }

        [Fact]
        public void Analysis_WithPerfectInvertedData()
        {
            var list1 = new List<(DateTime, double)>(normalDistributionTestData);
            var list2 = new List<(DateTime, double)>(normalDistributionTestData);
            list2 = list2.Select(x => (x.Item1, x.Item2 * (-1))).ToList();

            // Act
            var result = CrossCorrelation.DoAnalysis(list1, list2);

            // Assert
            Assert.Single(result);
            Assert.Equal(TimeSpan.Zero, result[0].Item1);
            Assert.Equal(-1.0, result[0].Item2);
        }
        [Fact]
        public void Analysis_WithNormalAndSinusDistribution()
        {
            var list1 = new List<(DateTime, double)>(normalDistributionTestData);
            Func<double, double> cosineFunction = TestDataCreatorService.GetCosineFunction(1, 12, 2, 1);
            var list2 = new List<(DateTime, double)>();
            for (int i = 8; i <= 16; i++)
            {
                list2.Add((DateTime.Today.AddHours(i), cosineFunction(i)));
            }

            // Act
            var result = CrossCorrelation.DoAnalysis(list1, list2);

            // Assert
            Assert.Single(result);
            Assert.Equal(TimeSpan.Zero, result[0].Item1); // Inverse data should have zero lag
            Assert.Equal(1.0, result[0].Item2, 5); // Adjust the precision if needed
        }


        [Fact]
        public void Analysis_InputEmptyList()
        {
            var list1 = new List<(DateTime, double)>();
            var list2 = new List<(DateTime, double)>(normalDistributionTestData);

            // Act
            var act1 = () => CrossCorrelation.DoAnalysis(list1, list2);
            var act2 = () => CrossCorrelation.DoAnalysis(list2, list1);


            // Assert
            Assert.Throws<ArgumentException>(act1);
            Assert.Throws<ArgumentException>(act2);
        }
    }

}
