using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
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
            _authService = authService;
            _userContextService = userContextService;
        }
            
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            User? user = _authService.GetUserFromHttp(context);

            if (user != null)
            {
                _userContextService.SetUser(user);
                _userContextService.FecthAccesibleEstablishments();
                _userContextService.FetchActiveEstablishmentFromHttpHeader(context);
            };
            return next(context);
        }
    }
}
