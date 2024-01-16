using NJsonSchema.NewtonsoftJson.Converters;
using System.Runtime.Serialization;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Utils;

namespace WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.SalesHandlers
{
    [Newtonsoft.Json.JsonConverter(typeof(JsonInheritanceConverter), "$type")]
    [KnownType(typeof(SalesStatisticNumber))]

    public abstract class SalesStatisticsCommand : CommandBase, ICmdField_SalesIds
    {
        public Guid EstablishmentId { get; set; }
        public List<Guid> SalesIds { get; set; }
        public DateTimePeriod TimePeriod { get; set; }
        public TimeResolution TimeResolution { get; set; }
        public abstract Dictionary<DateTime, double> Calculation(IEnumerable<Sale> sales, DateTimePeriod period, TimeResolution timeResolution);
    }

    public class SalesStatisticNumber : SalesStatisticsCommand
    {
        public override Dictionary<DateTime, double> Calculation(IEnumerable<Sale> sales, DateTimePeriod period, TimeResolution timeResolution)
        {
            List<DateTime> timeline = TimeHelper.CreateTimelineAsList(period, timeResolution);
            Dictionary<DateTime, List<Sale>> salesMappedToTimeline = TimeHelper.MapObjectsToTimeline(sales, x => x.TimestampPayment, timeline, timeResolution);
            Dictionary<DateTime, double> salesCountPerDateTime = salesMappedToTimeline.ToDictionary(
                kvp => kvp.Key,
                kvp => (double)kvp.Value.Count
            );
            return salesCountPerDateTime;
        }
    }

    public class SalesStatisticsReturn : ReturnBase
    {
        public Dictionary<DateTime, double> data { get; set; }
    }

    public class SalesStatisticHandler : HandlerBase<SalesStatisticsCommand, SalesStatisticsReturn>
    {
        private IUnitOfWork unitOfWork;

        public SalesStatisticHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public override async Task<SalesStatisticsReturn> Handle(SalesStatisticsCommand command)
        {
            //Fetch
            List<Sale> sales = this.unitOfWork.salesRepository.GetFromIds(command.SalesIds);


            //Act
            Dictionary<DateTime, double> result = command.Calculation(sales, command.TimePeriod, command.TimeResolution);

            //Return
            return new SalesStatisticsReturn { data = result };
        }
    }
}
