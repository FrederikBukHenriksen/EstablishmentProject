using DMIOpenData;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Services;
using WebApplication1.Services.Analysis;
using WebApplication1.Utils;

namespace WebApplication1.CommandHandlers
{
    //[KnownType(typeof(COR_Sales_Temperature))]
    public class CorrelationCommand : CommandBase, ICmdField_SalesIds
    {
        public Guid EstablishmentId { get; set; }
        public List<Guid> SalesIds { get; set; }
        public DateTimePeriod TimePeriod { get; set; }
        public TimeResolution TimeResolution { get; set; }
    }

    public class CorrelationReturn : ReturnBase
    {
        public Dictionary<TimeSpan, double> LagAndCorrelation { get; set; }
        public List<(Guid saleId, double value)> metadata { get; set; }
    }


    public class CorrelationHandler : HandlerBase<CorrelationCommand, CorrelationReturn>
    {
        private IWeatherApi weatherApi;
        private IUserContextService userContextService;
        private IEstablishmentRepository establishmentRepository;
        private ISalesRepository salesRepository;

        public CorrelationHandler(IUserContextService userContextService, IEstablishmentRepository establishmentRepository, ISalesRepository salesRepository)
        {
            this.weatherApi = new DmiWeatherApi();
            this.userContextService = userContextService;
            this.establishmentRepository = establishmentRepository;
            this.salesRepository = salesRepository;
        }

        public override async Task<CorrelationReturn> Handle(CorrelationCommand command)
        {
            Establishment establishment = this.userContextService.GetActiveEstablishment();
            Coordinates coordinates = new Coordinates() { Latitude = 55.676098, Longitude = 12.568337 };

            //Get sales data
            IEnumerable<Sale> sales = this.establishmentRepository.GetEstablishmentSales(establishment.Id);

            var dateTimeList = TimeHelper.CreateTimelineAsList(command.TimePeriod, command.TimeResolution);

            Dictionary<DateTime, List<Sale>> salesOverTimeline = TimeHelper.MapObjectsToTimeline<Sale>(sales, x => x.GetTimeOfSale(), dateTimeList, command.TimeResolution);

            Dictionary<DateTime, int> numberOfSalesForTheDateTimeKey = salesOverTimeline
                .ToDictionary(kv => kv.Key, kv => kv.Value.Count);

            List<(DateTime, double)> listOfDateTimeAndCounts = numberOfSalesForTheDateTimeKey
                .Select(kv => (kv.Key, (double)kv.Value))
                .ToList();

            //Get weather data
            var weatherDataStart = command.TimePeriod.Start.Date;
            var weatherDataEnd = command.TimePeriod.End.Date.AddDays(1).AddTicks(-1);
            List<(DateTime, double)> temperaturePerHour = this.weatherApi.GetMeanTemperaturePerHour(coordinates, command.TimePeriod.Start, command.TimePeriod.End).Result;
            Dictionary<DateTime, List<(DateTime, double)>> tempMappedToTimeline = TimeHelper.MapObjectsToTimeline(temperaturePerHour, x => x.Item1, dateTimeList, command.TimeResolution);

            Dictionary<DateTime, double> averages = tempMappedToTimeline.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(tuple => tuple.Item2).DefaultIfEmpty(0).Average()
            );

            var averageToList = averages
                .Select(kv => (kv.Key, (double)kv.Value))
                .ToList();

            var spearman = CrossCorrelation.DoAnalysis(listOfDateTimeAndCounts, averageToList);

            var spearmanAsDictionary = spearman.ToDictionary(x => x.Item1, x => x.Item2);

            return new CorrelationReturn { LagAndCorrelation = spearmanAsDictionary };
        }
    }
}
