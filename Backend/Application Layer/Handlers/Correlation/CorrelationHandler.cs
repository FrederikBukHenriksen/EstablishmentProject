using DMIOpenData;
using WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.Weather;
using WebApplication1.Application_Layer.Handlers.SalesHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services.Analysis;
using WebApplication1.Utils;

namespace WebApplication1.CommandHandlers
{
    public class CorrelationCommand : CommandBase, ICmdField_SalesIds
    {
        public Guid EstablishmentId { get; set; }
        public List<Guid> SalesIds { get; set; }
        public DateTimePeriod TimePeriod { get; set; }
        public Coordinates Coordinates { get; set; }
        public TimeResolution TimeResolution { get; set; }
        public int UpperLag { get; set; }
        public int LowerLag { get; set; }

    }

    public class CorrelationReturn : ReturnBase
    {
        public List<(int, double)> LagAndCorrelation { get; set; }
        public List<(DateTime dateTime, List<double?> values)> calculationValues { get; set; }



        public CorrelationReturn(
            List<(int, double)> lagAndCorrelation,
            List<(DateTime, double)> referenceList,
            List<(DateTime, double)> shiftingList

        )
        {
            this.LagAndCorrelation = lagAndCorrelation.OrderBy(x => x.Item1).ToList();
            this.calculationValues = this.MergeLists(referenceList, shiftingList).OrderBy(x => x.Item1).ToList();
        }

        //public CorrelationReturn(
        //    Dictionary<int, double> lagAndCorrelation,
        //    Dictionary<DateTime, (double?, double?)> calculationValues
        //)
        //{
        //    this.LagAndCorrelation = lagAndCorrelation.Select(x => (x.Key, x.Value)).OrderBy(x => x.Key).ToList();
        //    this.calculationValues = calculationValues.Select(kv => (dateTime: kv.Key, values: new List<double?> { kv.Value.Item1, kv.Value.Item2 }))
        //    .OrderBy(x => x.dateTime).ToList();
        //}

        private List<(DateTime, List<double?>)> MergeLists(List<(DateTime, double)> list1, List<(DateTime, double)> list2)
        {
            var mergedList = new List<(DateTime, List<double?>)>();

            var allDates = list1.Select(x => x.Item1).Union(list2.Select(x => x.Item1)).Distinct();

            foreach (var date in allDates)
            {
                var values1 = list1.Where(x => x.Item1 == date).Select(x => (double?)x.Item2).ToList();
                var values2 = list2.Where(x => x.Item1 == date).Select(x => (double?)x.Item2).ToList();

                mergedList.Add((date, values1.Concat(values2).ToList()));
            }
            return mergedList;
        }
    }

    public class CorrelationHandler : HandlerBase<CorrelationCommand, CorrelationReturn>
    {
        private IHandler<GetSalesCommand, GetSalesRawReturn> getSalesHandler;
        private IHandler<GetTemperatureCommand, GetWeatherReturn> getWeatherHandler;

        public CorrelationHandler(IHandler<GetSalesCommand, GetSalesRawReturn> getSalesHandler, IHandler<GetTemperatureCommand, GetWeatherReturn> getWeatherHandler)
        {
            this.getSalesHandler = getSalesHandler;
            this.getWeatherHandler = getWeatherHandler;
        }

