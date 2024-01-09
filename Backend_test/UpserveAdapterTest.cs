using System.Net.Http.Json;
using WebApplication1.Application_Layer.Services;

namespace EstablishmentProject.test
{

    public class UpserveAdapterTest : BaseIntegrationTest
    {

        public UpserveAdapterTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }


        [Fact]
        public async void GetTables()
        {
            //ARRANGE
            List<TableObject> ItemObjects = new List<TableObject> {
                new TableObject
                {
                    Id = "20a86918-c01d-edab-d809-d76d9fd96232",
                    Name = "Food"
                }
            };

            var meta = new MetaInfoObject()
            {
                Limit = 0,
                Offset = 0,
                TotalCount = 0,
                Next = "string",
                Previous = "string"
            };

            var TableResponseModel = new TableResponseModel()
            {
                Meta = meta,
                TableObjects = ItemObjects
            };


            HttpResponseMessage response = new()
            {
                Content = JsonContent.Create(TableResponseModel)
            };



        }

    }
}
