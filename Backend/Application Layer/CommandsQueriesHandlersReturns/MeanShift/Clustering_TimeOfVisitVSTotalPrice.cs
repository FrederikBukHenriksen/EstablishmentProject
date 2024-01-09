using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Services;
using WebApplication1.Services.Analysis;
using WebApplication1.Utils;

namespace WebApplication1.CommandHandlers
{
    public class Clustering_TimeOfVisit_TotalPrice_Command : CommandBase
    {
        public SalesSortingParameters? salesSortingParameters { get; set; }
        public string id { get; set; }
    }

    public class Clustering_TimeOfVisit_TotalPrice_Return : ReturnBase
    {
        public List<List<Guid>> clusters { get; set; }
        public Dictionary<Guid, Dictionary<string, double>> calculations { get; set; }
    }

    public class Clustering_TimeOfVisitVSTotalPrice : HandlerBase<Clustering_TimeOfVisit_TotalPrice_Command, Clustering_TimeOfVisit_TotalPrice_Return>
    {
        private IUserContextService userContextService;
        private ISalesRepository salesRepository;
        private IEstablishmentRepository establishmentRepository;

        public Clustering_TimeOfVisitVSTotalPrice(IUserContextService userContextService, IEstablishmentRepository establishmentRepository, ISalesRepository salesRepository)
        {
            this.userContextService = userContextService;
            this.salesRepository = salesRepository;
            this.establishmentRepository = establishmentRepository;
        }

        public override Clustering_TimeOfVisit_TotalPrice_Return Handle(Clustering_TimeOfVisit_TotalPrice_Command command)
        {
            //Fetch
            List<Sale> sales = this.userContextService.GetActiveEstablishment().GetSales();

            //Arrange
            List<(Sale sale, Dictionary<string, double>)> saleDataAttributes = sales
                .Where(sale => sale.GetTimespanOfVisit() != null)
                .Select(sale => (
                    entity: sale,
                    values: new Dictionary<string, double> { { "TimeOfVisit", sale.GetTimeOfSale().Second }, { "LengthOfVisit", ((TimeSpan)sale.GetTimespanOfVisit()).Seconds } }
                    ))
                .ToList();

            List<(Sale, List<double>)> saleData = saleDataAttributes.Select(x => (x.Item1, x.Item2.Select(y => y.Value).ToList())).ToList();

            var bandwith = new List<double> { 1, 1 };

            //Act
            List<List<Sale>> clusteredSales = MeanShiftClustering.Cluster(saleData, bandwith);

            //Return

            return new Clustering_TimeOfVisit_TotalPrice_Return
            {
                clusters = clusteredSales.Select(innerList => innerList.Select(sale => sale.Id).ToList()).ToList(),
                calculations = saleDataAttributes.ToDictionary(x => x.Item1.Id, x => x.Item2)
            };
        }
    }
}
