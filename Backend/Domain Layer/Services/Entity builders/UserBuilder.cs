using System.Data;
using System.Text.RegularExpressions;
using WebApplication1.Domain_Layer.Services.Repositories;
using WebApplication1.Domain_Layer.Services.Entity_builders;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface IUserBuilder : IEntityBuilder<User>
    {
        IUserBuilder WithEmail(string email);
        IUserBuilder WithPassword(string password);
        IUserBuilder WithUserRoles(ICollection<(Establishment establishment, Role role)> establishmentAndRole);

    }
    public class UserBuilder : EntityBuilderBase<User>, IUserBuilder
    {
        private IUserRepository userRepository;

        private string? builderEmail = null;
        private string? builderPassword = null;
        private List<UserRole>? builderUserRoles = null;

        public UserBuilder(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public override void ReadPropertiesOfEntity(User entity)
        {
            this.builderEmail = entity.Email;
            this.builderPassword = entity.Password;
        }

        public override void WritePropertiesOfEntity(User Entity)
        {
            Entity.Email = (string)this.builderEmail;
            Entity.Password = (string)this.builderPassword;
            Entity.UserRoles = (List<UserRole>)this.builderUserRoles;
        }

        public IUserBuilder WithEmail(string email)
        {
            var emailLowerCase = email.ToLower();
            this.builderEmail = emailLowerCase;
            return this;
        }

        public IUserBuilder WithPassword(string password)
        {
            this.builderPassword = password;
            return this;
        }

        public override bool Validation()
        {
            if (!this.IsEmailValid(this.builderEmail)) throw new Exception("Email is not valid");
            if (!this.IsEmailUnique(this.builderEmail)) throw new Exception("Email is not unique");
            if (!this.IsPasswordValid(this.builderPassword)) throw new Exception("Password is not valid");
            return true;
        }

        private bool IsPasswordValid(string password) => password.Length >= 8;

        private bool IsEmailValid(string email)
        {
            string pattern = @"^(?i)[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        private bool IsEmailUnique(string email) => !(this.userRepository.GetAll().Any(u => u.Email == this.builderEmail));

        public IUserBuilder WithUserRoles(ICollection<(Establishment establishment, Role role)> establishmentAndRole)
        {
            if (this.builderUserRoles == null)
            {
                this.builderUserRoles = new List<UserRole>();
            }
            this.builderUserRoles = establishmentAndRole.Select(x => new UserRole
            {
                Establishment = x.establishment,
                Role = x.role
            }).ToList();
            return this;
        }
    }
}
