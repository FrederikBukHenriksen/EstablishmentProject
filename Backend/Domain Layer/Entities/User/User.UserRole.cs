namespace WebApplication1.Domain_Layer.Entities
{

    public interface IUser_UserRole
    {
        public UserRole CreateUserRole(Establishment establishment, User user, Role role);
        public void AddUserRole(UserRole userRole);
        public void RemoveUserRole(UserRole userRole);
        public List<UserRole> GetUserRoles();

    }

    public partial class User : IUser_UserRole
    {
        public void AddUserRole(UserRole userRole)
        {
            this.UserRoleMustNotExist(userRole);
            this.UserRoleMustNotAlreadyExistForTheSameEstablishment(userRole.Establishment);
            this.UserRoles.Add(userRole);
        }

        public UserRole CreateUserRole(Establishment establishment, User user, Role role)
        {
            return new UserRole(this, establishment, role);
        }


        public List<UserRole> GetUserRoles()
        {
            return this.UserRoles.ToList();
        }

        public void RemoveUserRole(UserRole userRole)
        {
            this.UserRoleMustExist(userRole);
            this.UserRoles.Remove(userRole);
            this.AddUserRole(userRole);
        }

        //Checkers and validators

        protected void UserRoleMustNotExist(UserRole userRole)
        {
            if (this.GetUserRoles().Contains(userRole)) throw new ArgumentException("Userrole already exists");
        }

        protected void UserRoleMustExist(UserRole userRole)
        {
            if (!this.GetUserRoles().Contains(userRole)) throw new InvalidOperationException("Userrole does not exist");
        }

        protected bool DoesUserRoleExist(UserRole userRole)
        {
            return this.GetUserRoles().Contains(userRole);
        }

        protected void UserRoleMustNotAlreadyExistForTheSameEstablishment(Establishment establishment)
        {
            if (this.GetUserRoles().Any(x => x.Establishment == establishment)) throw new InvalidOperationException("User already has a userrole for establishment");
        }
        protected bool DoesRoleForEstablishmentExist(Establishment establishment)
        {
            return this.GetUserRoles().Any(x => x.Establishment == establishment);
        }


    }
}
