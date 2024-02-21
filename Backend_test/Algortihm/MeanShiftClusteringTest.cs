using WebApplication1.Infrastructure.Data;
using WebApplication1.Services.Analysis;

namespace EstablishmentProject.test.Algortihm
{
    public class MeanShiftClusteringTest
    {

        [Fact]
        public void Cluster_ReturnsCorrectClusters()
        {
            // Arrange
            var data = new List<(string, List<double>)>
            {
                ("A", new List<double> { 1.0, 2.0 }),
                ("B", new List<double> { 1.1, 2.1 }),
                ("C", new List<double> { 5.0, 6.0 }),
                ("D", new List<double> { 5.1, 6.1 }),
                ("E", new List<double> { 10.0, 12.0 }),
                ("F", new List<double> { 10.1, 12.1 })
            };

            var bandwidth = new List<double> { 1.0, 1.0 };

            // Act

            var result = new MeanShiftClusteringStepByStep().Cluster(data, bandwidth);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(result, cluster => cluster.Contains("A") && cluster.Contains("B"));
            Assert.Contains(result, cluster => cluster.Contains("C") && cluster.Contains("D"));
            Assert.Contains(result, cluster => cluster.Contains("E") && cluster.Contains("F"));
        }

        [Fact]
        public void Cluster_EmptyInput_ReturnsEmptyClusters()
        {
            // Arrange
            var emptyData = new List<(string, List<double>)>();
            var bandwidth = new List<double> { 0.2, 0.2 };

            // Act
            var result = new MeanShiftClusteringStepByStep().Cluster(emptyData, bandwidth);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Cluster_SingleClusterScenario_ReturnsSingleCluster()
        {
            // Arrange
            var data = new List<(string, List<double>)>
            {
                ("A", new List<double> { 1.0, 2.0 }),
                ("B", new List<double> { 1.1, 2.1 }),
                ("C", new List<double> { 1.2, 2.2 }),
            };

            var bandwidth = new List<double> { 0.5, 0.5 };

            // Act
            var result = new MeanShiftClusteringStepByStep().Cluster(data, bandwidth);

            // Assert
            Assert.Single(result);
            Assert.Contains(result, cluster => cluster.Contains("A") && cluster.Contains("B") && cluster.Contains("C"));
        }

        [Fact]
        public void Cluster_LargeBandwidth_ReturnsSingleCluster()
        {
            // Arrange
            var data = new List<(string, List<double>)>
            {
                ("A", new List<double> { 1.0, 2.0 }),
                ("B", new List<double> { 5.0, 6.0 }),
                ("C", new List<double> { 10.0, 12.0 }),
            };

            var largeBandwidth = new List<double> { 15.0, 15.0 };

            // Act
            var result = new MeanShiftClusteringStepByStep().Cluster(data, largeBandwidth);

            // Assert
            Assert.Single(result);
            Assert.Contains(result, cluster => cluster.Contains("A") && cluster.Contains("B") && cluster.Contains("C"));
        }

        [Fact]
        public void Cluster_WithTwoVeryDifferentBandwidths_ShoulReturnTwoClusters()
        {

        }

        [Fact]
        public void Cluster_With3DNormalCenteredMass_WithStepByStepApproach_ShouldReturnASingleCluster()
        {
            // Arrange
            var number = 100;
            Func<double, double> normFunc = TestDataCreatorService.GetNormalFunction(0, 10);
            Random random = new Random(1);

            var data = new List<(string, List<double>)>();
            for (int i = 0; i < number; i++)
            {
                //Random double between -10 and 10.
                var x = normFunc(random.NextDouble() * 20.0 - 10.0);
                var y = normFunc(random.NextDouble() * 20.0 - 10.0);
                var z = normFunc(random.NextDouble() * 20.0 - 10.0);

                data.Add(("point", new List<double> { x, y, z }));
            };

            var bandwidth = new List<double> { 5, 5, 5 };

            // Act
            var result = new MeanShiftClusteringStepByStep().Cluster(data, bandwidth);

            // Assert
            Assert.Single(result);
            Assert.Equal(number, result[0].Count);
        }

        [Fact]
        public void Cluster_With3DNormalCenteredMass_WithDirectApproach_ShouldReturnASingleCluster()
        {
            // Arrange
            var number = 100;
            Func<double, double> normFunc = TestDataCreatorService.GetNormalFunction(0, 10);
            Random random = new Random(1);

            var data = new List<(string, List<double>)>();
            for (int i = 0; i < number; i++)
            {
                //Random double between -10 and 10.
                var x = normFunc(random.NextDouble() * 20.0 - 10.0);
                var y = normFunc(random.NextDouble() * 20.0 - 10.0);
                var z = normFunc(random.NextDouble() * 20.0 - 10.0);
                data.Add(("point", new List<double> { x, y, z }));
            };

            var bandwidth = new List<double> { 5, 5, 5 };

            // Act
            var result = new MeanShiftClusteringDirectly().Cluster(data, bandwidth);

            // Assert
            Assert.Single(result);
            Assert.Equal(number, result[0].Count);
        }

        [Fact]
        public void Cluster_With2OverlappingNormal_WithDirectApproach_ShouldReturnTwoClusters()
        {
            // Arrange
            var number = 100;
            Func<double, double> normFunc = TestDataCreatorService.GetNormalFunction(0, 2);
            Random random = new Random(1);

            var data = new List<(string, List<double>)>();
            for (int i = 0; i < number; i++)
            {
                //Random double between -10 and 10.
                var x = normFunc(random.NextDouble() * 20.0 - 10.0);
                var y = normFunc(random.NextDouble() * 20.0 - 10.0);
                var point = new List<double> { x, y, };
                if (i < number / 2)
                {
                    point = point.Zip(new List<double> { 5, 5 }, (m, s) => m + s).ToList();
                }
                else
                {
                    point = point.Zip(new List<double> { -5, -5 }, (m, s) => m + s).ToList();
                }
                data.Add(("point", point));
            };

            var bandwidth = new List<double> { 1, 1 };

            // Act
            var result = new MeanShiftClusteringDirectly().Cluster(data, bandwidth);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Cluster_With2OverlappingNormal_WithStepByStepApproach_ShouldReturnTwoClusters()
        {
            // Arrange
            var number = 100;
            Func<double, double> normFunc = TestDataCreatorService.GetNormalFunction(0, 2);
            Random random = new Random(1);

            var data = new List<(string, List<double>)>();
            for (int i = 0; i < number; i++)
            {
                //Random double between -10 and 10.
                var x = normFunc(random.NextDouble() * 20.0 - 10.0);
                var y = normFunc(random.NextDouble() * 20.0 - 10.0);
                var point = new List<double> { x, y, };
                if (i < number / 2)
                {
                    point = point.Zip(new List<double> { 5, 5 }, (m, s) => m + s).ToList();
                }
                else
                {
                    point = point.Zip(new List<double> { -5, -5 }, (m, s) => m + s).ToList();
                }
                data.Add(("point", point));
            };

            var bandwidth = new List<double> { 1, 1 };

            // Act
            var result = new MeanShiftClusteringDirectly().Cluster(data, bandwidth);

            // Assert
            Assert.Equal(2, result.Count);
        }


    }
}
