using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Reflection;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Utils;
using static WebApplication1.Services.DmiWeatherApiItems;

namespace DMIOpenData
{
    public enum WeatherParameter
    {
        [Description("temp_mean_past1h")]
        Temperature,
    }

    public interface IWeatherApi
    {
        Task<List<(DateTime, double)>> GetMeanTemperature(Coordinates coordinates, DateTime startTime, DateTime endTime, TimeResolution timeresolution);
    }

    public class DmiWeatherApi : IWeatherApi
    {
        private const string BaseUrlData = "https://dmigw.govcloud.dk/v2/metObs/collections/observation/items";
        private const string BaseUrlStation = "https://dmigw.govcloud.dk/v2/metObs/collections/station/items";
        private readonly string apiKey = "ff666551-985c-4533-970f-96f3ef50036e";
        private readonly HttpClient httpClient;

        public DmiWeatherApi(HttpClient httpClient = null)
        {
            this.httpClient = httpClient ?? new HttpClient();
        }

        private async Task<string> GetIdOfClosestStation(Coordinates coordinates, WeatherParameter weatherParameter)
        {
            var parameters = new Dictionary<string, dynamic>
            {
                { "limit", 100000 },
                { "parameterId", weatherParameter.GetDescription() },
            };

            var url = $"{BaseUrlStation}?api-key={this.apiKey}&{string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))}";
            var response = await this.httpClient.GetAsync(url);
            string jsonString = await response.Content.ReadAsStringAsync();
            Root root = JsonConvert.DeserializeObject<Root>(jsonString);

            var featuresWithTemperature = root.features.Where(x => x.properties.parameterId.Any(x => x == "temp_mean_past1h")).ToList(); //temp_dry, weather

            var rootClosestToMyCoordinates = featuresWithTemperature.OrderBy(x => this.EuclideanDistance(x.geometry.coordinates[0], x.geometry.coordinates[1], coordinates.longitude, coordinates.latitude)).First();
            return rootClosestToMyCoordinates.properties.stationId;
        }

        public async Task<List<(DateTime, double)>> GetMeanTemperature(Coordinates coordinates, DateTime startTime, DateTime endTime, TimeResolution timeresolution)
        {
            var weatherParameter = WeatherParameter.Temperature;
            if (startTime > endTime)
            {
                throw new ArgumentException("the end time must be later than the start time");
            }

            string stationId = await this.GetIdOfClosestStation(coordinates, weatherParameter);

            var parameters = new Dictionary<string, dynamic>
            {
                { "limit", 100000 },
                { "stationId", stationId }, //"06181" = Jægersborg
                { "datetime", $"{startTime.ToString("s")}Z/{endTime.ToString("s")}Z" },
                { "parameterId", weatherParameter.GetDescription() },
            };

            var url = $"{BaseUrlData}?api-key={this.apiKey}&{string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))}";
            var response = await this.httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string jsonString = await response.Content.ReadAsStringAsync();
            var root = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString);

            JArray featuresArray = (JArray)root["features"];

            List<JObject> listOfJsonObjects = featuresArray.Select(item => JObject.Parse(item.ToString())).ToList();

            List<(DateTime, double)> data = listOfJsonObjects.Select(x => ((DateTime)x["properties"]["observed"], (double)x["properties"]["value"])).OrderBy(x => x.Item1).ToList();

            List<(DateTime, double)> asList = AverageForTimeResolution(startTime, endTime, timeresolution, data);
            return asList;
        }

        private static List<(DateTime, double)> AverageForTimeResolution(DateTime startTime, DateTime endTime, TimeResolution timeresolution, List<(DateTime datetime, double values)> data)
        {
            List<(DateTime datetime, double values)> dataOrdered = data.OrderBy(x => x.Item1).ToList();
            List<DateTime> timeline = TimeHelper.CreateTimelineAsList(startTime, endTime, timeresolution);
            Dictionary<DateTime, List<(DateTime datetime, double values)>> averageTemperaturePerDateTime = TimeHelper.MapObjectsToTimeline(dataOrdered, x => x.datetime, timeline, timeresolution);
            Dictionary<DateTime, double> averagePerTimeResolution = averageTemperaturePerDateTime.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(v => v.values).Average()
            );
            List<(DateTime, double)> asList = averagePerTimeResolution.Select(x => (x.Key, x.Value)).ToList();
            return asList;
        }

        private double EuclideanDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }

    public static class WeatherParameterExtensions
    {
        public static string GetDescription(this WeatherParameter value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field == null) return null;

            DescriptionAttribute attribute = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute));

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
