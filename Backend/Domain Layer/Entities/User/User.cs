using System.Text.RegularExpressions;

namespace WebApplication1.Domain_Layer.Entities
{

    public interface IUser
    {
        void SetEmail(string email);
        string GetEmail();
        void SetPassword(string password);
        string GetPassword();
    }


    public partial class User : EntityBase, IUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();


        public User()
        {

        }
        public User(string email, string password)
        {
            this.SetEmail(email);
            this.SetPassword(password);
        }

        public void SetEmail(string email)
        {
            this.EmailMustBeValid(email);
            this.Email = email;
        }

        public string GetEmail() { return this.Email; }

        public void SetPassword(string password)
        {
            this.PasswordMustBeValid(password);
            this.Password = password;
        }

        public string GetPassword() { return this.Password; }



        protected void PasswordMustBeValid(string password)
        {
            if (!this.IsPasswordValid(password)) throw new ArgumentException("Password is not valid");
        }
        private bool IsPasswordValid(string password) => password.Length >= 8;

        protected void EmailMustBeValid(string email)
        {
            if (!this.IsEmailValid(email)) throw new ArgumentException("Email is not valid");
        }

        private bool IsEmailValid(string email)
        {
            string pattern = @"^(?i)[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

    }
}
