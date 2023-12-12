using DMIOpenData;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Services;
using WebApplication1.Services.Analysis;
using WebApplication1.Utils;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApplication1.CommandHandlers
{

    public abstract class CorrelationCommand : CommandBase
    {
        public DateTimePeriod TimePeriod { get; set; }
        public abstract (List<(DateTime dateTime1, double value1)> variable1, List<(DateTime dateTime2, double value2)> variable2) GetData();
    }

    public class COR_Sales_Temperature : CorrelationCommand
{
        private DmiWeatherApi weatherApi;
        private IUserContextService userContextService;
        private ISalesRepository salesRepository;

        public COR_Sales_Temperature(IUserContextService userContextService, ISalesRepository salesRepository)
        {
            this.weatherApi = new DmiWeatherApi();
            this.userContextService = userContextService;
            this.salesRepository = salesRepository;
        }

        public override (List<(DateTime dateTime1, double value1)> variable1, List<(DateTime dateTime2, double value2)> variable2) GetData()
        {
            //Fetch
            Establishment activeEstablishment = this.userContextService.GetActiveEstablishment();
            IEnumerable<Sale> sales = salesRepository.GetSalesFromEstablishment(activeEstablishment);

            var weatherDataStart = this.TimePeriod.Start.Date;
            var weatherDataEnd = this.TimePeriod.End.Date.AddDays(1).AddTicks(-1);
            List<(DateTime, double)> temperaturePerHour = weatherApi.GetMeanTemperaturePerHour(activeEstablishment.Location.Coordinates, this.TimePeriod.Start, this.TimePeriod.End).Result;

            //Arrange
            IEnumerable<Sale> salesWithTimespan = sales.Where(x => x.TimestampArrival >= this.TimePeriod.Start && x.TimestampArrival <= this.TimePeriod.End);

            IEnumerable<IGrouping<int, Sale>> salesGroupedByHour = salesWithTimespan.GroupBy(x => x.TimestampPayment.Hour);
            List<(DateTime, double)> numberOfSalesPerHour = salesGroupedByHour.Select(x => (x.First().TimestampPayment, (double)x.Count())).ToList();

            //Return
            return (numberOfSalesPerHour, temperaturePerHour);
        }
    }

    public class CorrelationReturn : ReturnBase
    {
        public List<(TimeSpan lag, double correlation)> LagAndCorrelation { get; set; }
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

        public override CorrelationReturn Handle(CorrelationCommand command)
        {
            //Establishment establishment = this.userContextService.GetActiveEstablishment();
            ////Location location = establishment.Location;
            ////Coordinates coordinates = location.Coordinates;
            //Coordinates coordinates = new Coordinates() { Latitude = 55.676098, Longitude = 12.568337 };

            ////Get sales data
            //IEnumerable<Sale> sales = establishmentRepository.GetEstablishmentSales(establishment.Id);
            //IEnumerable<Sale> salesWithTimespan = sales.Where(x => x.TimestampArrival >= command.TimePeriod.Start && x.TimestampArrival <= command.TimePeriod.End);

            //IEnumerable<IGrouping<int, Sale>> salesGroupedByHour = salesWithTimespan.GroupBy(x => x.TimestampPayment.Hour);
            //List<(DateTime, double)> numberOfSalesPerHour = salesGroupedByHour.Select(x => (x.First().TimestampPayment, (double)x.Count())).ToList();

            ////Get weather data
            //var weatherDataStart = command.TimePeriod.Start.Date;
            //var weatherDataEnd = command.TimePeriod.End.Date.AddDays(1).AddTicks(-1);
            //List<(DateTime, double)> temperaturePerHour = weatherApi.GetMeanTemperaturePerHour(coordinates, command.TimePeriod.Start, command.TimePeriod.End).Result;

            var spearman = CrossCorrelation.DoAnalysis(command.GetData().variable1, command.GetData().variable1);
            //var largestSpearman = spearman.OrderByDescending(x => Math.Abs(x.Item2)).First();

            return new CorrelationReturn();
        }
    }
}
