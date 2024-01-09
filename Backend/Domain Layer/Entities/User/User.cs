using System.Text.RegularExpressions;
using WebApplication1.Domain_Layer.Services.Repositories;

namespace WebApplication1.Domain_Layer.Entities
{
    public class User : EntityBase
    {
        private readonly IUserRepository userRepository;
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

        public void SetEmail(string email)
        {
            if (!this.IsEmailValid(email)) throw new Exception("Email is not valid");
            this.Email = email;
        }

        public void SetPassword(string password)
        {
            if (!this.IsPasswordValid(password)) throw new Exception("Password is not valid");
            this.Password = password;
        }

        private bool IsPasswordValid(string password) => password.Length >= 8;

        private bool IsEmailValid(string email)
        {
            string pattern = @"^(?i)[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

    }
}
