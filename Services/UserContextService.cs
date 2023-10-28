using Microsoft.IdentityModel.Tokens;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public interface IUserContextService
    {
        public void SetUser(Guid userId);
        public User? GetUser();
    }

    public class UserContextService : IUserContextService
    {
        private readonly IUserRepository _userRepository;
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
            if( _user == null)
            {
                throw new Exception("User not found");
            }
            return _user;
        }


    }


}
