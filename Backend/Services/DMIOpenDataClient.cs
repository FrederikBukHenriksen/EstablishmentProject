using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using WebApplication1.Migrations;
using WebApplication1.Services;

namespace DMIOpenData
{
    public interface WeatherAPI
    {
        public List<(DateTime, double)> getTemperature(DateTime startTime, DateTime endTime);
    }

    public class DMIOpenDataClient
    {
        private const string BaseUrl = "https://dmigw.govcloud.dk/v2/{api}";
        private const string BaseUrlStation = "https://dmigw.govcloud.dk/v2/{api}";
        private const string BaseUrlData = "https://dmigw.govcloud.dk/v2/{api}";


        private readonly string apiKey = "ff666551-985c-4533-970f-96f3ef50036e";



        public DMIOpenDataClient()
        {
        }

        private string BaseUrlMethod(string api)
        {
            return BaseUrl.Replace("{api}", api);
        }
        //Task<Dictionary<string, dynamic>>
        private async void QueryAsync(string api, string service, Dictionary<string, dynamic> parameters)
        {
            using (HttpClient client = new HttpClient())
            {

                var url = $"{BaseUrlMethod(api)}/{service}?api-key={apiKey}&{string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))}";
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string jsonString = await response.Content.ReadAsStringAsync();
                Root root = JsonConvert.DeserializeObject<Root>(jsonString);

                var featuresWithTemperature = root.features.Where(x => x.properties.parameterId.Any(x => x == "weather")).ToList(); //temp_dry
                double myLatitude = 55.7635;
                double myLongitude = 12.4949;

                var rootClosestToMyCoordinates = featuresWithTemperature.OrderBy(x => EuclideanDistance(x.geometry.coordinates[0], x.geometry.coordinates[1], myLongitude, myLatitude)).First();


                Console.WriteLine(jsonString);
                Console.WriteLine(root);
                Console.WriteLine(featuresWithTemperature);



                //return await response.Content.ReadAsAsync<Dictionary<string, dynamic>>();
                }
        }

        public async Task<string> GetIdOfClosestStation()
        {
            var parameters = new Dictionary<string, dynamic>
            {
                { "limit", 100000 },
                { "offset", 0 }
            };

            using (HttpClient client = new HttpClient())
            {
                var url = $"{BaseUrlMethod("metObs")}/{"collections/station/items"}?api-key={apiKey}&{string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))}";
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string jsonString = await response.Content.ReadAsStringAsync();
                Root root = JsonConvert.DeserializeObject<Root>(jsonString);

                var featuresWithTemperature = root.features.Where(x => x.properties.parameterId.Any(x => x == "weather")).ToList(); //temp_dry
                double myLatitude = 55.7635;
                double myLongitude = 12.4949;

                var rootClosestToMyCoordinates = featuresWithTemperature.OrderBy(x => EuclideanDistance(x.geometry.coordinates[0], x.geometry.coordinates[1], myLongitude, myLatitude)).First();
                return rootClosestToMyCoordinates.properties.stationId;
            }
        }



        //Task<List<Dictionary<string, dynamic>>> 
        public async void GetStationsAsync()
        {

            //var result = await QueryAsync("metObs", "collections/station/items", parameters);
            var ok = GetIdOfClosestStation();
            //return result.ContainsKey("features") ? result["features"] : new List<Dictionary<string, dynamic>>();
        }

        public async void GetDataAsync(int limit = 10, int offset = 0)
        {
            var parameters = new Dictionary<string, dynamic>
            {
                { "limit", limit },
                { "offset", offset },
                { "stationId", "06181" }, //"06181" = Jægersborg
                { "parameterId", "weather" },

            };
            //var result = await QueryAsync("metObs", "collections/station/items", parameters);
            QueryAsync("metObs", "collections/observation/items", parameters);
            //return result.ContainsKey("features") ? result["features"] : new List<Dictionary<string, dynamic>>();
        }

        public static string ConstructDateTimeArgument(DateTime? fromTime = null, DateTime? toTime = null)
        {
            if (fromTime == null && toTime == null)
            {
                return null;
            }

            if (fromTime != null && toTime == null)
            {
                return $"{fromTime.Value.ToString("s")}Z";
            }

            if (fromTime == null && toTime != null)
            {
                return $"{toTime.Value.ToString("s")}Z";
            }

            return $"{fromTime.Value.ToString("s")}Z/{toTime.Value.ToString("s")}Z";
        }
        private double EuclideanDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }


        
    }
}