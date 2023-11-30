using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.Data;
using WebApplication1.Data.DataModels;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Program;
using Xunit.Abstractions;
using static WebApplication1.Utils.SalesHelper;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private IUserRepository _userRepository;
        private ISalesService salesService;
        private ISalesRepository _salesRepository;
        private IUserRolesRepository _userRolesRepository;
        private IEstablishmentRepository _establishmentRepository;
        private ApplicationDbContext _applicationDbContext;

        public TestController(ISalesService salesService,IEstablishmentRepository establishmentRepository, IUserRepository userRepository, IUserRolesRepository userRolesRepository, ISalesRepository salesRepository, ApplicationDbContext applicationDbContext)
        {
            this.salesService = salesService;
            _salesRepository = salesRepository;
            _userRolesRepository = userRolesRepository;
            _userRepository = userRepository;
            _establishmentRepository = establishmentRepository;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet("lol")]
        public IActionResult Lol()
        {
            //string hello = "guten tag";
            //try
            //{
            //    hello = this.HttpContext.Session.GetString("establishment");
            //}
            //catch (System.Exception e)
            //{
            //}


            //this.HttpContext.Session.SetString("establishment", "Frederik");




            Dictionary<SalesAttributes, (Type type, Func<Sale, object> selector)> dic = new Dictionary<SalesAttributes, (Type, Func<Sale, object>)>() 
            {
                {SalesAttributes.TimestampArrival, (typeof(DateTime), (Sale sale) => sale.TimestampArrival)},
                {SalesAttributes.TimestampPayment, (typeof(DateTime), (Sale sale) => sale.TimestampPayment)},
            };


            var sale = _salesRepository.GetAll().First();
            var att = SalesAttributes.TimestampPayment;
            var findDic = dic[att];
            Type lol = findDic.type;

            var hej = salesService.GetAttributeValue(sale, findDic.selector);
            var ok = (DateTime)hej;


            return Ok("hello");



        }



        [HttpGet]
        public void Establishment()
        {
           

            User user1 = TestDataFactory.CreateUser(username: "Frederik", password: "1234");

            Establishment establishment1 = TestDataFactory.CreateEstablishment(Guid.Parse("00000000-0000-0000-0000-000000000001"));

            UserRole userRole1 = TestDataFactory.CreateUserRole(establishment1, user1, Role.Admin);

            Item espresso = TestDataFactory.CreateItem(id: Guid.Parse("00000000-0000-0000-0000-000000000001"), name: "Espresso");
            establishment1.Items.Add(espresso);
            Item sparklingWater = TestDataFactory.CreateItem(id: Guid.Parse("00000000-0000-0000-0000-000000000002"), name: "Sparkling water");
            establishment1.Items.Add(sparklingWater);
            Item bunWithCheese = TestDataFactory.CreateItem(id: Guid.Parse("00000000-0000-0000-0000-000000000003"), name: "Bun with cheese");
            establishment1.Items.Add(bunWithCheese);

            Sale sale1 = TestDataFactory.CreateSale(timestampEnd: DateTime.Now.AddHours(-7));
            Sale sale2 = TestDataFactory.CreateSale(timestampEnd: DateTime.Now.AddHours(-6));
            Sale sale3 = TestDataFactory.CreateSale(timestampEnd: DateTime.Now.AddHours(-5));
            Sale sale4 = TestDataFactory.CreateSale(timestampEnd: DateTime.Now.AddHours(-5));
            Sale sale5 = TestDataFactory.CreateSale(timestampEnd: DateTime.Now.AddHours(-5));

            sale1.SalesItems.Add(new SalesItems
            {
                Item = espresso,
                Quantity = 2,
            });

            sale2.SalesItems.Add(new SalesItems
            {
                Item = espresso,
                Quantity = 5,
            });


            sale3.SalesItems.Add(new SalesItems
            {
                Item = espresso,
                Quantity = 10,
            });


            sale4.SalesItems.Add(new SalesItems
            {
                Item = espresso,
                Quantity = 5,
            });


            sale5.SalesItems.Add(new SalesItems
            {
                Item = espresso,
                Quantity = 2,
            });

            establishment1.Sales.Add(sale1);
            establishment1.Sales.Add(sale2);
            establishment1.Sales.Add(sale3);
            establishment1.Sales.Add(sale4);
            establishment1.Sales.Add(sale5);

            _establishmentRepository.Add(establishment1);
            _userRepository.Add(user1);
            _userRolesRepository.Add(userRole1);
        }
    }
}