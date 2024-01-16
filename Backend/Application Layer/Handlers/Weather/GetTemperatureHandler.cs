using DMIOpenData;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Services;
using WebApplication1.Utils;

namespace WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.Weather
{


    public class GetTemperatureCommand : CommandBase
    {
        public Guid? EstablishmentId { get; set; }
        public TimeResolution TimeResolution { get; set; }
        public DateTimePeriod DateTimePeriod { get; set; }
    }

    public class GetTemperatureReturn : ReturnBase
    {
        public Dictionary<DateTime, double> data { get; set; }
    }


    public class GetTemperatureHandler : HandlerBase<GetTemperatureCommand, GetTemperatureReturn>
    {
        private IUserContextService contextService;
        private IWeatherApi weatherApi;

        public GetTemperatureHandler(IUserContextService contextService, IWeatherApi weatherApi)
        {
            this.contextService = contextService;
            this.weatherApi = weatherApi;
        }

        public override GetTemperatureReturn Handle(GetTemperatureCommand command)
        {
            ////Arrange
            //Establishment establisment = command.EstablishmentId != null ? this.contextService.TrySetActiveEstablishment((Guid)command.EstablishmentId) : this.contextService.GetActiveEstablishment();

            //List<(DateTime, double)> temperaturePerHour = await this.weatherApi.GetMeanTemperaturePerHour(establisment.Information.Location.Coordinates, command.DateTimePeriod.Start, command.DateTimePeriod.End);

            ////Return the data accourding to the time resolution
            //List<DateTime> timeline = TimeHelper.CreateTimelineAsList(command.DateTimePeriod, command.TimeResolution);

            //Dictionary<DateTime, List<(DateTime, double)>> grouped = TimeHelper.MapObjectsToTimeline(temperaturePerHour, x => x.Item1, timeline, command.TimeResolution);

            //Dictionary<DateTime, double> avergage = grouped.ToDictionary(x => x.Key, x => x.Value.Average(y => y.Item2));

            //return new GetTemperatureReturn { data = avergage };
            throw new NotImplementedException();
        }
    }
}
