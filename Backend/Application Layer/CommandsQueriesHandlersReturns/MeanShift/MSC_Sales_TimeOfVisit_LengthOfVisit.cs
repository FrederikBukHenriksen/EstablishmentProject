using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Services;

namespace WebApplication1.CommandHandlers
{
    public class MSC_Sales_TimeOfVisit_LengthOfVisit : MeanShiftClusteringCommand
    {
        private List<Sale> sales;
        public MSC_Sales_TimeOfVisit_LengthOfVisit(IUserContextService userContextService, ISalesRepository salesRepository)
        {
            this.sales = salesRepository.GetSalesFromEstablishment(userContextService.GetActiveEstablishment());
        }

        public override List<(Sale,List<double>)> GetData()
        {
        List<(Sale sale, List<double> values)> SalesWithValues = sales
            .Select(sale => (
                entity: sale,
                values: new List<double> { sale.GetTimeOfSale().Second, sale.GetTotalPrice() }
                ))
            .ToList();

            return SalesWithValues;
        }

        public override List<double> GetBandwidths()
        {
            return new List<double> { 1, 1 };
        }
    }
}
