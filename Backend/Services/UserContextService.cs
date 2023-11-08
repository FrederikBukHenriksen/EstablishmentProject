using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public interface IUserContextService
    {
        public void SetUser(Guid userId);
        public User? GetUser();
        Establishment? GetActiveEstablishment();
        IEnumerable<Establishment>? GetAccessibleEstablishments();
        void SetEstablishment(Guid establishmentId);
    }

    public class UserContextService : IUserContextService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRolesRepository _userRolesRepository;
        private readonly IEstablishmentRepository _establishmentRepository;

        private User? _user = null;
        private Establishment? _establishment = null;
        private IEnumerable<Establishment>? _establishments = null;

        public UserContextService(IUserRepository userRepository,IUserRolesRepository userRolesRepository, IEstablishmentRepository establishmentRepository)
        {
            this._userRepository = userRepository;
            this._userRolesRepository = userRolesRepository;
            this._establishmentRepository = establishmentRepository;
        }

        public void SetUser(Guid userId)
        {
            var testGetAll = _userRepository.GetAll();
            _user = _userRepository.Get(userId);
            if (_user == null)
            {
                return;
            }
            this.FetchEstablishments();
        }

        public void SetEstablishment(Guid establishmentId)
        {
            if (_user == null)
            {
                throw new Exception("User must be set prior to establishment");
            }
            if (_establishment == null)
            {
                throw new Exception("User does not have access to any establishments");
            }
            var containsEstablishment = this._establishments!.Any(x => x.Id == establishmentId);
            if (!containsEstablishment)
            {
                throw new Exception("User does not have access to the establishment");
            }
            _establishment = _establishmentRepository.Get(establishmentId);
        }

        public User? GetUser()
        {
            return _user;
        }

        public Establishment? GetActiveEstablishment()
        {
            return _establishment;
        }

        public IEnumerable<Establishment>? GetAccessibleEstablishments()
        {
            return _establishments;
        }

        private void FetchEstablishments()
        {
            if (_user == null)
            {
                return;
            }
            this._establishments = _userRolesRepository.GetAllInclude().Where(x => x.User.Id == _user.Id).Select(x => x.Establishment);
        }






    }


}
