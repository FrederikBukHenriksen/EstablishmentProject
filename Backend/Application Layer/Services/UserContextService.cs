using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;

namespace WebApplication1.Services
{
    public interface IUserContextService
    {
        public User GetUser();
        public Establishment GetActiveEstablishment();
        public List<Establishment> GetAccessibleEstablishments();
        public UserRole GetActiveUserRole();
        List<UserRole>? GetAllUserRoles();
        public void SetUser(User? user);
        public void FetchActiveEstablishmentFromHttpHeader(HttpContext httpContext);
        public void FecthAccesibleEstablishments();
    }

    public class UserContextService : IUserContextService
    {
        private readonly IUserRolesRepository _userRolesRepository;
        private readonly IEstablishmentRepository _establishmentRepository;

        private User? _user = null;
        private Establishment? _activeEstablishment = null;
        private List<UserRole>? _userRoles = null;

        public UserContextService(IUserRolesRepository userRolesRepository, IEstablishmentRepository establishmentRepository)
        {
            this._userRolesRepository = userRolesRepository;
            this._establishmentRepository = establishmentRepository;
        }

        public void SetUser(User? user)
        {
            _user = user;
        }

        public User GetUser()
        {
            return _user;
        }

        public void FecthAccesibleEstablishments()
        {
            this._userRoles = this._userRolesRepository.GetAllIncludeEstablishment().Where(x => x.User == this.GetUser()).ToList();
        }

        public Establishment GetActiveEstablishment()
        {

            return _activeEstablishment;
        }

        public List<Establishment> GetAccessibleEstablishments()
        {
            return _userRoles.Select(x => x.Establishment).ToList();
        }

        public void FetchActiveEstablishmentFromHttpHeader(HttpContext httpContext)
        {

            string EstablishmentIdAsString = httpContext.Request.Headers["EstablishmentId"];

            if (!EstablishmentIdAsString.IsNullOrEmpty())
            {
                Guid EstablishmentId = Guid.Parse(EstablishmentIdAsString);
                bool isUserAssociatedWithEstablishment = GetAccessibleEstablishments().Any(x => x.Id == EstablishmentId);
                if (isUserAssociatedWithEstablishment)
                {
                    _activeEstablishment = _establishmentRepository.GetById(EstablishmentId);
                }
            }
        }

        public List<UserRole>? GetAllUserRoles()
        {
            return this._userRoles;
        }

        public UserRole? GetActiveUserRole()
        {
            return this._userRoles.Find(x => x.Establishment == this.GetActiveEstablishment());
        }

    }
}