        public override async Task<CorrelationReturn> Handle(CorrelationCommand command)
        {
            //Hard coded coordintes for now
            command.Coordinates = new Coordinates(55.676098, 12.568337);

            //Fetch sales
            GetSalesCommand getSalesCommand = new GetSalesCommand { EstablishmentId = command.EstablishmentId, SalesIds = command.SalesIds };
            List<Sale> sales = (await this.getSalesHandler.Handle(getSalesCommand)).Sales;

            //Fecth weather
            var lagDateStart = command.TimePeriod.Start.AddToDateTime(command.LowerLag * (-1), command.TimeResolution);
            var lagDateEnd = command.TimePeriod.End.AddToDateTime(command.UpperLag, command.TimeResolution);
            GetTemperatureCommand getTemperatureCommand = new GetTemperatureCommand
            {
                coordinates = command.Coordinates,
                start = lagDateStart,
                end = lagDateEnd,
                TimeResolution = command.TimeResolution
            };
            List<(DateTime, double)> temperaturePerHour = (await this.getWeatherHandler.Handle(getTemperatureCommand)).data;


            //Arrange sales
            var timeline = TimeHelper.CreateTimelineAsListV2(command.TimePeriod.Start, command.TimePeriod.End, command.TimeResolution);
            Dictionary<DateTime, List<Sale>> salesTimeline = TimeHelper.MapObjectsToTimelineV2(sales, x => x.GetTimeOfSale(), timeline, command.TimeResolution);

            List<(DateTime, double)> listOfDateTimeAndCounts = salesTimeline
                .Select(kv => (kv.Key, (double)kv.Value.Count)).OrderBy(x => x.Key)
                .ToList();

            //Act
            List<(TimeSpan, double)> spearman = CrossCorrelation.DoAnalysis(listOfDateTimeAndCounts.OrderBy(x => x.Item1).ToList(), temperaturePerHour.OrderBy(x => x.Item1).ToList());

            //RETURN
            var spearmanWithLag = spearman.Select(x => (TimeHelper.FromTimeSpanToHours(x.Item1), x.Item2)).ToList();

            return new CorrelationReturn(spearmanWithLag, listOfDateTimeAndCounts, temperaturePerHour);



            //List<Sale> sales = establishment.GetSales().Where(x => command.SalesIds.Contains(x.Id)).ToList();


            //var dateTimeList = TimeHelper.CreateTimelineAsList(command.TimePeriod.Start, command.TimePeriod.End, command.TimeResolution);

            //Dictionary<DateTime, List<Sale>> salesOverTimeline = TimeHelper.MapObjectsToTimeline(sales, x => x.GetTimeOfSale(), dateTimeList, command.TimeResolution);

            //Dictionary<DateTime, double> numberOfSalesForTheDateTimeKey = salesOverTimeline
            //    .ToDictionary(kv => kv.Key, kv => (double)kv.Value.Count);


            ////FETCH and ARRANGE weather
            //var lagDateStart = command.TimePeriod.Start.AddToDateTime(command.LowerLag * (-1), command.TimeResolution);
            //var lagDateEnd = command.TimePeriod.End.AddToDateTime(command.UpperLag, command.TimeResolution);
            //List<(DateTime, double)> temperaturePerHour = await this.weatherApi.GetMeanTemperature(command.Coordinates, lagDateStart, lagDateEnd, command.TimeResolution);
            //var weatherDateTimeList = TimeHelper.CreateTimelineAsList(lagDateStart, lagDateEnd, command.TimeResolution);

            //Dictionary<DateTime, List<(DateTime, double)>> tempMappedToTimeline = TimeHelper.MapObjectsToTimeline(temperaturePerHour, x => x.Item1, weatherDateTimeList, command.TimeResolution);

            //Dictionary<DateTime, double> averages = tempMappedToTimeline.ToDictionary(
            //    kvp => kvp.Key,
            //    kvp => kvp.Value.Select(tuple => tuple.Item2).DefaultIfEmpty(0).Average()
            //);

            ////ACT

            //List<(DateTime, double)> listOfDateTimeAndCounts = numberOfSalesForTheDateTimeKey
            //    .Select(kv => (kv.Key, (double)kv.Value))
            //    .ToList();

            //List<(DateTime Key, double)> averageToList = averages
            //    .Select(kv => (kv.Key, (double)kv.Value))
            //    .ToList();

            //List<(TimeSpan, double)> spearman = CrossCorrelation.DoAnalysis(listOfDateTimeAndCounts, averageToList);





            //RETURN

            //Dictionary<TimeSpan, double> spearmanAsDictionary = spearman.ToDictionary(x => x.Item1, x => x.Item2);
            //Dictionary<int, double> spearmanWithLagAsDictionary = spearman.ToDictionary(x => TimeHelper.FromTimeSpanToHours(x.Item1), x => x.Item2);

            //List<Sale> salesWithinLag = sales.Where(x => x.GetTimeOfSale() >= lagDateStart && x.GetTimeOfSale() <= lagDateEnd).ToList();
            //Dictionary<DateTime, double> salesWithinLagNumberOfSales = salesWithinLag
            //    .GroupBy(x => TimeHelper.TimeResolutionUniqueRounder(x.GetTimeOfSale(), command.TimeResolution))
            //    .ToDictionary(x => x.Key, x => (double)x.Count());

            //Dictionary<DateTime, (double?, double?)> combinedDictionary = salesWithinLagNumberOfSales
            //    .Select(kv =>
            //    {
            //        double? valueSales = kv.Value;
            //        double? valueAverages = averages.TryGetValue(kv.Key, out double average) ? (double?)average : null;
            //        return new KeyValuePair<DateTime, (double?, double?)>(kv.Key, (valueSales, valueAverages));
            //    })
            //    .Concat(averages
            //        .Where(kv => !salesWithinLagNumberOfSales.ContainsKey(kv.Key))
            //        .Select(kv => new KeyValuePair<DateTime, (double?, double?)>(kv.Key, (null, (double?)kv.Value))))
            //    .ToDictionary(kv => kv.Key, kv => kv.Value);


            //return new CorrelationReturn(spearmanWithLagAsDictionary, combinedDictionary);

        }
    }
}
