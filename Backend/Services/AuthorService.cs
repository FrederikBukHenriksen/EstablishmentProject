//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;

//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
//public class AuthorServiceAttribute : Attribute, IAuthorizationFilter
//{
//    private readonly string _role;

//    public AuthorServiceAttribute(Role role)
//    {
//        _role = role.ToString();
//    }

//    public void OnAuthorization(AuthorizationFilterContext context)
//    {
//        // Check if the user is authenticated
//        if (!context.HttpContext.User.Identity.IsAuthenticated)
//        {
//            context.Result = new UnauthorizedResult();
//            return;
//        }

//        // Check if the user has the required role
//        if (!context.HttpContext.User.IsInRole(_role))
//        {
//            context.Result = new ForbidResult();
//            return;
//        }
//    }
//}
