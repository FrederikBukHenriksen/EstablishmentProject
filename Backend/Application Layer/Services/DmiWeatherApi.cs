using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services;
using WebApplication1.Utils;

namespace DMIOpenData
{
    public enum WeatherParamters
    {
        [Description("temp_mean_past1h")]
        Temperature,
    }

    public interface IWeatherApi
    {
        Task<List<(DateTime, double)>> GetTemperature(Coordinates coordinates, DateTime startTime, DateTime endTime, TimeResolution timeresolution);
    }

    public class DmiWeatherApi : IWeatherApi
    {
        private const string BaseUrl = "https://dmigw.govcloud.dk/v2/metObs";
        private const string BaseUrlData = "https://dmigw.govcloud.dk/v2/metObs/collections/observation/items";
        private const string BaseUrlStation = "https://dmigw.govcloud.dk/v2/metObs/collections/station/items";
        private readonly string apiKey = "ff666551-985c-4533-970f-96f3ef50036e";

        private readonly HttpClient httpClient;

        public DmiWeatherApi(HttpClient httpClient = null)
        {
            this.httpClient = httpClient ?? new HttpClient();
        }
        private async Task<string> GetIdOfClosestStation(Coordinates coordinates)
        {
            var parameters = new Dictionary<string, dynamic>
                {
                    { "limit", 100000 },
                };

            var url = $"{BaseUrlStation}?api-key={this.apiKey}&{string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))}";
            var response = await this.httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string jsonString = await response.Content.ReadAsStringAsync();
            Root root = JsonConvert.DeserializeObject<Root>(jsonString);

            var featuresWithTemperature = root.features.Where(x => x.properties.parameterId.Any(x => x == "temp_mean_past1h")).ToList(); //temp_dry, weather

            var rootClosestToMyCoordinates = featuresWithTemperature.OrderBy(x => this.EuclideanDistance(x.geometry.coordinates[0], x.geometry.coordinates[1], coordinates.Longitude, coordinates.Latitude)).First();
            return rootClosestToMyCoordinates.properties.stationId;
        }


        public async Task<List<(DateTime, double)>> GetTemperature(Coordinates coordinates, DateTime startTime, DateTime endTime, TimeResolution timeresolution)
        {
            if (startTime > endTime)
            {
                throw new ArgumentException("the end time must be later than the start time");
            }

            string stationId = await this.GetIdOfClosestStation(coordinates);

            var parameters = new Dictionary<string, dynamic>
                {
                    { "limit", 100000 },
                    { "stationId", stationId }, //"06181" = Jægersborg
                    { "datetime", $"{startTime.ToString("s")}Z/{endTime.ToString("s")}Z" },
                    { "parameterId", "temp_mean_past1h" },

                };

            var url = $"{BaseUrlData}?api-key={this.apiKey}&{string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))}";
            var response = await this.httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string jsonString = await response.Content.ReadAsStringAsync();
            var root = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString);

            JArray featuresArray = (JArray)root["features"];

            List<JObject> listOfJsonObjects = featuresArray.Select(item => JObject.Parse(item.ToString())).ToList();

            List<(DateTime, double)> data = listOfJsonObjects.Select(x => ((DateTime)x["properties"]["observed"], (double)x["properties"]["value"])).OrderBy(x => x.Item1).ToList();

            List<(DateTime datetime, double values)> dataOrdered = data.OrderBy(x => x.Item1).ToList();

            //Account for TimeResolution
            List<DateTime> timeline = TimeHelper.CreateTimelineAsList(new DateTimePeriod(startTime, endTime), timeresolution);
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
}