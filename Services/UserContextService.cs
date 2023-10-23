using Microsoft.IdentityModel.Tokens;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public interface IUserContextService
    {
        public void SetUser(Guid userId);
        public User? GetUser();
        public List<Establishment>? GetEstablishments();
    }

    public class UserContextService : IUserContextService
    {
        private readonly IUserRepository _userRepository;

        private List<Establishment>? _establishments = null;
        private Boolean _establishmentsLoaded = false;
        private User? _user = null;

        public UserContextService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void SetUser(Guid userId)
        {
            _user = _userRepository.Get(userId);
        }

        public User? GetUser()
        {
            return _user;
        }

        public List<Establishment>? GetEstablishments()
        {
            //if (_user == null)
            //{
            //    throw new Exception();
            //}
            //if (!_establishmentsLoaded)
            //{
            //    _establishments = _user.Establishments.ToList();
            //    if (_establishments.IsNullOrEmpty())
            //    {
            //        _establishments = null;
            //    }
            //    _establishmentsLoaded = true;
            //}
            return _establishments;
        }
    }


}
