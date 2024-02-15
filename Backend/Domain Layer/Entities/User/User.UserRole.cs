namespace WebApplication1.Domain_Layer.Entities
{

    public interface IUser_UserRole
    {
        public UserRole CreateUserRole(Establishment establishment, Role role);
        public void RemoveUserRole(UserRole userRole);
        public List<UserRole> GetUserRoles();
        public UserRole UpdateUserRole(UserRole userRole);
        public UserRole AddUserRole(UserRole userRole);

    }

    public partial class User : IUser_UserRole
    {
        public UserRole AddUserRole(UserRole userRole)
        {
            this.UserRoles.Add(userRole);
            return userRole;
        }

        public UserRole CreateUserRole(Establishment establishment, Role role)
        {
            return new UserRole(this, establishment, role);
        }

        public List<UserRole> GetUserRoles()
        {
            return this.UserRoles.ToList();
        }

        public void RemoveUserRole(UserRole userRole)
        {
            this.UserRoles.Remove(userRole);
        }

        public UserRole UpdateUserRole(UserRole userRole)
        {
            this.RemoveUserRole(userRole);
            this.AddUserRole(userRole);
            return userRole;
        }
    }
}
