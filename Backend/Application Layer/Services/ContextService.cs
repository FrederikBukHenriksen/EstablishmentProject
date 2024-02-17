using Microsoft.IdentityModel.Tokens;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Services
{
    public interface IUserContextService
    {
        public User GetUser();
        public Establishment GetActiveEstablishment();
        public List<Establishment> GetAccessibleEstablishments();
        public List<Guid> GetAccessibleEstablishmentsIds();
        public UserRole GetActiveUserRole();
        List<UserRole>? GetAllUserRoles();
        public void SetUser(User? user);
        public Establishment TrySetActiveEstablishment(Guid establishmentId);
        public void FetchActiveEstablishmentFromHttpHeader(HttpContext httpContext);
        public void FecthAccesibleEstablishments();
    }

    public class ContextService : IUserContextService
    {
        private IUnitOfWork unitOfWork;
        private User? _user = null;
        private Establishment? _activeEstablishment = null;
        private List<UserRole>? _userRoles = null;

        public ContextService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void SetUser(User? user)
        {
            this._user = user;
        }

        public User GetUser()
        {
            if (this._user == null)
            {
                throw new NullReferenceException();
            }
            return this._user;
        }

        public void FecthAccesibleEstablishments()
        {
            this._userRoles = this.unitOfWork.userRepository.GetById(this.GetUser().Id).UserRoles.ToList();
        }

        public Establishment GetActiveEstablishment()
        {
            if (this._activeEstablishment == null)
            {
                throw new NullReferenceException();
            }
            return this._activeEstablishment;
        }

        public List<Establishment> GetAccessibleEstablishments()
        {
            if (this._userRoles == null)
            {
                throw new NullReferenceException();
            }
            return this._userRoles.Select(x => x.Establishment).ToList();
        }

        public void FetchActiveEstablishmentFromHttpHeader(HttpContext httpContext)
        {

            string EstablishmentIdAsString = httpContext.Request.Headers["EstablishmentId"];

            if (!EstablishmentIdAsString.IsNullOrEmpty())
            {
                Guid EstablishmentId = Guid.Parse(EstablishmentIdAsString);
                bool isUserAssociatedWithEstablishment = this.GetAccessibleEstablishments().Any(x => x.Id == EstablishmentId);
                if (isUserAssociatedWithEstablishment)
                {
                    this._activeEstablishment = this.unitOfWork.establishmentRepository.GetById(EstablishmentId);
                }
            }
        }

        public List<UserRole>? GetAllUserRoles()
        {
            if (this._userRoles == null)
            {
                throw new NullReferenceException();
            }
            return this._userRoles;
        }

        public UserRole? GetActiveUserRole()
        {
            if (this._userRoles == null)
            {
                throw new NullReferenceException();
            }
            return this._userRoles.Find(x => x.Establishment == this.GetActiveEstablishment());
        }


        public Establishment TrySetActiveEstablishment(Guid establishmentId)
        {
            var accessibleEstablishments = this.GetAccessibleEstablishments();
            if (accessibleEstablishments.Any(x => x.Id == establishmentId))
            {
                return accessibleEstablishments.Find(x => x.Id == establishmentId);
            }
            throw new UnauthorizedAccessException();

        }

        public List<Guid> GetAccessibleEstablishmentsIds()
        {
            return this.GetAccessibleEstablishments().Select(x => x.Id).ToList();
        }
    }
}
