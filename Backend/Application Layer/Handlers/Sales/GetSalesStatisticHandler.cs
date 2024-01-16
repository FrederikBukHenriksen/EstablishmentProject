using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Services;
using WebApplication1.Utils;

namespace WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.Sales
{
    public abstract class GetSalesStatisticsCommand : CommandBase
    {
        public Guid? EstablishmentId { get; set; }
        public SalesSortingParameters? SalesSortingParameters { get; set; }
        public DateTimePeriod TimePeriod { get; set; }
        public TimeResolution TimeResolution { get; set; }
        public abstract Dictionary<DateTime, double> Calculation(IEnumerable<Sale> sales, DateTimePeriod period, TimeResolution timeResolution);
    }

    public class GetSalesAverageNumber : GetSalesStatisticsCommand
    {
        public override Dictionary<DateTime, double> Calculation(IEnumerable<Sale> sales, DateTimePeriod period, TimeResolution timeResolution)
        {
            List<DateTime> timeline = TimeHelper.CreateTimelineAsList(period, timeResolution);
            Dictionary<DateTime, List<Sale>> salesMappedToTimeline = TimeHelper.MapObjectsToTimeline(sales, x => x.TimestampPayment, timeline, timeResolution);
            Dictionary<DateTime, double> salesPerDateTime = salesMappedToTimeline.GroupBy(x => x.Key).ToDictionary(x => x.Key, x => (double)x.Count());
            return salesPerDateTime;
        }
    }

    public class GetSalesStatisticsReturn : ReturnBase
    {
        public Dictionary<DateTime, double> data { get; set; }
    }

    public class GetSalesStatisticHandler : HandlerBase<GetSalesStatisticsCommand, GetSalesStatisticsReturn>
    {
        private IEstablishmentRepository establishmentRepository;
        private IUserContextService contextService;

        public GetSalesStatisticHandler(IEstablishmentRepository establishmentRepository, IUserContextService contextService)
        {
            this.establishmentRepository = establishmentRepository;
            this.contextService = contextService;
        }

        public override GetSalesStatisticsReturn Handle(GetSalesStatisticsCommand command)
        {
            //Arrange
            Domain_Layer.Entities.Establishment establisment = command.EstablishmentId != null ? this.contextService.TrySetActiveEstablishment((Guid)command.EstablishmentId) : this.contextService.GetActiveEstablishment();
            List<Sale> sales = establisment.Sales.ToList();

            //Act
            List<Sale> filteredSales = SalesSortingParametersExecute.SortSales(sales, command.SalesSortingParameters);
            Dictionary<DateTime, double> result = command.Calculation(sales, command.TimePeriod, command.TimeResolution);

            return new GetSalesStatisticsReturn { data = result };
        }
    }
}
