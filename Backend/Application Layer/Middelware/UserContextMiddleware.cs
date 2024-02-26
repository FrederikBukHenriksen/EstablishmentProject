using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services;

namespace WebApplication1.Middelware
{
    public class UserContextMiddleware : IMiddleware
    {
        private IAuthenticationService authService;
        private IUserContextService userContextService;

        public UserContextMiddleware(IAuthenticationService authService, IUserContextService userContextService)
        {
            this.authService = authService;
            this.userContextService = userContextService;
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            User? user = this.authService.ExtractUserFromJwt(context);
            if (user != null)
            {
                this.userContextService.SetUser(user);
            };
            return next(context);
        }
    }
}
