using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services;

namespace WebApplication1.Middelware
{
    public class UserContextMiddleware : IMiddleware
    {
        private IAuthService authService;
        private IUserContextService userContextService;

        public UserContextMiddleware(IAuthService authService, IUserContextService userContextService)
        {
            this.authService = authService;
            this.userContextService = userContextService;
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            User? user = this.authService.GetUserFromHttp(context);
            if (user != null)
            {
                this.userContextService.SetUser(user);
            };
            return next(context);
        }
    }
}
