using DMIOpenData;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services.Analysis;
using WebApplication1.Utils;

namespace WebApplication1.CommandHandlers
{
    public class CorrelationCommand : CommandBase, ICmdField_EstablishmentId, ICmdField_SalesIds
    {
        public Guid EstablishmentId { get; set; }
        public List<Guid> SalesIds { get; set; }
        public SalesSorting salesSorting { get; set; }
        public DateTimePeriod TimePeriod { get; set; }
        public TimeResolution TimeResolution { get; set; }
        public int MaxLag { get; set; }
    }

    public class CorrelationReturn : ReturnBase
    {
        public List<(int, double)> LagAndCorrelation { get; set; }
        public List<(DateTime id, List<double?> values)> calculationValues { get; set; }

        public CorrelationReturn(
            Dictionary<TimeSpan, double> lagAndCorrelation,
        Dictionary<DateTime, (double?, double?)> calculationValues
            )
        {
            this.LagAndCorrelation = lagAndCorrelation.Select(x => (TimeHelper.FromTimeSpanToHours(x.Key, TimeResolution.Hour), x.Value)).ToList();
            this.calculationValues = calculationValues.Select(kv => (id: kv.Key, values: new List<double?> { kv.Value.Item1, kv.Value.Item2 }))
            .ToList();
        }
    }


    public class CorrelationHandler : HandlerBase<CorrelationCommand, CorrelationReturn>
    {
        private IUnitOfWork unitOfWork;
        private IWeatherApi weatherApi;

        public CorrelationHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.weatherApi = new DmiWeatherApi();
        }

        public override async Task<CorrelationReturn> Handle(CorrelationCommand command)
        {
            Establishment establishment = this.unitOfWork.establishmentRepository.IncludeSales().GetById(command.EstablishmentId)!;

            Coordinates coordinates = new Coordinates() { Latitude = 55.676098, Longitude = 12.568337 };
            //FETCH and ARRANGE sales   
            List<Sale> sales = establishment.GetSales().Where(x => command.SalesIds.Any(y => y == x.Id)).ToList();


            var dateTimeList = TimeHelper.CreateTimelineAsList(command.TimePeriod, command.TimeResolution);

            Dictionary<DateTime, List<Sale>> salesOverTimeline = TimeHelper.MapObjectsToTimeline(sales, x => x.GetTimeOfSale(), dateTimeList, command.TimeResolution);

            Dictionary<DateTime, double> numberOfSalesForTheDateTimeKey = salesOverTimeline
                .ToDictionary(kv => kv.Key, kv => (double)kv.Value.Count);


            //FETCH and ARRANGE weather
            var weatherDataStart = command.TimePeriod.Start.Date.AddDays(0);
            var weatherDataEnd = command.TimePeriod.End.Date.AddDays(2);
            List<(DateTime, double)> temperaturePerHour = await this.weatherApi.GetTemperature(coordinates, weatherDataStart, weatherDataEnd, command.TimeResolution);
            var weatherDateTimeList = TimeHelper.CreateTimelineAsList(new DateTimePeriod(weatherDataStart, weatherDataEnd), command.TimeResolution);

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

            Dictionary<TimeSpan, double> spearmanAsDictionary = spearman.ToDictionary(x => x.Item1, x => x.Item2);

            //return
            Dictionary<DateTime, (double?, double?)> combinedDictionary = numberOfSalesForTheDateTimeKey
                .Select(kv =>
                {
                    double? valueSales = kv.Value;
                    double? valueAverages = averages.TryGetValue(kv.Key, out double average) ? (double?)average : null;
                    return new KeyValuePair<DateTime, (double?, double?)>(kv.Key, (valueSales, valueAverages));
                })
                .Concat(averages
                    .Where(kv => !numberOfSalesForTheDateTimeKey.ContainsKey(kv.Key))
                    .Select(kv => new KeyValuePair<DateTime, (double?, double?)>(kv.Key, (null, (double?)kv.Value))))
                .ToDictionary(kv => kv.Key, kv => kv.Value);


            return new CorrelationReturn(spearmanAsDictionary, combinedDictionary);

        }


        //private static List<(DateTime Key, double Value)> ShiftDateTimeValues(List<(DateTime Key, double Value)> originalList, TimeSpan shiftAmount)
        //{
        //    List<(DateTime Key, double Value)> shiftedList = new List<(DateTime Key, double Value)>();

        //    foreach (var tuple in originalList)
        //    {
        //        DateTime shiftedDateTime = tuple.Key + shiftAmount;
        //        shiftedList.Add((shiftedDateTime, tuple.Value));
        //    }

        //    return shiftedList;
        //}
    }
}
