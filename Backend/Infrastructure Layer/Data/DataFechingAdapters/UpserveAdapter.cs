using Newtonsoft.Json;
using WebApplication1.Application_Layer.Services.DataFetching;
using WebApplication1.Domain_Layer.Entities;

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

    public class Meta
    {
        public int limit { get; set; }
        public int offset { get; set; }
        public int total_count { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
    }

    public class ItemModifier
    {
        public string modifier_id { get; set; }
    }

    public class ItemObject
    {
        public string item_id { get; set; }
        public string name { get; set; }
        public string item_identifier { get; set; }
        public string price { get; set; }
        public string category { get; set; }
        public string category_id { get; set; }
        public string tax { get; set; }
        public string tax_rate_id { get; set; }
        public bool contains_alcohol { get; set; }
        public string status { get; set; }
        public string item_type { get; set; }
        public List<ItemModifier> item_modifiers { get; set; }
    }
    public class SalesItemsObject
    {
        public string id { get; set; }
        public string check_id { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public string category_id { get; set; }
        public string item_id { get; set; }
        public int quantity { get; set; }
        public string price { get; set; }
        public string pre_tax_price { get; set; }
        public string regular_price { get; set; }
        public string cost { get; set; }
        public List<object> sides { get; set; }
        public List<object> modifiers { get; set; }
        public object voidcomp { get; set; }
    }

    public class SalesObject
    {
        public string id { get; set; }
        public string trading_day_id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string status { get; set; }
        public string sub_total { get; set; }
        public string tax_total { get; set; }
        public string total { get; set; }
        public string outstanding_balance { get; set; }
        public string mandatory_tip_amount { get; set; }
        public DateTime open_time { get; set; }
        public DateTime close_time { get; set; }
        public string employee_name { get; set; }
        public string employee_role_name { get; set; }
        public string employee_id { get; set; }
        public int guest_count { get; set; }
        public string type { get; set; }
        public string type_id { get; set; }
        public string taxed_type { get; set; }
        public List<SalesItemsObject> items { get; set; }
        public List<object> sides { get; set; }
        public List<object> modifiers { get; set; }
        public object voidcomp { get; set; }
        public string table_name { get; set; }
        public string zone { get; set; }
        public string zone_id { get; set; }
        public List<object> check_notes { get; set; }
    }
    public class OnlineOrder
    {
        public string id { get; set; }
        public DateTime time_placed { get; set; }
        public string source { get; set; }
        public string confirmation_code { get; set; }
        public DateTime promised_time { get; set; }
        public object delivery_info { get; set; }
    }

    public class Payment
    {
        public string id { get; set; }
        public string amount { get; set; }
        public string tip_amount { get; set; }
        public string autograt_amount { get; set; }
        public DateTime date { get; set; }
        public string employee_name { get; set; }
        public string employee_role_name { get; set; }
        public string employee_id { get; set; }
        public string type { get; set; }
        public string tender_description { get; set; }
        public string cc_name { get; set; }
        public string cc_type { get; set; }
    }

    public class Voidcomp
    {
        public string reason_text { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }

    public interface ISalesReponseModel
    {
        public Meta meta { get; set; }
    }

    public class SalesResponseModel : ISalesReponseModel
    {
        public Meta meta { get; set; }
        public List<SalesObject> objects { get; set; }
    }

    public class TableObject
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class TablesResponseModel : ISalesReponseModel
    {
        public Meta meta { get; set; }
        public List<TableObject> objects { get; set; }
    }

    public class ItemResponseModel : ISalesReponseModel
    {
        public Meta meta { get; set; }
        public List<ItemObject> objects { get; set; }
    }

}
