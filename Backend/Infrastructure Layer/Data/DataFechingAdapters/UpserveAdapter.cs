using Newtonsoft.Json;
using WebApplication1.Application_Layer.Services.DataFetching;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Application_Layer.Services
{
    public class UpserveCredentials
    {

        public UpserveCredentials(string credentialsAsJson)
        {
            var credentials = JsonConvert.DeserializeObject<UpserveCredentials>(credentialsAsJson);
            this.Key = credentials.Key;
            this.Username = credentials.Username;
            this.Password = credentials.Password;
        }
        public string Key { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class RetrivingMetadata
    {
        public string ThirdPartyId { get; }

        public RetrivingMetadata(
            string thirdPartyId
            )
        {
            this.ThirdPartyId = thirdPartyId;
        }
    }

    public class UpserveAdapter : IDataFetching, IRetrieveItems
    {
        public HttpClient httpClient;
        public UpserveAdapter(UpserveCredentials credentials)
        {
            this.httpClient = this.httpClient ?? new HttpClient();
            this.httpClient.DefaultRequestHeaders.Add("X-Breadcrumb-API-Key", credentials.Key);
            this.httpClient.DefaultRequestHeaders.Add("X-Breadcrumb-Username", credentials.Username);
            this.httpClient.DefaultRequestHeaders.Add("X-Breadcrumb-Passeord", credentials.Password);
        }

        public async Task<List<(Func<Item>, RetrivingMetadata)>> FetchItems(Establishment establishment)
        {
            string url = "https://api.breadcrumb.com/ws/v2/items.json";
            var parameters = new Dictionary<string, dynamic>
            {
                { "limit", 500 },
                { "offset", 0 }
            };

            List<ItemResponseModel> respones = new List<ItemResponseModel>();
            do
            {
                string fullUrl = $"{url}&{string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))}";

                var response = await this.FetchData<ItemResponseModel>(httpClient: this.httpClient, url: fullUrl);
                respones.Add(response);
            }
            while (respones.Last().Meta.Next != "null");

            List<ItemObject> itemObjects = respones.SelectMany(x => x.ItemObjects).ToList();

            List<(Func<Item>, RetrivingMetadata)> result = new List<(Func<Item>, RetrivingMetadata)>();

            foreach (var itemObject in itemObjects)
            {
                result.Add((
                    () => establishment.CreateItem(itemObject.Name, double.Parse(itemObject.Price)),
                    new RetrivingMetadata(itemObject.ItemId)
                    ));
            }

            return result;
        }

        public Task<List<(Func<Sale>, RetrivingMetadata)>> FetchSales(Establishment establishment)
        {
            throw new NotImplementedException();
        }

        public Task<List<(Func<Table>, RetrivingMetadata)>> FetchTables(Establishment establishment)
        {
            throw new NotImplementedException();
        }

        private async Task<T> FetchData<T>(HttpClient httpClient, string url)
        {
            try
            {
                var response = await this.httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch
            {
                throw new Exception("Data could not be fetched");
            }
        }

    }

    public class TableResponseModel
    {
        public MetaInfoObject Meta { get; set; }
        public List<TableObject> TableObjects { get; set; }
    }

    public class ItemResponseModel
    {
        public MetaInfoObject Meta { get; set; }
        public List<ItemObject> ItemObjects { get; set; }
    }

    public class MetaInfoObject
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int TotalCount { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
    }

    public class TableObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ItemObject
    {
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string ItemIdentifier { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
        public string CategoryId { get; set; }
        public string Tax { get; set; }
        public string TaxRateId { get; set; }
        public bool ContainsAlcohol { get; set; }
        public string Status { get; set; }
        public string ItemType { get; set; }
        public List<ItemModifierInfo> ItemModifiers { get; set; }
    }

    public class ItemModifierInfo
    {
        public string ModifierId { get; set; }
    }
}
