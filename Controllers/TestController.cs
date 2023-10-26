using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.DataModels;
using WebApplication1.Models;
using WebApplication1.Repositories;

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

        public TestController(IUserRepository userRepository, IUserRolesRepository userRolesRepository, IEstablishmentRepository establishmentRepository)
        {
            _userRepository = userRepository;
            _userRolesRepository = userRolesRepository;
            _establishmentRepository = establishmentRepository;
        }

        [HttpGet]
        public User Establishment (
            )
        {
            var getSimpleEstab = _establishmentRepository.Find(x => true);

            var testEstab = _establishmentRepository.getItAll(new Guid("00000000-0000-0000-0000-000000000001"));

            var user = _userRepository.GetAll().FirstOrDefault();

            //var estab = _userRolesRepository.Queryable
            //    .Where(x => x.User.Id == user.Id)
            //    .Select(x => x.Establishment)
            //.FirstOrDefault();

            //_userRolesRepository.Context.Entry(user).Collection(x => x.UserRoles).Query().Load();
            //_userRolesRepository.Context.Entry(userRole).Reference(x => x.User).Load();


            return user;

        }

    }
}