using WebApplication1.Application_Layer.Handlers.MeanShift;
using WebApplication1.Application_Layer.Handlers.SalesHandlers;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services.Analysis;

namespace WebApplication1.CommandHandlers
{
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
            var getSalesCommand = new GetSalesCommand { EstablishmentId = command.EstablishmentId, SalesIds = command.SalesIds };
            List<Sale> sales = (await this.getSalesHandler.Handle(getSalesCommand)).Sales;

            //Arrange
            List<(Sale sale, List<double> values)> saleData = sales
                .Select(sale => (
                    entity: sale,
                    values: new List<double> { sale.GetTimeOfSale().TimeOfDay.TotalMinutes, sale.GetTotalPrice() }
                    ))
                .ToList();

            List<(Guid Id, List<double> values)> calculationValues = saleData.Select(x => (x.sale.Id, x.values)).ToList();
            List<double> bandwith = [command.bandwidthTimeOfVisit, command.bandwidthTotalPrice];

            //Act
            List<List<Sale>> clusteredSales = new MeanShiftClusteringStationary().Cluster(saleData, bandwith);


            //Return
            var ok = saleData.Select(x => (x.sale.Id, x.values)).ToList();
            return new ClusteringReturn(
                clusters: clusteredSales.Select(innerList => innerList.Select(sale => sale.Id).ToList()).ToList(),
                calculationValues: calculationValues
                );
        }
    }
}
