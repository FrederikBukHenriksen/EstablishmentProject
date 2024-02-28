using EstablishmentProject.test.TestingCode;
using RichardSzalay.MockHttp;
using System.Net;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Application_Layer.Services.DataFetching;
using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test
{

    public class UpserveAdapterTest : IntegrationTest
    {
        public UpserveAdapterTest() : base(new List<ITestService> { DatabaseTestContainer.CreateAsync().Result })
        {
        }


        [Fact]
        public async void RetrieveItems()
        {
            // Arrange
            var establishment = new Establishment("Test establishment");

            var credentials = new LighspeedCredentials("Username", "Password", "Key");

            var adapter = new UpserveAdapter(credentials, createTestHttpClient());
            List<EntityIdAndForeignId> foreingIDAnEntityIDs = new List<EntityIdAndForeignId>();

            // Act
            var res = await adapter.RetrieveItems();
            List<Item> items = res.Select(x => x.Item1).ToList().Select(x => x(establishment)).ToList();

            // Assert
            Assert.Equal(1, items.Count);
            Assert.Equal("Coffee", items[0].Name);
            Assert.Equal(35.00, items[0].GetPrice());
        }

        [Fact]
        public async void RetrieveTables()
        {
            // Arrange
            var establishment = new Establishment("Test establishment");

            var credentials = new LighspeedCredentials("Username", "Password", "Key");

            var adapter = new UpserveAdapter(credentials, createTestHttpClient());


            // Act
            var res = await adapter.RetrieveTables();
            List<Table> tables = res.Select(x => x.Item1).ToList().Select(x => x(establishment)).ToList();

            // Assert
            Assert.Equal(1, tables.Count);
            Assert.Equal("Table 1", tables[0].Name);
        }

        [Fact]
        public async void RetrieveSales()
        {
            var establishment = new Establishment("Test establishment");
            var item = establishment.CreateItem("Coffee", 35.00);
            establishment.AddItem(item);
            var table = establishment.CreateTable("Table 1");
            establishment.AddTable(table);

            string itemForeingId = "20a86918-c01d-edab-d809-d76d9fd96232";
            string tableForignId = "45b5ec2d-1a8d-4b65-8699-944326fdde2a";

            var credentials = new LighspeedCredentials("Username", "Password", "Key");

            var adapter = new UpserveAdapter(credentials, createTestHttpClient());

            List<EntityIdAndForeignId> foreingIDAnEntityIDs = new List<EntityIdAndForeignId>
            {
                new EntityIdAndForeignId(item.Id, itemForeingId),
                new EntityIdAndForeignId(table.Id, tableForignId)
            };

            //Act
            var res = await adapter.RetrieveSales();
            var salesmid = res.Select(x => x.Item1).ToList();
            var sales = salesmid.Select(x => x(establishment, foreingIDAnEntityIDs)).ToList();

            //Assert
            Assert.Equal(1, sales.Count);
            Assert.Equal(table.Id, sales[0].GetSalesTables()[0].Table.Id);
            Assert.Equal(item.Id, sales[0].GetSalesItems()[0].Item.Id);
            Assert.Equal(1, sales[0].GetSalesItems()[0].quantity);
        }

        private HttpClient createTestHttpClient()
        {
            MockHttpMessageHandler mockHttpMessageHandler = new MockHttpMessageHandler();
            HttpContent httpContentItems = new StringContent(itemsContent);
            mockHttpMessageHandler.When("https://api.breadcrumb.com/ws/v2/items.json*").Respond(HttpStatusCode.OK, httpContentItems);
            HttpContent httpContentTables = new StringContent(tableContent);
            mockHttpMessageHandler.When("https://api.breadcrumb.com/ws/v2/tables.json*").Respond(HttpStatusCode.OK, httpContentTables);
            HttpContent httpContentSales = new StringContent(SalesContent);
            mockHttpMessageHandler.When("https://api.breadcrumb.com/ws/v2/checks.json*").Respond(HttpStatusCode.OK, httpContentSales);
            return new HttpClient(mockHttpMessageHandler);
        }

        private string itemsContent = @"{
                'meta': {
                    'limit': 0,
                    'offset': 0,
                    'total_count': 0,
                    'next': null,
                    'previous': null
                },
                'objects': [
                    {
                        'item_id': '20a86918-c01d-edab-d809-d76d9fd96232',
                        'name': 'Coffee',
                        'item_identifier': '',
                        'price': '35,00',
                        'category': '',
                        'category_id': '',
                        'tax': '',
                        'tax_rate_id': '',
                        'contains_alcohol': false,
                        'status': '',
                        'item_type': ''
                    }
                ]
            }";

        private string tableContent = @"{
                'meta': {
                    'limit': 0,
                    'offset': 0,
                    'total_count': 0,
                    'next': null,
                    'previous': null
                },
                'objects': [
                    {
                        'item_id': '45b5ec2d-1a8d-4b65-8699-944326fdde2a',
                        'name': 'Table 1',
                    }
                ]
            }";

        private string SalesContent = @"
                {
                'meta': {
                    'limit': 0,
                    'offset': 0,
                    'total_count': 0,
                    'next': null,
                    'previous': null
                },
                  ""objects"": [
                    {
                      ""id"": ""91dfe96e-e1e7-4bf5-ae32-6fd29d20c8d3"",
                      ""trading_day_id"": ""string"",
                      ""name"": ""string"",
                      ""number"": ""string"",
                      ""status"": ""Open"",
                      ""sub_total"": ""string"",
                      ""tax_total"": ""string"",
                      ""total"": ""string"",
                      ""outstanding_balance"": ""string"",
                      ""mandatory_tip_amount"": ""string"",
                      ""open_time"": ""2019-08-24T14:15:22Z"",
                      ""close_time"": ""2019-08-24T14:16:22Z"",
                      ""employee_name"": ""string"",
                      ""employee_role_name"": ""string"",
                      ""employee_id"": ""string"",
                      ""guest_count"": 0,
                      ""type"": ""Table"",
                      ""type_id"": ""1"",
                      ""taxed_type"": ""inclusive"",
                      ""items"": [
                        {
                          ""id"": ""20a86918-c01d-edab-d809-d76d9fd96232"",
                          ""check_id"": ""string"",
                          ""name"": ""Coffee"",
                          ""date"": ""2019-08-24T14:15:22Z"",
                          ""category_id"": ""string"",
                          ""item_id"": ""string"",
                          ""quantity"": 1,
                          ""price"": ""string"",
                          ""pre_tax_price"": ""string"",
                          ""regular_price"": ""string"",
                          ""cost"": ""string"",
                          ""sides"": [
                            {
                              ""id"": ""string"",
                              ""check_id"": ""string"",
                              ""name"": ""string"",
                              ""date"": ""2019-08-24T14:15:22Z"",
                              ""category_id"": ""string"",
                              ""item_id"": ""string"",
                              ""quantity"": 0,
                              ""price"": ""string"",
                              ""pre_tax_price"": ""string"",
                              ""regular_price"": ""string"",
                              ""cost"": ""string"",
                              ""modifiers"": [
                                {
                                  ""id"": ""string"",
                                  ""name"": ""string"",
                                  ""price"": ""string"",
                                  ""tax"": ""string"",
                                  ""tax_rate_id"": ""string"",
                                  ""status"": ""active""
                                }
                              ],
                              ""voidcomp"": {
                                ""reason_text"": ""string"",
                                ""authorized_id"": ""string"",
                                ""authorizer_role_name"": ""string"",
                                ""type"": ""void"",
                                ""value"": ""string""
                              }
                            }
                          ],
                          ""modifiers"": [
                            {
                              ""id"": ""string"",
                              ""name"": ""string"",
                              ""price"": ""string"",
                              ""tax"": ""string"",
                              ""tax_rate_id"": ""string"",
                              ""status"": ""active""
                            }
                          ],
                          ""voidcomp"": {
                            ""reason_text"": ""string"",
                            ""authorized_id"": ""string"",
                            ""authorizer_role_name"": ""string"",
                            ""value"": ""string"",
                            ""total"": ""string"",
                            ""type"": ""void""
                          }
                        }
                      ],
                      ""online_order"": {
                        ""id"": ""string"",
                        ""time_placed"": ""2019-08-24T14:15:22Z"",
                        ""source"": ""string"",
                        ""confirmation_code"": ""string"",
                        ""promised_time"": ""2019-08-24T14:15:22Z"",
                        ""delivery_info"": {
                          ""name"": ""string"",
                          ""phone"": ""string"",
                          ""address"": {
                            ""street1"": ""string"",
                            ""street2"": ""string"",
                            ""city"": ""string"",
                            ""state"": ""string"",
                            ""zip"": ""string""
                          }
                        }
                      },
                      ""payments"": [
                        {
                          ""id"": ""string"",
                          ""amount"": ""string"",
                          ""tip_amount"": ""string"",
                          ""autograt_amount"": ""string"",
                          ""date"": ""2019-08-24T14:15:22Z"",
                          ""employee_name"": ""string"",
                          ""employee_role_name"": ""string"",
                          ""employee_id"": ""string"",
                          ""type"": ""string"",
                          ""tender_description"": ""string"",
                          ""cc_name"": ""string"",
                          ""cc_type"": ""Mastercard""
                        }
                      ],
                      ""voidcomp"": {
                        ""reason_text"": ""string"",
                        ""type"": ""comp value"",
                        ""value"": ""string""
                      },
                      ""table_name"": ""Table 1"",
                      ""zone"": ""string"",
                      ""zone_id"": ""string"",
                      ""check_notes"": [
                        {
                          ""id"": ""string"",
                          ""note"": ""string"",
                          ""author_id"": ""string""
                        }
                      ]
                    }
                  ]
                }";



    }
}
