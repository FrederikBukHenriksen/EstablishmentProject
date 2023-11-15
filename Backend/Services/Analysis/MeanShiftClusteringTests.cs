//using System.Collections.Generic;
//using Xunit;

//namespace WebApplication1.Services.Analysis.Tests
//{
//    public class MeanShiftClusteringTests
//    {
//        [Fact]
//        public void Cluster_ReturnsCorrectNumberOfClusters()
//        {
//            // Arrange
//            var clustering = new MeanShiftClustering();
//            var data = new List<List<double>>()
//            {
//                new List<double>() { 1, 2, 3 },
//                new List<double>() { 4, 5, 6 },
//                new List<double>() { 7, 8, 9 },
//                new List<double>() { 10, 11, 12 }
//            };
//            var bandwidth = 2.0;

//            // Act
//            var result = clustering.Cluster(data, bandwidth);

//            // Assert
//            Assert.Equal(2, result.Count);
//        }

//        [Fact]
//        public void Cluster_ReturnsCorrectClusters()
//        {
//            // Arrange
//            var clustering = new MeanShiftClustering();
//            var data = new List<List<double>>()
//            {
//                new List<double>() { 1, 2, 3 },
//                new List<double>() { 4, 5, 6 },
//                new List<double>() { 7, 8, 9 },
//                new List<double>() { 10, 11, 12 }
//            };
//            var bandwidth = 2.0;

//            // Act
//            var result = clustering.Cluster(data, bandwidth);

//            // Assert
//            Assert.Contains(new List<double>() { 1, 2, 3 }, result[0]);
//            Assert.Contains(new List<double>() { 4, 5, 6 }, result[0]);
//            Assert.Contains(new List<double>() { 7, 8, 9 }, result[1]);
//            Assert.Contains(new List<double>() { 10, 11, 12 }, result[1]);
//        }
//    }
//}
