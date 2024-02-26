using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services.Analysis;

namespace WebApplication1.Application_Layer.Handlers.MeanShift
{
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
