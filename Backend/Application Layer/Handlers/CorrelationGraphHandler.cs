using DMIOpenData;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Services;

namespace WebApplication1.CommandHandlers
{

    public class CorrelationGraphCommand : CommandBase
    {
        //public Guid ItemId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CorrelationGraphReturn : ReturnBase
    {
        public ICollection<(DateTime, double)>? PrimaryGraph { get; set; }
        public ICollection<(DateTime, double)>? SecondaryGraph { get; set; }
    }

    public class CorrelationGraphHandler : HandlerBase<CorrelationGraphCommand, CorrelationGraphReturn>
    {
        private IWeatherApi weatherApi;
        private IUserContextService userContextService;
        private IEstablishmentRepository establishmentRepository;
        private ISalesRepository salesRepository;

        public CorrelationGraphHandler(IUserContextService userContextService, IEstablishmentRepository establishmentRepository, ISalesRepository salesRepository)
        {
            this.weatherApi = new DmiWeatherApi();
            this.userContextService = userContextService;
            this.establishmentRepository = establishmentRepository;
            this.salesRepository = salesRepository;
        }

        public override async Task<CorrelationGraphReturn> Handle(CorrelationGraphCommand command)
        {
            //Establishment establishment = this.userContextService.GetActiveEstablishment();
            //Coordinates coordinates = new Coordinates() { Latitude = 55.676098, Longitude = 12.568337 };

            ////Get sales data
            //IEnumerable<Sale> sales = this.establishmentRepository.GetEstablishmentSales(establishment.Id);

            //IEnumerable<Sale> salesWithTimespan = sales.Where(x => x.TimestampArrival >= command.StartDate && x.TimestampArrival <= command.EndDate);

            //IEnumerable<IGrouping<int, Sale>> salesGroupedByHour = salesWithTimespan.GroupBy(x => x.TimestampPayment.Hour);
            //List<(DateTime, double)> numberOfSalesPerHour = salesGroupedByHour.Select(x => (x.First().TimestampPayment, (double)x.Count())).ToList();

            ////Get weather data
            //var weatherDataStart = command.StartDate.Date;
            //var weatherDataEnd = command.EndDate.Date.AddDays(1).AddTicks(-1);
            //List<(DateTime, double)> temperaturePerHour = this.weatherApi.GetTemperature(coordinates, command.StartDate, command.EndDate).Result;

            //var spearman = CrossCorrelation.DoAnalysis(numberOfSalesPerHour, temperaturePerHour);

            return new CorrelationGraphReturn();

        }
    }
}
