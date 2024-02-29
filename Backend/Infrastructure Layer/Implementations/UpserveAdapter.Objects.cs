using System.Diagnostics.CodeAnalysis;

namespace WebApplication1.Infrastructure_Layer.Data.DataFechingAdapters
{
    [ExcludeFromCodeCoverage]
    internal partial class UpserveAdapter
    {
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
}
