using NJsonSchema.NewtonsoftJson.Converters;
using System.Runtime.Serialization;
using WebApplication1.Application_Layer.Handlers.SalesHandlers;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services.Analysis;

namespace WebApplication1.CommandHandlers
{
    [Newtonsoft.Json.JsonConverter(typeof(JsonInheritanceConverter), "$type")]
    [KnownType(typeof(Clustering_TimeOfVisit_TotalPrice_Command))]
    [KnownType(typeof(Clustering_TimeOfVisit_LengthOfVisit_Command))]
    public abstract class ClusteringCommand : CommandBase, ICmdField_SalesIds
    {
        public ClusteringCommand(
            Guid establishmentId,
            List<Guid> salesIds
            )
        {
            this.EstablishmentId = establishmentId;
            this.SalesIds = salesIds;

        }
        public Guid EstablishmentId { get; set; }
        public List<Guid> SalesIds { get; set; }

    }

    public class Clustering_TimeOfVisit_TotalPrice_Command : ClusteringCommand
    {
        public double bandwidthTimeOfVisit;
        public double bandwidthTotalPrice;

        public Clustering_TimeOfVisit_TotalPrice_Command(Guid establishmentId, List<Guid> salesIds, double bandwidthTimeOfVisit, double bandwidthTotalPrice) : base(establishmentId, salesIds)
        {
            this.bandwidthTimeOfVisit = bandwidthTimeOfVisit;
            this.bandwidthTotalPrice = bandwidthTotalPrice;
        }
    }

    public class Clustering_TimeOfVisit_LengthOfVisit_Command : ClusteringCommand
    {
        public Clustering_TimeOfVisit_LengthOfVisit_Command(Guid establishmentId, List<Guid> salesIds) : base(establishmentId, salesIds)
        {
        }

    }

    public class ClusteringReturn : ReturnBase
    {

        public ClusteringReturn(
            List<List<Guid>> clusters,
            List<(Guid id, List<double> values)> calculationValues)
        {
            this.clusters = clusters;
            this.calculationValues = calculationValues;
        }
        public List<List<Guid>> clusters { get; }
        public List<(Guid id, List<double> values)> calculationValues { get; }
    }

    public class Clustering_TimeOfVisitVSTotalPrice : HandlerBase<Clustering_TimeOfVisit_TotalPrice_Command, ClusteringReturn>
    {
        private IHandler<GetSalesCommand, GetSalesRawReturn> getSalesHandler;

        public Clustering_TimeOfVisitVSTotalPrice(IHandler<GetSalesCommand, GetSalesRawReturn> getSalesHandler)
        {
            this.getSalesHandler = getSalesHandler;
        }

        public override async Task<ClusteringReturn> Handle(Clustering_TimeOfVisit_TotalPrice_Command command)
        {
            //Fetch
            var localCommand = new GetSalesCommand { EstablishmentId = command.EstablishmentId, SalesIds = command.SalesIds };
            List<Sale> sales = (await this.getSalesHandler.Handle(localCommand)).Sales;

            //Arrange
            List<(Sale sale, List<double> values)> saleData = sales
                .Select(sale => (
                    entity: sale,
                    values: new List<double> { sale.GetTimeOfSale().TimeOfDay.TotalMinutes, sale.GetTotalPrice() }
                    ))
                .ToList();

            List<(Guid Id, List<double> values)> calculationValues = saleData.Select(x => (x.sale.Id, x.values)).ToList();

            var bandwith = new List<double> { command.bandwidthTimeOfVisit, command.bandwidthTotalPrice };


            //Act
            List<List<Sale>> clusteredSales = new MeanShiftClusteringStepByStep().Cluster(saleData, bandwith);
            //List<List<Sale>> clusteredSales = new MeanShiftClusteringDirectly().Cluster(saleData, bandwith);


            //Return
            var ok = saleData.Select(x => (x.sale.Id, x.values)).ToList();
            return new ClusteringReturn(
                clusters: clusteredSales.Select(innerList => innerList.Select(sale => sale.Id).ToList()).ToList(),
                calculationValues: calculationValues
                );

        }
    }

    public class Clustering_TimeOfVisitVSLengthOfVisit : HandlerBase<Clustering_TimeOfVisit_LengthOfVisit_Command, ClusteringReturn>
    {
        private IUnitOfWork unitOfWork;

        public Clustering_TimeOfVisitVSLengthOfVisit(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public override async Task<ClusteringReturn> Handle(Clustering_TimeOfVisit_LengthOfVisit_Command command)
        {
            //Fetch
            Establishment establishment = this.unitOfWork.establishmentRepository.IncludeSales().IncludeSalesItems().GetById(command.EstablishmentId);
            List<Sale> sales = establishment.GetSales().Where(sale => command.SalesIds.Any(x => x == sale.Id)).ToList();

            List<(Sale sale, List<double> values)> saleData = sales
                .Where(sale => sale.GetTimespanOfVisit != null)
                .Select(sale => (
                    entity: sale,
                    values: new List<double> { sale.GetTimeOfSale().TimeOfDay.TotalMinutes, ((TimeSpan)sale.GetTimespanOfVisit()).TotalMinutes }
                    ))
                .ToList();

            var bandwith = new List<double> { 1, 1 };

            //Act
            List<List<Sale>> clusteredSales = new MeanShiftClusteringDirectly().Cluster(saleData, bandwith);

            //Return
            return new ClusteringReturn
            (
                clusters: clusteredSales.Select(innerList => innerList.Select(sale => sale.Id).ToList()).ToList(),
                calculationValues: saleData.Select(x => (x.sale.Id, x.values)).ToList()

            );
        }
    }

}
