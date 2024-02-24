using EstablishmentProject.test.TestingCode;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure.Data;
using WebApplication1.Utils;

public class Clustering_TimeOfVisit_TotalPrice_Test : IntegrationTest
{
    private IHandler<Clustering_TimeOfVisit_TotalPrice_Command, ClusteringReturn> Clustering_TimeOfVisitVSTotalPrice;
    private IUnitOfWork unitOfWork;
    private ITestDataBuilder testDataBuilder;
    private Establishment establishment = new Establishment();
    private Item testItem;

    public Clustering_TimeOfVisit_TotalPrice_Test() : base(new List<ITestService> { DatabaseTestContainer.CreateAsync().Result })
    {
        Clustering_TimeOfVisitVSTotalPrice = scope.ServiceProvider.GetRequiredService<IHandler<Clustering_TimeOfVisit_TotalPrice_Command, ClusteringReturn>>();
        testDataBuilder = new TestDataBuilder();
        unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        //ARRANGE
        establishment = new Establishment("Cafe 1");
        testItem = establishment.CreateItem("test", 1);
        establishment.AddItem(testItem);

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
        Clustering_TimeOfVisit_TotalPrice_Command command =
            new Clustering_TimeOfVisit_TotalPrice_Command(
            establishmentId: establishment.Id,
            salesIds: establishment.GetSales().Select(x => x.Id).ToList(),
            bandwidthTimeOfVisit: 80,
            bandwidthTotalPrice: 100);

        //ACT
        ClusteringReturn result = await Clustering_TimeOfVisitVSTotalPrice.Handle(command);

        //ASSERT
        List<List<Sale>> salesInClusters = result.clusters.Select(x => x.Select(y => establishment.GetSales().Find(z => z.Id == y)).ToList()).ToList();
        List<List<Item>> items = salesInClusters.Select(x => x.Select(y => y.SalesItems[0].Item).ToList()).ToList();

        //Correct clusters
        foreach (var itemList in items)
        {
            foreach (var item in itemList)
            {
                Assert.Equal(itemList[0], item);
            }
        }
    }


    private void CreateTestData()
    {
        Func<double, double> morningSalesDistribution = (x) => TestDataBuilder.GetNormalFunction(10, 2)(x) * 20;
        Func<double, double> afternoonSalesDistribution = (x) => TestDataBuilder.GetNormalFunction(14, 2)(x) * 15;
        Func<double, double> wholeDaySalesDistribution = (x) => TestDataBuilder.GetNormalFunction(14, 2)(x) * 10;

        var morningBreakfast = testDataBuilder.FINALFilterOnOpeningHours(8, 12, testDataBuilder.FINALgenerateDistrubution(DateTime.Today.AddDays(-7), DateTime.Today, morningSalesDistribution, TimeResolution.Hour));
        var afternoonLunch = testDataBuilder.FINALFilterOnOpeningHours(12, 16, testDataBuilder.FINALgenerateDistrubution(DateTime.Today.AddDays(-7), DateTime.Today, afternoonSalesDistribution, TimeResolution.Hour));
        var wholeDayCoffee = testDataBuilder.FINALFilterOnOpeningHours(8, 16, testDataBuilder.FINALgenerateDistrubution(DateTime.Today.AddDays(-7), DateTime.Today, wholeDaySalesDistribution, TimeResolution.Hour));

        var normalRandomSeed = new SystemRandomSource(1);


        Normal morningDistribution = new Normal(60, 10, normalRandomSeed);
        foreach (var distribution in morningBreakfast.ToList())
        {
            for (int i = 0; i < distribution.Value; i++)
            {
                var randomNormalDistributionNumber = morningDistribution.RandomSource.Next(80, 121);
                var sale = establishment.CreateSale(distribution.Key);
                establishment.AddSale(sale);
                establishment.AddSalesItems(sale, establishment.CreateSalesItem(sale, testItem, randomNormalDistributionNumber));
            }
        }

        Normal afternoonDistribution = new Normal(60, 10, normalRandomSeed);

        foreach (var distribution in afternoonLunch.ToList())
        {
            for (int i = 0; i < distribution.Value; i++)
            {
                var randomNormalDistributionNumber = afternoonDistribution.RandomSource.Next(80, 121);
                var sale = establishment.CreateSale(distribution.Key);
                establishment.AddSale(sale);
                establishment.AddSalesItems(sale, establishment.CreateSalesItem(sale, testItem, randomNormalDistributionNumber));
            }
        }
    }
}