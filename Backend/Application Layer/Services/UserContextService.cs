using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;

namespace WebApplication1.Services
{
    public interface IUserContextService
    {
        public User GetUser();
        public Establishment GetActiveEstablishment();
        public List<Establishment> GetAccessibleEstablishments();
        public void SetUser(Guid userId);
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

        public User GetUser()
        {
            if (_user == null)
            {
                throw  new NullReferenceException("User is null");
            }
            return _user;
        }

        public Establishment GetActiveEstablishment()
        {
            return _establishmentRepository.GetAll().First(); // TESTING PURPOSES
            if (_establishment == null)
            {
                throw new NullReferenceException("Active establishment is null");
            }
            return _establishment;
        }

        public List<Establishment> GetAccessibleEstablishments()
        {
            if (_establishments == null)
            {
                throw new NullReferenceException("Accessible establishments are null");
            }
            return _establishments;
        }

        public void FetchActiveEstablishmentFromHttpHeader(HttpContext httpContext)
        {
            string EstablishmentIdAsString = httpContext.Request.Headers["EstablishmentId"];
            if (!EstablishmentIdAsString.IsNullOrEmpty())
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
