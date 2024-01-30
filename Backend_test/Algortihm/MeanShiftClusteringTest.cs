using WebApplication1.Services.Analysis;

namespace EstablishmentProject.test.Algortihm
{
    public class MeanShiftClusteringTest : BaseIntegrationTest
    {
        public MeanShiftClusteringTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

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

            var bandwidth = new List<double> { 0.2, 0.2 };

            // Act
            var result = MeanShiftClustering.Cluster(data, bandwidth);

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
            var result = MeanShiftClustering.Cluster(emptyData, bandwidth);

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
            var result = MeanShiftClustering.Cluster(data, bandwidth);

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
            var result = MeanShiftClustering.Cluster(data, largeBandwidth);

            // Assert
            Assert.Single(result);
            Assert.Contains(result, cluster => cluster.Contains("A") && cluster.Contains("B") && cluster.Contains("C"));
        }
    }
}
