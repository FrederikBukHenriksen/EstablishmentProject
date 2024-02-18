namespace WebApplication1.Domain_Layer.Entities
{

    public class UserRole : EntityBase
    {
        public virtual User User { get; set; }
        public virtual Establishment Establishment { get; set; }
        public Role Role { get; set; }

        public UserRole()
        {
            this.Id = Guid.NewGuid();
        }

        public UserRole(User user, Establishment establishment, Role role)
        {
            this.Id = Guid.NewGuid();
            this.User = user;
            this.Establishment = establishment;
            this.Role = role;
        }
    }


}
