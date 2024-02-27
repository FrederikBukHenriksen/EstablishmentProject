using WebApplication1.Application_Layer.Handlers.SalesHandlers;
using WebApplication1.CommandHandlers;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services.Analysis;

namespace WebApplication1.Application_Layer.Handlers.MeanShift
{

    public class Clustering_TimeOfVisit_LengthOfVisit_Command : ClusteringCommand
    {
        public double bandwidthTimeOfVisit;
        public double bandwidthLengthOfVisit;
        public Clustering_TimeOfVisit_LengthOfVisit_Command(Guid establishmentId, List<Guid> salesIds, double bandwidthTimeOfVisit, double bandwidthLengthOfVisit) : base(establishmentId, salesIds)
        {
            this.bandwidthTimeOfVisit = bandwidthTimeOfVisit;
            this.bandwidthLengthOfVisit = bandwidthLengthOfVisit;
        }
    }

    public class Clustering_TimeOfVisitVSLengthOfVisit : HandlerBase<Clustering_TimeOfVisit_LengthOfVisit_Command, ClusteringReturn>
    {
        private IHandler<GetSalesCommand, GetSalesRawReturn> getSalesHandler;

        public Clustering_TimeOfVisitVSLengthOfVisit(IHandler<GetSalesCommand, GetSalesRawReturn> getSalesHandler)
        {
            this.getSalesHandler = getSalesHandler;
        }

        public override async Task<ClusteringReturn> Handle(Clustering_TimeOfVisit_LengthOfVisit_Command command)
        {
            //Fetch
            var getSalesCommand = new GetSalesCommand { EstablishmentId = command.EstablishmentId, SalesIds = command.SalesIds };
            List<Sale> sales = (await this.getSalesHandler.Handle(getSalesCommand)).Sales;

            List<(Sale sale, List<double> values)> saleData = sales
                .Where(sale => sale.GetTimespanOfVisit != null)
                .Select(sale => (
                    entity: sale,
                    values: new List<double> { sale.GetTimeOfSale().TimeOfDay.TotalMinutes, ((TimeSpan)sale.GetTimespanOfVisit()).TotalMinutes }
                    ))
                .ToList();

            List<double> bandwith = [command.bandwidthTimeOfVisit, command.bandwidthLengthOfVisit];

            //Act
            List<List<Sale>> clusteredSales = new MeanShiftClusteringStationary().Cluster(saleData, bandwith);

            //Return
            return new ClusteringReturn
            (
                clusters: clusteredSales.Select(innerList => innerList.Select(sale => sale.Id).ToList()).ToList(),
                calculationValues: saleData.Select(x => (x.sale.Id, x.values)).ToList()

            );
        }
    }
}
