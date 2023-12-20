using System.Collections.Generic;
using WebApplication1.Domain.Entities;
using WebApplication1.Services.Analysis;
using Xunit;

namespace EstablishmentProject.Test
{
    public class MeanShiftClusteringTests
    {
        [Fact]
        public void Cluster_ReturnsCorrectNumberOfClusters()
        {
               // Arrange
            var data = new List<(Sale sale, List<double> values)>()
            {
                (new Sale(), new List<double>() { 1, 2, 3 }),
                (new Sale(), new List<double>() { 4, 5, 6 }),
                (new Sale(), new List<double>() { 7, 8, 9 }),
                (new Sale(), new List<double>() { 10, 11, 12 })
            };
            var bandwidth = new List<double> { 2.0, 2.0, 2.0 };

            // Act
            var result = MeanShiftClustering.Cluster(data, bandwidth);

            // Assert
            Assert.Equal(2, result.Count);
        }



        [Fact]
        public void Cluster_ReturnsCorrectNumberOfPointsInClusters()
        {
            // Arrange
            var data = new List<(Sale sale, List<double> values)>()
            {
                (new Sale(), new List<double>() { 1, 2, 3 }),
                (new Sale(), new List<double>() { 4, 5, 6 }),
                (new Sale(), new List<double>() { 7, 8, 9 }),
                (new Sale(), new List<double>() { 10, 11, 12 })
            };
            var bandwidth = new List<double> { 2.0, 2.0, 2.0 };

            // Act
            var result = MeanShiftClustering.Cluster(data, bandwidth);

            // Assert
        }

    }
}
