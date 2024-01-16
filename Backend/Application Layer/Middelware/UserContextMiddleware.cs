using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services;

namespace WebApplication1.Middelware
{
    public class UserContextMiddleware : IMiddleware
    {
        private IAuthService _authService;
        private IUserContextService _userContextService;

        public UserContextMiddleware(IAuthService authService, IUserContextService userContextService)
        {
            this._authService = authService;
            this._userContextService = userContextService;
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            User? user = this._authService.GetUserFromHttp(context);

            if (user != null)
            {
                this._userContextService.SetUser(user);
                this._userContextService.FecthAccesibleEstablishments();
                this._userContextService.FetchActiveEstablishmentFromHttpHeader(context);
            };
            return next(context);
        }
    }
}
