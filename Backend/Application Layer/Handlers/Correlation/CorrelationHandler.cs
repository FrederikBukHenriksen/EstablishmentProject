using DMIOpenData;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services.Analysis;
using WebApplication1.Utils;

namespace WebApplication1.CommandHandlers
{
    public class CorrelationCommand : CommandBase, ICmdField_EstablishmentId
    {
        public Guid EstablishmentId { get; set; }
        public List<Guid> SalesIds { get; set; }
        public DateTimePeriod TimePeriod { get; set; }
        public TimeResolution TimeResolution { get; set; }
        public int UpperLag { get; set; }
        public int LowerLag { get; set; }

    }

    public class CorrelationReturn : ReturnBase
    {
        public List<(int, double)> LagAndCorrelation { get; set; }
        public List<(DateTime dateTime, List<double?> values)> calculationValues { get; set; }

        public CorrelationReturn(
            Dictionary<int, double> lagAndCorrelation,
        Dictionary<DateTime, (double?, double?)> calculationValues
            )
        {
            this.LagAndCorrelation = lagAndCorrelation.Select(x => (x.Key, x.Value)).OrderBy(x => x.Key).ToList();
            this.calculationValues = calculationValues.Select(kv => (dateTime: kv.Key, values: new List<double?> { kv.Value.Item1, kv.Value.Item2 }))
            .OrderBy(x => x.dateTime).ToList();
        }
    }


    public class CorrelationHandler : HandlerBase<CorrelationCommand, CorrelationReturn>
    {
        private IUnitOfWork unitOfWork;
        private IWeather weatherApi;

        public CorrelationHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.weatherApi = new DmiWeather();
        }

        public override async Task<CorrelationReturn> Handle(CorrelationCommand command)
        {
            Establishment establishment = this.unitOfWork.establishmentRepository.IncludeSales().GetById(command.EstablishmentId)!;

            Coordinates coordinates = new Coordinates(55.676098, 12.568337);
            //FETCH and ARRANGE sales   
            List<Sale> sales = establishment.GetSales().Where(x => command.SalesIds.Contains(x.Id)).ToList();


            var dateTimeList = TimeHelper.CreateTimelineAsList(command.TimePeriod.Start, command.TimePeriod.End, command.TimeResolution);

            Dictionary<DateTime, List<Sale>> salesOverTimeline = TimeHelper.MapObjectsToTimeline(sales, x => x.GetTimeOfSale(), dateTimeList, command.TimeResolution);

            Dictionary<DateTime, double> numberOfSalesForTheDateTimeKey = salesOverTimeline
                .ToDictionary(kv => kv.Key, kv => (double)kv.Value.Count);


            //FETCH and ARRANGE weather
            var lagDateStart = command.TimePeriod.Start.AddToDateTime(command.LowerLag * (-1), command.TimeResolution);
            var lagDateEnd = command.TimePeriod.End.AddToDateTime(command.UpperLag, command.TimeResolution);
            List<(DateTime, double)> temperaturePerHour = await this.weatherApi.GetMeanTemperature(coordinates, lagDateStart, lagDateEnd, command.TimeResolution);
            var weatherDateTimeList = TimeHelper.CreateTimelineAsList(lagDateStart, lagDateEnd, command.TimeResolution);

            Dictionary<DateTime, List<(DateTime, double)>> tempMappedToTimeline = TimeHelper.MapObjectsToTimeline(temperaturePerHour, x => x.Item1, weatherDateTimeList, command.TimeResolution);

            Dictionary<DateTime, double> averages = tempMappedToTimeline.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(tuple => tuple.Item2).DefaultIfEmpty(0).Average()
            );

            //ACT

            List<(DateTime, double)> listOfDateTimeAndCounts = numberOfSalesForTheDateTimeKey
                .Select(kv => (kv.Key, (double)kv.Value))
                .ToList();

            List<(DateTime Key, double)> averageToList = averages
                .Select(kv => (kv.Key, (double)kv.Value))
                .ToList();

            List<(TimeSpan, double)> spearman = CrossCorrelation.DoAnalysis(listOfDateTimeAndCounts, averageToList);



            //RETURN

            Dictionary<TimeSpan, double> spearmanAsDictionary = spearman.ToDictionary(x => x.Item1, x => x.Item2);
            Dictionary<int, double> spearmanWithLagAsDictionary = spearman.ToDictionary(x => TimeHelper.FromTimeSpanToHours(x.Item1), x => x.Item2);

            List<Sale> salesWithinLag = sales.Where(x => x.GetTimeOfSale() >= lagDateStart && x.GetTimeOfSale() <= lagDateEnd).ToList();
            Dictionary<DateTime, double> salesWithinLagNumberOfSales = salesWithinLag
                .GroupBy(x => TimeHelper.TimeResolutionUniqueRounder(x.GetTimeOfSale(), command.TimeResolution))
                .ToDictionary(x => x.Key, x => (double)x.Count());

            Dictionary<DateTime, (double?, double?)> combinedDictionary = salesWithinLagNumberOfSales
                .Select(kv =>
                {
                    double? valueSales = kv.Value;
                    double? valueAverages = averages.TryGetValue(kv.Key, out double average) ? (double?)average : null;
                    return new KeyValuePair<DateTime, (double?, double?)>(kv.Key, (valueSales, valueAverages));
                })
                .Concat(averages
                    .Where(kv => !salesWithinLagNumberOfSales.ContainsKey(kv.Key))
                    .Select(kv => new KeyValuePair<DateTime, (double?, double?)>(kv.Key, (null, (double?)kv.Value))))
                .ToDictionary(kv => kv.Key, kv => kv.Value);


            return new CorrelationReturn(spearmanWithLagAsDictionary, combinedDictionary);

        }
    }
}
