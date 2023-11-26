using DMIOpenData;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Repositories;
using WebApplication1.Services;
using WebApplication1.Services.Analysis;

namespace WebApplication1.CommandHandlers
{

    public class CorrelationGraphCommand : CommandBase
    {
        //public Guid ItemId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CorrelationGraphReturn
    {
        public GraphDTO PrimaryGraph { get; set; }
        public GraphDTO SecondaryGraph { get; set; }
    }

    public class CorrelationGraphHandler : HandlerBase<CorrelationGraphCommand, CorrelationGraphReturn>
    {
        private IWeatherApi weatherApi;
        private IUserContextService userContextService;
        private ISalesRepository salesRepository;

        public CorrelationGraphHandler(IUserContextService userContextService, ISalesRepository salesRepository)
        {
            this.weatherApi = new DmiWeatherApi();
            this.userContextService = userContextService;
            this.salesRepository = salesRepository;
        }

        public override CorrelationGraphReturn Execute(CorrelationGraphCommand command)
        {
            Establishment establishment = this.userContextService.GetActiveEstablishment();
            Coordinates coordinates = new Coordinates() { Latitude = 55.676098, Longitude = 12.568337 };

            //Get sales data
            IEnumerable<Sale> sales = salesRepository.GetAll().Where(x => x.Establishment == establishment);
            IEnumerable<Sale> salesWithTimespan = sales.Where(x => x.TimestampStart >= command.StartDate && x.TimestampStart <= command.EndDate);

            IEnumerable<IGrouping<int, Sale>> salesGroupedByHour = salesWithTimespan.GroupBy(x => x.TimestampEnd.Hour);
            List<(DateTime, double)> numberOfSalesPerHour = salesGroupedByHour.Select(x => (x.First().TimestampEnd, (double)x.Count())).ToList();

            //Get weather data
            var weatherDataStart = command.StartDate.Date;
            var weatherDataEnd = command.EndDate.Date.AddDays(1).AddTicks(-1);
            List<(DateTime, double)> temperaturePerHour = weatherApi.GetMeanTemperaturePerHour(coordinates, command.StartDate, command.EndDate).Result;

            var spearman = CrossCorrelation.DoAnalysis(numberOfSalesPerHour, temperaturePerHour);

            return new CorrelationGraphReturn { PrimaryGraph = new GraphDTO(), SecondaryGraph = new GraphDTO() };
            
        }
    }
}
