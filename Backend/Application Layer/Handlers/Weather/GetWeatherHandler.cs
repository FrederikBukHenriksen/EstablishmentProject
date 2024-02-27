using DMIOpenData;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Utils;

namespace WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.Weather
{


    public class GetTemperatureCommand : CommandBase
    {
        public Coordinates coordinates { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }

        public TimeResolution TimeResolution { get; set; }
    }

    public class GetWeatherReturn : ReturnBase
    {
        public List<(DateTime, double)> data { get; set; } = new List<(DateTime, double)>();
    }

    public class GetWeatherHandler : HandlerBase<GetTemperatureCommand, GetWeatherReturn>
    {
        private IWeather weatherApi;

        public GetWeatherHandler(IWeather weatherApi)
        {
            this.weatherApi = weatherApi;
        }

        public override async Task<GetWeatherReturn> Handle(GetTemperatureCommand command)
        {
            List<(DateTime, double)> temperaturePerTimeResolution = await this.weatherApi.GetMeanTemperature(command.coordinates, command.start, command.end, command.TimeResolution);
            return new GetWeatherReturn()
            {
                data = temperaturePerTimeResolution
            };
        }
    }
}
