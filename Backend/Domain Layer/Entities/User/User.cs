using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace WebApplication1.Domain_Layer.Entities
{

    public interface IUser
    {
        void SetEmail(string email);
        void SetPassword(string password);
    }


    public partial class User : EntityBase, IUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();


        public User()
        {

        }
        public User(string email, string password, List<(Establishment, Role)>? userRoles = null)
        {
            this.SetEmail(email);
            this.SetPassword(password);
            if (!userRoles.IsNullOrEmpty())
            {
                userRoles!.ForEach(x => this.AddUserRole(this.CreateUserRole(x.Item1, x.Item2)));
            }
        }

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
