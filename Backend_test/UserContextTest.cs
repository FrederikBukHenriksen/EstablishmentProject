using Establishment.Test;
using Microsoft.AspNetCore.Http;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Test
{
    public class UserContextTest : BaseIntegrationTest
    {
        private IMiddleware _userContextMiddleware;
        private IUserContextService _userContextService;

        public UserContextTest(IntegrationTestWebAppFactory factory, IMiddleware userContextMiddleware, IUserContextService userContextService) : base(factory)
        {
            _userContextMiddleware = userContextMiddleware;
            _userContextService = userContextService;

            //Arrange
            List<User> users = new List<User> {
                new User()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000000"),
            Username = "Frederik",
                    Password = "LydiaErSød"
                }
            };
            dbContext.Set<User>().AddRange(users);
            dbContext.SaveChanges();
        }

        public static class UserContextData
        {
            public static IEnumerable<object[]> Data =>
                new List<object[]>
                {
                    //Correct logins
                                new object[] { "00000000-0000-0000-0000-000000000000",                 new User()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000000"),
            Username = "Frederik",
                    Password = "LydiaErSød"
                }},
                };
        }

        [Theory]
        [MemberData(nameof(UserContextData.Data), MemberType = typeof(UserContextData))]
        public async void UserContext(string jwtToken)
        {
            //Arrange
            //var jwtToken = this._authService.GenerateJwtToken(Guid.Empty);
            DefaultHttpContext httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer " + jwtToken;
            httpContext.Response.Cookies.Append("jwt", jwtToken);

            //Act
            _userContextMiddleware.InvokeAsync(httpContext, (context) => Task.CompletedTask);

            //Assert
            User expectedUser = new User()
            {
                Id = Guid.Empty,
                Username = "Frederik",
                Password = "LydiaErSød"
            };
            User? contextUser = _userContextService.GetUser();
            Assert.Equal(expectedUser, contextUser);

        }
    }
}
