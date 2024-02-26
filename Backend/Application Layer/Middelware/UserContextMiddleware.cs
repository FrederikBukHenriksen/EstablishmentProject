using WebApplication1.Application_Layer.Services.Authentication_and_login;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services;

namespace WebApplication1.Middelware
{
    public class UserContextMiddleware : IMiddleware
    {
        private IJWTService JWTService;
        private IUserContextService userContextService;

        public UserContextMiddleware(IUserContextService userContextService, IJWTService JWTService)
        {
            this.userContextService = userContextService;
            this.JWTService = JWTService;
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            User? user = this.JWTService.ExtractUserFromRequest(context);
            if (user != null)
            {
                this.userContextService.SetUser(user);
            };
            return next(context);
        }
    }
}
