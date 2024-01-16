using NJsonSchema.NewtonsoftJson.Converters;
using System.Runtime.Serialization;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services;
using WebApplication1.Services.Analysis;
using WebApplication1.Utils;

namespace WebApplication1.CommandHandlers
{
    [Newtonsoft.Json.JsonConverter(typeof(JsonInheritanceConverter), "$type")]
    [KnownType(typeof(Clustering_TimeOfVisit_TotalPrice_Command))]
    [KnownType(typeof(Clustering_TimeOfVisit_LengthOfVisit_Command))]

    public abstract class ClusteringCommand : CommandBase
    {
    }

    public class Clustering_TimeOfVisit_TotalPrice_Command : ClusteringCommand
    {
        public SalesSorting? salesSortingParameters { get; set; }
    }

    public class Clustering_TimeOfVisit_LengthOfVisit_Command : ClusteringCommand
    {
        public SalesSorting? salesSortingParameters { get; set; }
    }

    public class ClusteringReturn : ReturnBase
    {
        public List<List<Guid>> clusters { get; set; }
    }

    public class Clustering_TimeOfVisitVSTotalPrice : HandlerBase<Clustering_TimeOfVisit_TotalPrice_Command, ClusteringReturn>
    {
        private IUserContextService userContextService;

        public Clustering_TimeOfVisitVSTotalPrice(IUserContextService userContextService)
        {
            this.userContextService = userContextService;
        }

        public override async Task<ClusteringReturn> Handle(Clustering_TimeOfVisit_TotalPrice_Command command)
        {
            //Fetch
            List<Sale> sales = this.userContextService.GetActiveEstablishment().GetSales();

            //Arrange
            List<(Sale sale, List<double>)> saleData = sales
                .Select(sale => (
                    entity: sale,
                    values: new List<double> { sale.GetTimeOfSale().TimeOfDay.TotalMinutes, sale.GetTotalPrice() }
                    ))
                .ToList();

            var bandwith = new List<double> { 1, 35 };

            //Act
            List<List<Sale>> clusteredSales = MeanShiftClustering.Cluster(saleData, bandwith);

            //Return
            return new ClusteringReturn
            {
                clusters = clusteredSales.Select(innerList => innerList.Select(sale => sale.Id).ToList()).ToList()
            };
        }
    }

    public class Clustering_TimeOfVisitVSLengthOfVisit : HandlerBase<Clustering_TimeOfVisit_LengthOfVisit_Command, ClusteringReturn>
    {
        private IUserContextService userContextService;

        public Clustering_TimeOfVisitVSLengthOfVisit(IUserContextService userContextService)
        {
            this.userContextService = userContextService;
        }

        public override async Task<ClusteringReturn> Handle(Clustering_TimeOfVisit_LengthOfVisit_Command command)
        {
            //Fetch
            List<Sale> sales = this.userContextService.GetActiveEstablishment().GetSales();

            List<(Sale sale, List<double>)> saleData = sales
                .Where(sale => sale.GetTimespanOfVisit != null)
                .Select(sale => (
                    entity: sale,
                    values: new List<double> { sale.GetTimeOfSale().TimeOfDay.TotalMinutes, ((TimeSpan)sale.GetTimespanOfVisit()).TotalMinutes }
                    ))
                .ToList();

            var bandwith = new List<double> { 1, 1 };

            //Act
            List<List<Sale>> clusteredSales = MeanShiftClustering.Cluster(saleData, bandwith);

            //Return
            return new ClusteringReturn
            {
                clusters = clusteredSales.Select(innerList => innerList.Select(sale => sale.Id).ToList()).ToList(),
            };
        }
    }

}
