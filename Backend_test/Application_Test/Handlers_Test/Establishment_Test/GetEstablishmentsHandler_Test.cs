using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Handlers.SalesHandlers;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Utils;

namespace EstablishmentProject.test.Application.Handlers.Establishment_Test
{
    public class GetEstablishmentsHandler_Test : BaseTest
    {
        private IUnitOfWork unitOfWork;
        private GetSalesCommand getSalesCommand_WithSaleIds;
        private GetSalesCommand getSalesCommand_WithSalesSorting;
        private Establishment establishment;
        private Sale sale;


        public GetEstablishmentsHandler_Test() : base(new List<ITestService> { TestContainer.CreateAsync().Result })
        {
            //Inject services
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            createTestData();

            getSalesCommand_WithSaleIds = new GetSalesCommand
            {
                EstablishmentId = establishment.Id,
                SalesIds = establishment.Sales.Select(x => x.Id).ToList()
            };

            getSalesCommand_WithSalesSorting = new GetSalesCommand
            {
                EstablishmentId = establishment.Id,
                SalesSorting = new SalesSorting(withinTimeperiods: new List<(DateTime start, DateTime end)> { (DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1)) })
            };
        }

        private void createTestData()
        {
            establishment = new Establishment("Test establishment");
            Item item = establishment.AddItem(establishment.CreateItem("Test item", 0.0));
            sale = establishment.AddSale(establishment.CreateSale(DateTime.Now, itemAndQuantity: new List<(Item, int)> { (item, 1) }));

            using (var uow = unitOfWork)
            {
                uow.establishmentRepository.Add(establishment);
            }
        }
    }
}