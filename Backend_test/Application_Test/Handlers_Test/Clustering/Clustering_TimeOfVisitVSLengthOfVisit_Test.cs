using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure.Data;
using WebApplication1.Utils;

public class Clustering_TimeOfVisitVSLengthOfVisit_Test : BaseTest
{
    private IHandler<Clustering_TimeOfVisit_LengthOfVisit_Command, ClusteringReturn> handler;

    private Establishment establsihment = new Establishment();
    private List<Sale> sales = new List<Sale>();
    public Clustering_TimeOfVisitVSLengthOfVisit_Test() : base(new List<ITestService> { TestContainer.CreateAsync().Result })
    {
        handler = scope.ServiceProvider.GetRequiredService<IHandler<Clustering_TimeOfVisit_LengthOfVisit_Command, ClusteringReturn>>();

        var testDataCreatorService = scope.ServiceProvider.GetRequiredService<ITestDataCreatorService>();

        List<OpeningHours> openingHours = testDataCreatorService.CreateSimpleOpeningHoursForWeek(open: new LocalTime(8, 0), close: new LocalTime(16, 0));
        List<DateTime> calendar = testDataCreatorService.OpenHoursCalendar(DateTime.Today.AddDays(-7), DateTime.Today, timeResolution: WebApplication1.Utils.TimeResolution.Hour, openingHours);
        Dictionary<DateTime, int> distribution = testDataCreatorService.DistributionOnTimeres(calendar, TestDataCreatorService.GetCosineFunction(period: 8 * Math.PI, verticalShift: 5, horizontalShift: 12), TimeResolution.Hour);

        establsihment = new Establishment("Cafe 1");

        Random random = new Random(1);
        foreach (KeyValuePair<DateTime, int> kvp in distribution)
        {

        }
        //Save to DB
        IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        using (unitOfWork)
        {
            unitOfWork.establishmentRepository.Add(establsihment);
        }

    }

    [Fact]
    public async void Cluster_WithLargeTimeBandwith_ShouldCreateClustersForEachItemCollection()
    {
        //ARRANGE
        var bandwidthTimeOfVisit = 500;
        var bandwidthTotalPrice = 20;
        Clustering_TimeOfVisit_LengthOfVisit_Command command =
            new Clustering_TimeOfVisit_LengthOfVisit_Command(
            establishmentId: establsihment.Id,
            salesIds: sales.Select(x => x.Id).ToList());

        //ACT
        ClusteringReturn result = await handler.Handle(command);

        //ASSERT
        List<List<Sale>> salesInClusters = result.clusters.Select(x => x.Select(y => establsihment.GetSales().Find(z => z.Id == y)).ToList()).ToList();
        List<List<Item>> items = salesInClusters.Select(x => x.Select(y => y.SalesItems[0].Item).ToList()).ToList();

        //Correct clusters
        Assert.Equal(3, result.clusters.Count);
        foreach (var itemList in items)
        {
            foreach (var item in itemList)
            {
                Assert.Equal(itemList[0], item);
            }
        }
    }
}