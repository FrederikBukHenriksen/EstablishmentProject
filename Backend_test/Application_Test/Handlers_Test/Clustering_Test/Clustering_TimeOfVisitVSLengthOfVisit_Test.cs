using EstablishmentProject.test.TestingCode;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Handlers.MeanShift;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure.Data;
using WebApplication1.Utils;

public class Clustering_TimeOfVisitVSLengthOfVisit_Test : IntegrationTest
{
    private IHandler<Clustering_TimeOfVisit_LengthOfVisit_Command, ClusteringReturn> handler;
    private IUnitOfWork unitOfWork;
    private Establishment establishment = new Establishment();
    public Clustering_TimeOfVisitVSLengthOfVisit_Test() : base(new List<ITestService> { DatabaseTestContainer.CreateAsync().Result })
    {
        handler = scope.ServiceProvider.GetRequiredService<IHandler<Clustering_TimeOfVisit_LengthOfVisit_Command, ClusteringReturn>>();
        unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        //ARRANGE
        establishment = new Establishment("Cafe 1");
        CreateTestData();
        using (var uow = unitOfWork)
        {
            uow.establishmentRepository.Add(establishment);
        }
    }

    [Fact]
    public async void Cluster_WithLargeTimeBandwith_ShouldCreateClustersWithOnlyOneTypeOfItem()
    {
        //ARRANGE
        Clustering_TimeOfVisit_LengthOfVisit_Command command =
            new Clustering_TimeOfVisit_LengthOfVisit_Command(
            establishmentId: establishment.Id,
            salesIds: establishment.GetSales().Select(x => x.Id).ToList(),
            bandwidthTimeOfVisit: 250,
            bandwidthLengthOfVisit: 40);

        //ACT
        ClusteringReturn result = await handler.Handle(command);

        //ASSERT
        Assert.Equal(2, result.clusters.Count);
    }


    private void CreateTestData()
    {
        Func<double, double> linearFirstDistribution = TestDataBuilder.GetLinearFuncition(2, -8 * 2);
        Func<double, double> linearSecondDistribution = TestDataBuilder.GetLinearFuncition(-2, 32);

        var testDataBuilder = new TestDataBuilder();

        var firstSalesDistribution = testDataBuilder.FINALgenerateDistrubution(DateTime.Today.AddDays(-1), DateTime.Today, linearFirstDistribution, TimeResolution.Hour);
        var firstSales = testDataBuilder.FINALFilterOnOpeningHours(8, 12, firstSalesDistribution);
        var secondSalesDistribution = testDataBuilder.FINALgenerateDistrubution(DateTime.Today.AddDays(-1), DateTime.Today, linearSecondDistribution, TimeResolution.Hour);
        var secondSales = testDataBuilder.FINALFilterOnOpeningHours(12, 16, secondSalesDistribution);

        var aggregate = testDataBuilder.FINALAggregateDistributions([firstSales, secondSales]);

        var normalRandomSeed = new SystemRandomSource(1);

        Normal normal = new Normal(0, 5, normalRandomSeed);
        foreach (var distribution in aggregate.ToList())
        {
            for (int i = 0; i < distribution.Value; i++)
            {
                var randomNormalDistributionNumber = normal.RandomSource.Next(0, 30);
                var sale = establishment.CreateSale(distribution.Key);
                sale.setTimeOfArrival(distribution.Key.AddMinutes(randomNormalDistributionNumber * (-1)));
                establishment.AddSale(sale);
            }
        }

        foreach (var distribution in aggregate.ToList())
        {
            for (int i = 0; i < distribution.Value; i++)
            {
                var randomNormalDistributionNumber = normal.RandomSource.Next(30, 60);
                var sale = establishment.CreateSale(distribution.Key);
                sale.setTimeOfArrival(distribution.Key.AddMinutes(randomNormalDistributionNumber * (-1)));
                establishment.AddSale(sale);

            }
        }
    }
}