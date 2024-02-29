using Newtonsoft.Json;
using WebApplication1.Application_Layer.Services.DataFetching;
using WebApplication1.Domain_Layer.Entities;
using static WebApplication1.Infrastructure_Layer.Data.DataFechingAdapters.UpserveAdapter;

namespace WebApplication1.Application_Layer.Services
{
    public class RetrivingMetadata
    {
        public string ThirdPartyId { get; }
        public RetrivingMetadata(string thirdPartyId)
        {
            this.ThirdPartyId = thirdPartyId;
        }
    }

    public class LighspeedCredentials
    {
        public string username;
        public string password;
        public string key;

        public LighspeedCredentials(string JSON)
        {
            JsonConvert.PopulateObject(JSON, this);
        }

        public LighspeedCredentials(string username, string password, string key)
        {
            this.username = username;
            this.password = password;
            this.key = key;
        }
    }

    public class UpserveAdapter : IRetrieveItemsData, IRetrieveTablesData, IRetrieveSalesData
    {
        public HttpClient httpClient = new HttpClient();
        public UpserveAdapter(LighspeedCredentials credentials, HttpClient? httpClient = null)
        {
            this.httpClient.DefaultRequestHeaders.Add("X-Breadcrumb-Username", credentials.username);
            this.httpClient.DefaultRequestHeaders.Add("X-Breadcrumb-Passeord", credentials.password);
            this.httpClient.DefaultRequestHeaders.Add("X-Breadcrumb-API-Key", credentials.key);

            if (httpClient != null)
            {
                this.httpClient = httpClient;
            }
        }

        public async Task<List<(Func<Establishment, List<EntityIdAndForeignId>, Sale>, RetrivingMetadata)>> RetrieveSales()
        {
            string url = "https://api.breadcrumb.com/ws/v2/checks.json";
            var parameters = new Dictionary<string, dynamic>
            {
                { "limit", 500 },
                { "offset", 0 }
            };

            List<SalesResponseModel> respones = await this.ExtractAllResponses<SalesResponseModel>(url, parameters);
            List<SalesObject> saleObjects = respones.SelectMany(x => x.objects).ToList();

            List<(Func<Establishment, List<EntityIdAndForeignId>, Sale>, RetrivingMetadata)> result = new List<(Func<Establishment, List<EntityIdAndForeignId>, Sale>, RetrivingMetadata)>();
            foreach (var saleObject in saleObjects)
            {
                Func<Establishment, List<EntityIdAndForeignId>, Sale> createItem = (establishment, entityAndForeignRelation) =>
                {
                    var sale = establishment.CreateSale(saleObject.close_time);
                    establishment.AddSale(sale);
                    sale.setTimeOfArrival(saleObject.open_time);

                    //Iterate over items sold on the sale
                    foreach (var salesItemsObject in saleObject.items)
                    {
                        //Find the existing item's foreign ID
                        EntityIdAndForeignId? foreignIdAndEntity = entityAndForeignRelation.Find(x => x.ForeingId == salesItemsObject.id);
                        if (foreignIdAndEntity == null)
                        {
                            throw new InvalidDataException("Item not found within establishment");
                        }
                        Item establishmentItem = establishment.GetItems().Find(x => x.Id == foreignIdAndEntity.entityId);
                        var salesItems = establishment.CreateSalesItem(sale, establishmentItem, salesItemsObject.quantity);
                        establishment.AddSalesItems(sale, salesItems);
                    }

                    if (saleObject.type == "Table" && saleObject.table_name != "")
                    {
                        Table? establishmentTable = establishment.GetTables().Find(x => x.Name == saleObject.table_name);
                        if (establishmentTable == null)
                        {
                            throw new InvalidDataException("Table not found within establishment");
                        }
                        var salesTables = establishment.CreateSalesTables(sale, establishmentTable);
                        establishment.AddSalesTables(sale, salesTables);
                    }

                    return sale;
                };
                RetrivingMetadata metaData = new RetrivingMetadata(saleObject.id);
                result.Add((createItem, metaData));
            }
            return result;
        }

        public async Task<List<(Func<Establishment, Item>, RetrivingMetadata)>> RetrieveItems()
        {
            string url = "https://api.breadcrumb.com/ws/v2/items.json";
            var parameters = new Dictionary<string, dynamic>
            {
                { "limit", 500 },
                { "offset", 0 }
            };

            List<ItemResponseModel> respones = await this.ExtractAllResponses<ItemResponseModel>(url, parameters);

            List<ItemObject> itemObjects = respones.SelectMany(x => x.objects).ToList();

            List<(Func<Establishment, Item>, RetrivingMetadata)> result = new List<(Func<Establishment, Item>, RetrivingMetadata)>();

            foreach (var itemObject in itemObjects)
            {
                Func<Establishment, Item> createItem = (establishment) => establishment.CreateItem(itemObject.name, double.Parse(itemObject.price));
                RetrivingMetadata metaData = new RetrivingMetadata(itemObject.item_id);
                result.Add((createItem, metaData));
            }
            return result;
        }

        public async Task<List<(Func<Establishment, Table>, RetrivingMetadata)>> RetrieveTables()
        {
            string url = "https://api.breadcrumb.com/ws/v2/tables.json";
            var parameters = new Dictionary<string, dynamic>
            {
                { "limit", 500 },
                { "offset", 0 }
            };

            List<TablesResponseModel> respones = await this.ExtractAllResponses<TablesResponseModel>(url, parameters);

            List<TableObject> tableObjects = respones.SelectMany(x => x.objects).ToList();

            List<(Func<Establishment, Table>, RetrivingMetadata)> result = new List<(Func<Establishment, Table>, RetrivingMetadata)>();

            foreach (var tableObject in tableObjects)
            {
                Func<Establishment, Table> createTable = (establishment) => establishment.CreateTable(tableObject.name);
                RetrivingMetadata metaData = new RetrivingMetadata(tableObject.id);
                result.Add((createTable, metaData));
            }
            return result;
        }



        private async Task<List<T>> ExtractAllResponses<T>(string url, Dictionary<string, dynamic> parameters) where T : ISalesReponseModel
        {
            List<T> respones = new List<T>();
            int incrementer = 0;
            do
            {
                dynamic limit;
                parameters.TryGetValue("limit", out limit);
                int newOffset = incrementer * ((int)limit);
                parameters.Remove("offset");
                parameters.Add("offset", newOffset);


                string fullUrl = $"{url}&{string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))}";

                var response = await this.FetchData<T>(httpClient: this.httpClient, url: fullUrl);
                respones.Add(response);
                incrementer++;
            }
            while (respones.Last().meta.next != null);
            return respones;
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
}
