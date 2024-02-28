using WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.Weather;
using WebApplication1.Application_Layer.Handlers.Correlation;
using WebApplication1.Application_Layer.Handlers.SalesHandlers;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services.Analysis;
using WebApplication1.Utils;

namespace WebApplication1.CommandHandlers
{
    public class Correlation_NumberOfSales_Vs_Temperature_Command : CorrelationCommand
    {
    }

    public class Correlation_NumberOfSales_Vs_Temperature_Handler : HandlerBase<Correlation_NumberOfSales_Vs_Temperature_Command, CorrelationReturn>
    {
        private IHandler<GetSalesCommand, GetSalesRawReturn> getSalesHandler;
        private IHandler<GetTemperatureCommand, GetWeatherReturn> getWeatherHandler;

        public Correlation_NumberOfSales_Vs_Temperature_Handler(IHandler<GetSalesCommand, GetSalesRawReturn> getSalesHandler, IHandler<GetTemperatureCommand, GetWeatherReturn> getWeatherHandler)
        {
            this.getSalesHandler = getSalesHandler;
            this.getWeatherHandler = getWeatherHandler;
        }

        public override async Task<CorrelationReturn> Handle(Correlation_NumberOfSales_Vs_Temperature_Command command)
        {
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
            List<(DateTime, double)> temperatureList = (await this.getWeatherHandler.Handle(getTemperatureCommand)).data;


            //Arrange sales
            List<(DateTime, List<Sale>)> salesTimeline = TimeHelper.MapObjectsToTimelineV4(sales, x => x.GetTimeOfSale(), command.TimePeriod.Start, command.TimePeriod.End, command.TimeResolution);

            List<(DateTime, double)> numberOfSales = salesTimeline.Select(x => (x.Item1, (double)x.Item2.Count)).ToList();


            //Act
            List<(TimeSpan, double)> spearman = CrossCorrelation.DoAnalysis(numberOfSales.OrderBy(x => x.Item1).ToList(), temperatureList.OrderBy(x => x.Item1).ToList());

            //RETURN
            var spearmanWithLag = spearman.Select(x => (TimeHelper.FromTimeSpanToHours(x.Item1), x.Item2)).ToList();

            return new CorrelationReturn(spearmanWithLag, numberOfSales, temperatureList);
        }
    }
}
