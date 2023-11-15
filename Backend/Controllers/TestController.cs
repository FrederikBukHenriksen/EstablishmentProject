using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Data.DataModels;
using WebApplication1.Models;
using WebApplication1.Repositories;
using Xunit.Abstractions;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private IUserRepository _userRepository;
        private IUserRolesRepository _userRolesRepository;
        private IEstablishmentRepository _establishmentRepository;
        private ApplicationDbContext _applicationDbContext;

        public TestController(IEstablishmentRepository establishmentRepository, ApplicationDbContext applicationDbContext)
        {
            _establishmentRepository = establishmentRepository;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public void Establishment()
        {

            var cusItems = new List<Item> { new Item { Name = "test", Price = 10 } };
            var extra    = new List<Item> { new Item { Name = "lige i 2eren", Price = 69 } };
            _establishmentRepository.Add(new Establishment { Name = "Luffe", Items = new List<Item> { new Item { Name = "Karen Marie", Price = 1 } } });
        }

    }
}