using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;

namespace WebApplication1.Domain_Layer.Services.Entity_services
{
    public class UserEntityService
    {
        private IEstablishmentRepository establishmentRepository;
        private IUserRepository userRepository;
        private IUserRolesRepository userRolesRepository;

        public UserEntityService(IEstablishmentRepository establishmentRepository, IUserRepository userRepository, IUserRolesRepository userRolesRepository)
        {
            this.establishmentRepository = establishmentRepository;
            this.userRepository = userRepository;
            this.userRolesRepository = userRolesRepository;
        }

        public void AddUser(string username, string password)
        {
            if(isUsernameAnEmail(username) && isUsernameUniqe(username))
            {
                User user = new User(username, password);
                userRepository.Add(user);
            }
        }

        public void AddUserRole(Guid userId, Guid establishmentId, Role role)
        {
            User user = userRepository.GetById(userId);
            Establishment establishment = establishmentRepository.GetById(establishmentId);

            if(user == null || establishment == null)
            {
                throw new Exception();
            }
            UserRole userRole = new UserRole(user, establishment, role);
            userRolesRepository.Add(userRole);
        }

        private bool verifyUsername(string username)
        {
            return isUsernameAnEmail(username) && isUsernameUniqe(username);
        }

        private bool isUsernameAnEmail(string username)
        {
            if (username.Contains("@") && username.Contains("."))
            {
                return true;
            }
            else
            {
                throw new Exception("Username is not an email");
            }
        }

        private bool isUsernameUniqe(string username)
        {
            if(userRepository.Find(x => x.Username == username) == null)
            {
                return true;
            }
            else
            {
                throw new Exception("Username is not an email");
            }
        }
    }
}
