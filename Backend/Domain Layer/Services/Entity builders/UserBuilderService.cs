using WebApplication1.Domain.Services.Repositories;

namespace WebApplication1.Domain.Entities
{
    public interface IUserBuilderService
    {
        User CreateUser(string email, string password, ICollection<UserRole>? userRoles = null);
    }
    public class UserBuilderService
    {
        private readonly IUserRepository userRepository;

        public UserBuilderService(IUserRepository userRepository)
        {
        this.userRepository = userRepository;
        }

        public User CreateUser(string email, string password, ICollection<UserRole>? userRoles = null)
        {
            if (!IsEmailValid(email)) throw new ArgumentException("Email is not valid");

            if (!IsEmailUnique(email)) throw new ArgumentException("Email is used");

            if (!IsPasswordValid(password)) throw new ArgumentException("Password is not valid");
            
            var user = new User();
            user.Email = email;
            user.Password = password;
            user.UserRoles = userRoles == null ? new List<UserRole>() : userRoles;
            return user;
        }

        private static bool IsPasswordValid(string password) => password.Length >= 8;

        private static bool IsEmailValid(string username) => username.Contains("@");

        private bool IsEmailUnique(string username) => this.userRepository.GetAll().Any(u => u.Email == username);
    }
}
