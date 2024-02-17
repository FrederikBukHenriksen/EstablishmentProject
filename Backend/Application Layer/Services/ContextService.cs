using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Services
{
    public interface IUserContextService
    {
        User GetUser();
        void SetUser(User user);
    }

    public class ContextService : IUserContextService
    {
        private User? User = null;

        public void SetUser(User user)
        {
            this.User = user;
        }

        public User GetUser()
        {
            if (this.User == null)
            {
                throw new InvalidOperationException("User profile was not found");
            }
            return this.User;
        }
    }
}
