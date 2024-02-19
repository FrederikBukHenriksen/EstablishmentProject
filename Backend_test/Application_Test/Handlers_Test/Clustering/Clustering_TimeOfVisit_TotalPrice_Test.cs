using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Infrastructure.Data;
using WebApplication1.Utils;

public class Clustering_TimeOfVisit_TotalPrice_Test : IntegrationTest
{
    private IHandler<Clustering_TimeOfVisit_TotalPrice_Command, ClusteringReturn> Clustering_TimeOfVisitVSTotalPrice;
    private IUnitOfWork unitOfWork;
    private ITestDataCreatorService testDataCreatorService;
    private Establishment establishment = new Establishment();
    private Item espresso;
    private Item coffee;
    private Item latte;
    private List<Sale> sales = new List<Sale>();
    private Random random;

    public Clustering_TimeOfVisit_TotalPrice_Test() : base(new List<ITestService> { DatabaseTestContainer.CreateAsync().Result })
    {
        Clustering_TimeOfVisitVSTotalPrice = scope.ServiceProvider.GetRequiredService<IHandler<Clustering_TimeOfVisit_TotalPrice_Command, ClusteringReturn>>();
        testDataCreatorService = scope.ServiceProvider.GetRequiredService<ITestDataCreatorService>();
        unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        //ARRANGE
        establishment = new Establishment("Cafe 1");
        espresso = establishment.CreateItem("Espresso", 35);
        establishment.AddItem(espresso);
        coffee = establishment.CreateItem("Coffee", 35);
        establishment.AddItem(coffee);
        latte = establishment.CreateItem("Latte", 50);
        establishment.AddItem(latte);
    }

    [Fact]
    public async void Cluster_WithLargeTimeBandwith_ShouldCreateClustersWithOnlyOneTypeOfItem()
    {
        //ARRANGE
        Arrange_SimpleButRandomSales();

        var bandwidthTimeOfVisit = 30;
        var bandwidthTotalPrice = 30;
        Clustering_TimeOfVisit_TotalPrice_Command command =
            new Clustering_TimeOfVisit_TotalPrice_Command(
            establishmentId: establishment.Id,
            salesIds: establishment.GetSales().Select(x => x.Id).ToList(),
            bandwidthTimeOfVisit: bandwidthTimeOfVisit,
            bandwidthTotalPrice: bandwidthTotalPrice);

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

    private void Arrange_SimpleButRandomSales()
    {
        List<OpeningHours> openingHours = testDataCreatorService.CreateSimpleOpeningHoursForWeek(open: new LocalTime(8, 0), close: new LocalTime(16, 0));
        List<DateTime> calendar = testDataCreatorService.OpenHoursCalendar(DateTime.Today.AddDays(-7), DateTime.Today, timeResolution: WebApplication1.Utils.TimeResolution.Hour, openingHours);
        Dictionary<DateTime, int> distribution = testDataCreatorService.DistributionOnTimeres(calendar, TestDataCreatorService.GetCosineFunction(8, verticalShift: 10, horizontalShift: 12), TimeResolution.Hour);

        random = new Random(1);
        var tVar = 15;

        foreach (KeyValuePair<DateTime, int> kvp in distribution)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                if (kvp.Key < kvp.Key.Date.AddHours(11))
                {
                    int randomNumber = random.Next(1, 3);
                    var sale = establishment.CreateSale(timestampPayment: timeVariance(kvp.Key, tVar), itemAndQuantity: new List<(Item, int)> { (espresso, randomNumber) });
                    establishment.AddSale(sale);
                }
                else if (kvp.Key > kvp.Key.Date.AddHours(12) && kvp.Key < kvp.Key.Date.AddHours(14))
                {
                    int randomNumber = random.Next(1, 2);
                    var sale = establishment.CreateSale(timestampPayment: timeVariance(kvp.Key, tVar), itemAndQuantity: new List<(Item, int)> { (coffee, randomNumber) });
                    establishment.AddSale(sale);
                }
                else if (kvp.Key > kvp.Key.Date.AddHours(15))
                {
                    int randomNumber = random.Next(1, 2);
                    var sale = establishment.CreateSale(timestampPayment: timeVariance(kvp.Key, tVar), itemAndQuantity: new List<(Item, int)> { (latte, randomNumber) });
                    establishment.AddSale(sale);
                }
            }
        }

        using (unitOfWork)
        {
            unitOfWork.establishmentRepository.Add(establishment);
        }
    }

    private DateTime timeVariance(DateTime datetime, int minutes)
    {
        return datetime.AddMinutes(random.Next(-minutes, minutes + 1));
    }
}