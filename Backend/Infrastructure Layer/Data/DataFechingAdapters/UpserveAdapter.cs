using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.Application_Layer.Services.DataFetching;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Entity_builders;

namespace WebApplication1.Application_Layer.Services
{
    public class UpserveCredentials
    {
        public string Key { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class UpserveAdapter : IDataFetching
    {
        private IFactoryServiceBuilder factoryServiceBuilder;
        public HttpClient httpClient;
        public UpserveAdapter(UpserveCredentials credentails, [FromServices] IFactoryServiceBuilder factoryServiceBuilder)
        {
            this.factoryServiceBuilder = factoryServiceBuilder;
            this.httpClient = this.httpClient ?? new HttpClient();
            this.httpClient.DefaultRequestHeaders.Add("X-Breadcrumb-API-Key", credentails.Key);
            this.httpClient.DefaultRequestHeaders.Add("X-Breadcrumb-Username", credentails.Username);
            this.httpClient.DefaultRequestHeaders.Add("X-Breadcrumb-Passeord", credentails.Password);
        }

        public Task<ICollection<Employee>> FetchEmployees()
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Item>> FetchItems()
        {
            string url = "https://api.breadcrumb.com/ws/v2/items.json";
            var parameters = new Dictionary<string, dynamic>
            {
                //{ "category_id", "" },
                //{ "status", "" },
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

            List<Item> items = new List<Item>();
            foreach (var itemObject in itemObjects)
            {
                IItemBuilderService builder = this.factoryServiceBuilder.ItemBuilder();

                var newItem = builder
                    .withName(itemObject.Name)
                    .withPrice(double.Parse(itemObject.Price))
                    .Build();

                items.Add(newItem);
            }

            return items;
        }

        public Task<ICollection<Sale>> FetchSales()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Table>> FetchTables()
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
