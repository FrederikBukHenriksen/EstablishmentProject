using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Infrastructure_Layer.DataTransferObjects;
using WebApplication1.Utils;

namespace WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.Sales
{
    public class GetSalesCommand : CommandBase
    {
        public SalesSortingParameters? SortingParameters { get; set; }
        public List<Guid> SalesIds { get; set; }
    }

    public class GetSalesReturn : ReturnBase
    {
        public List<SaleDTO> Sales { get; set; }
    }

    public class GetSalesHandler
    {
    }
}
