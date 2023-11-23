using Microsoft.AspNetCore.Http;
using System.Linq;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public interface IUserContextService
    {
        public void SetUser(Guid userId);
        public User? GetUser();
        public List<Establishment> GetAccessibleEstablishments();
        public Establishment? GetActiveEstablishment();
        public void FetchActiveEstablishmentFromHttpHeader(HttpContext httpContext);
        public void FecthAccesibleEstablishments();
    }

    public class UserContextService : IUserContextService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRolesRepository _userRolesRepository;
        private readonly IEstablishmentRepository _establishmentRepository;

        private User? _user = null;
        private Establishment? _establishment = null;
        private List<Establishment>? _establishments = null;

        public UserContextService(IUserRepository userRepository,IUserRolesRepository userRolesRepository, IEstablishmentRepository establishmentRepository)
        {
            this._userRepository = userRepository;
            this._userRolesRepository = userRolesRepository;
            this._establishmentRepository = establishmentRepository;
        }

        public void SetUser(Guid userId)
        {
            var testGetAll = _userRepository.GetAll();
            _user = _userRepository.GetById(userId);
            if (_user == null)
            {
                return;
            }
        }

        public void FecthAccesibleEstablishments()
        {
            _establishments = _userRolesRepository.GetAllIncludeEstablishment().Where(x => x.User.Id == _user.Id).Select(x => x.Establishment).ToList();
        }

        public User? GetUser()
        {
            return _user;
        }

        public Establishment? GetActiveEstablishment()
        {
 
            return _establishment;
        }

        public List<Establishment>? GetAccessibleEstablishments()
        {
            return _establishments;
        }

        public void FetchActiveEstablishmentFromHttpHeader(HttpContext httpContext)
        {
            string EstablishmentIdAsString = httpContext.Request.Headers["EstablishmentId"];
            if (EstablishmentIdAsString != null)
            {
                Guid EstablishmentId = Guid.Parse(EstablishmentIdAsString);
                bool UserIsAssociatedWithEstablishment = GetAccessibleEstablishments().Any(x => x.Id == EstablishmentId);
                if (UserIsAssociatedWithEstablishment)
                {
                    _establishment = _establishmentRepository.GetById(EstablishmentId);
                }
            }
        }
    }
}
