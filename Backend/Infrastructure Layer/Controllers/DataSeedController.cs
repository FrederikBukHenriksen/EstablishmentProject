using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandHandlers;
using WebApplication1.Data;
using WebApplication1.Data.DataModels;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Domain_Layer.Services.Entity_builders;
using WebApplication1.Program;
using WebApplication1.Utils;
using Xunit.Abstractions;
using static WebApplication1.Utils.SalesHelper;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/test")]
    public class DataSeedController : ControllerBase
    {
        private IUserRepository _userRepository;
        private ISalesService salesService;
        private ISalesRepository _salesRepository;
        private IUserRolesRepository _userRolesRepository;
        private IEstablishmentRepository _establishmentRepository;
        private ApplicationDbContext _applicationDbContext;

        public DataSeedController(ISalesService salesService,IEstablishmentRepository establishmentRepository, IUserRepository userRepository, IUserRolesRepository userRolesRepository, ISalesRepository salesRepository)
        {
            this.salesService = salesService;
            _salesRepository = salesRepository;
            _userRolesRepository = userRolesRepository;
            _userRepository = userRepository;
            _establishmentRepository = establishmentRepository;
        }

        [HttpGet("lol")]
        public IActionResult Lol(FactoryServiceBuilder factory) {
            List<Establishment> list = new List<Establishment>();

            // Resolve the transient service directly
            //var yourService = serviceProvider.GetRequiredService<IEstablishmentBuilder>();
            var haha = factory.EstablishmentBuilder();
                Establishment estab = haha.WithName("Cafe Frederik").Build();
                list.Add(estab);

            var lol = factory.EstablishmentBuilder();
            var ok = lol.WithId(Guid.Parse("00000000-0000-0000-0000-000000000000")).Build();
            list.Add(ok);

            //var heinz = factory.EstablishmentBuilder.UseExistingEntity(estab).Build();





            // Use yourService instance here
            //    var e1 = establishmentBuilder.WithName("Cafe Frederik").Build();
            //    var e2 = establishmentBuilder.Build();

            //var lol = new lolcat();

            //string hello = "guten tag";
            //try
            //{
            //    hello = this.HttpContext.Session.GetString("establishment");
            //}
            //catch (System.Exception e)
            //{
            //}

            //this.HttpContext.Session.SetString("establishment", "Frederik");

            //Dictionary<SalesAttributes, (Type type, Func<Sale, object> selector)> dic = new Dictionary<SalesAttributes, (Type, Func<Sale, object>)>() 
            //{
            //    {SalesAttributes.TimestampArrival, (typeof(DateTime), (Sale sale) => sale.TimestampArrival)},
            //    {SalesAttributes.TimestampPayment, (typeof(DateTime), (Sale sale) => sale.TimestampPayment)},
            //};

            //var sale = new Sale();
            //var att = SalesAttributes.TimestampPayment;
            //var findDic = dic[att];
            //Type lol = findDic.type;

            //var hej = salesService.GetAttributeValue(sale, findDic.selector);
            //var ok = (DateTime)hej;

            return Ok("hello");
        }



        [HttpGet]
        public void SeedDatabase()
        {
            User user1 = TestDataFactoryStatic.CreateUser(id: Guid.Parse("00000000-0000-0000-0000-000000000001"), username: "Frederik", password: "1234");

            Establishment establishment1 = TestDataFactoryStatic.CreateEstablishment(Guid.Parse("00000000-0000-0000-0000-000000000001"));

            UserRole userRole1 = TestDataFactoryStatic.CreateUserRole(establishment1, user1, Role.Admin);

            Item espresso = TestDataFactoryStatic.CreateItem(id: Guid.Parse("00000000-0000-0000-0000-000000000001"), name: "Espresso");
            establishment1.Items.Add(espresso);
            Item sparklingWater = TestDataFactoryStatic.CreateItem(id: Guid.Parse("00000000-0000-0000-0000-000000000002"), name: "Sparkling water");
            establishment1.Items.Add(sparklingWater);
            Item bunWithCheese = TestDataFactoryStatic.CreateItem(id: Guid.Parse("00000000-0000-0000-0000-000000000003"), name: "Bun with cheese");
            establishment1.Items.Add(bunWithCheese);

            Sale sale1 = TestDataFactoryStatic.CreateSale(timestampEnd: DateTime.Now.AddHours(-7));
            Sale sale2 = TestDataFactoryStatic.CreateSale(timestampEnd: DateTime.Now.AddHours(-6));
            Sale sale3 = TestDataFactoryStatic.CreateSale(timestampEnd: DateTime.Now.AddHours(-5));
            Sale sale4 = TestDataFactoryStatic.CreateSale(timestampEnd: DateTime.Now.AddHours(-5));
            Sale sale5 = TestDataFactoryStatic.CreateSale(timestampEnd: DateTime.Now.AddHours(-5));

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