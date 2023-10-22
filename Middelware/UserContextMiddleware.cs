using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
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
            Guid? userId = _authService.GetUserGuid(context);

            _userContextService.UserId = userId;
            
            return next(context);
        }
    }
}
