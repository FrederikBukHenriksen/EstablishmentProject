using DMIOpenData;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Services;
using WebApplication1.Services.Analysis;

namespace WebApplication1.CommandHandlers
{

    public class CorrelationBetweenSalesAndWeatherCommand : CommandBase
    {
        //public Guid ItemId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CorrelationBetweenSalesAndWeatherReturn : ReturnBase
    {
        public List<(TimeSpan, double)> correlation { get; set; }
    }


    public class CorrelationBetweenSoldItemsAndWeatherCommandHandler : HandlerBase<CorrelationBetweenSalesAndWeatherCommand, CorrelationBetweenSalesAndWeatherReturn>
    {
        private IWeatherApi weatherApi;
        private IUserContextService userContextService;
        private ISalesRepository salesRepository;

        public CorrelationBetweenSoldItemsAndWeatherCommandHandler(IUserContextService userContextService, ISalesRepository salesRepository)
        {
            this.weatherApi = new DmiWeatherApi();
            this.userContextService = userContextService;
            this.salesRepository = salesRepository;
        }

        public override CorrelationBetweenSalesAndWeatherReturn Handle(CorrelationBetweenSalesAndWeatherCommand command)
        {
            Establishment establishment = this.userContextService.GetActiveEstablishment();
            //Location location = establishment.Location;
            //Coordinates coordinates = location.Coordinates;
            Coordinates coordinates = new Coordinates() { Latitude = 55.676098, Longitude = 12.568337 };

            //Get sales data
            IEnumerable<Sale> sales = salesRepository.GetAll().Where(x => x.Establishment == establishment);
            IEnumerable<Sale> salesWithTimespan = sales.Where(x => x.TimestampArrival >= command.StartDate && x.TimestampArrival <= command.EndDate);

            IEnumerable<IGrouping<int, Sale>> salesGroupedByHour = salesWithTimespan.GroupBy(x => x.TimestampPayment.Hour);
            List<(DateTime, double)> numberOfSalesPerHour = salesGroupedByHour.Select(x => (x.First().TimestampPayment, (double)x.Count())).ToList();

            //Get weather data
            var weatherDataStart = command.StartDate.Date;
            var weatherDataEnd = command.EndDate.Date.AddDays(1).AddTicks(-1);
            List<(DateTime, double)> temperaturePerHour = weatherApi.GetMeanTemperaturePerHour(coordinates, command.StartDate, command.EndDate).Result;

            var spearman = CrossCorrelation.DoAnalysis(numberOfSalesPerHour, temperaturePerHour);
            //var largestSpearman = spearman.OrderByDescending(x => Math.Abs(x.Item2)).First();

            return new CorrelationBetweenSalesAndWeatherReturn();
        }
    }
}
