using EstablishmentProject.test.TestingCode;
using RichardSzalay.MockHttp;
using System.Net;
using WebApplication1.Application_Layer.Services;
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
            var adapter = new UpserveAdapter("Username", "Password", "Key", createItemsTestHttpClient());

            // Act
            var res = await adapter.RetrieveItems();
            List<Item> items = res.Select(x => x.Item1).ToList().Select(x => x(establishment)).ToList();

            // Assert
            Assert.Equal(1, items.Count);
            Assert.Equal("Food", items[0].Name);
            Assert.Equal(14.00, items[0].GetPrice());
        }

        private HttpClient createItemsTestHttpClient()
        {
            MockHttpMessageHandler mockHttpMessageHandler = new MockHttpMessageHandler();

            var content = @"{
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
                        'name': 'Food',
                        'item_identifier': '',
                        'price': '14,00',
                        'category': 'Appetizer',
                        'category_id': '4af7fc76-94bc-4910-8bf8-59f7ed71e042',
                        'tax': 'Sales Tax',
                        'tax_rate_id': 'b901acb9-5f5a-39d8-fdb4-1b712fb3995b',
                        'contains_alcohol': false,
                        'status': 'active',
                        'item_type': 'normal'
                    }
                ]
            }";

            HttpContent httpContentItems = new StringContent(content);
            mockHttpMessageHandler.When("*").Respond(HttpStatusCode.OK, httpContentItems);
            return new HttpClient(mockHttpMessageHandler);
        }
        private HttpClient createTablesTestHttpClient()
        {
            MockHttpMessageHandler mockHttpMessageHandler = new MockHttpMessageHandler();

            var content = @"{
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
                        'name': 'Food',
                        'item_identifier': '',
                        'price': '14,00',
                        'category': 'Appetizer',
                        'category_id': '4af7fc76-94bc-4910-8bf8-59f7ed71e042',
                        'tax': 'Sales Tax',
                        'tax_rate_id': 'b901acb9-5f5a-39d8-fdb4-1b712fb3995b',
                        'contains_alcohol': false,
                        'status': 'active',
                        'item_type': 'normal'
                    }
                ]
            }";

            HttpContent httpContentItems = new StringContent(content);
            mockHttpMessageHandler.When("*").Respond(HttpStatusCode.OK, httpContentItems);
            return new HttpClient(mockHttpMessageHandler);
        }



    }
}
