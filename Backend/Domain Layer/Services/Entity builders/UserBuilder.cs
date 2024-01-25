﻿using System.Data;
using WebApplication1.Domain_Layer.Services.Entity_builders;
using WebApplication1.Domain_Layer.Services.Repositories;

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

        public override void ConstructEntity(User Entity)
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
