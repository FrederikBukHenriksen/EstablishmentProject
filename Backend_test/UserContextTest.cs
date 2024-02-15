using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Middelware;
using WebApplication1.Services;

namespace EstablishmentProject.test
{
    public class UserContextTest : BaseIntegrationTest
    {
        //Services
        private UserContextMiddleware _userContextMiddleware;
        private IUserContextService _userContextService;
        private IAuthService _authService;

        //Arrange
        private Establishment establishment1;
        private Establishment establishment2;
        private List<Establishment> allEstablishments;

        private User userWithUserRole;
        private User userNoUserRole;
        private List<User> allUsers;

        public UserContextTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            //Services
            _authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
            _userContextMiddleware = scope.ServiceProvider.GetRequiredService<UserContextMiddleware>();
            _userContextService = scope.ServiceProvider.GetRequiredService<IUserContextService>();

            //var database = dbContext.Database.GetConnectionString();
            //var ok = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //Arrange
            establishment1 = new Establishment { Name = "Cafe 1" };
            establishment2 = new Establishment { Name = "Cafe 2" };
            allEstablishments = new List<Establishment> { establishment1, establishment2 };
            dbContext.Set<Establishment>().AddRange(allEstablishments);

            userWithUserRole = new User("Frederik@mail.com", "12345678", new List<(Establishment, Role)> { (establishment1, Role.Admin) });
            userNoUserRole = new User("Lydia@mail.com", "12345678");

            allUsers = new List<User> { userWithUserRole, userNoUserRole };
            dbContext.Set<User>().AddRange(allUsers);

            dbContext.SaveChanges();
        }

        [Fact]
        public void valid_user_with_userRole()
        {
            //Arrange
            var jwtToken = _authService.GenerateJwtToken(userWithUserRole.Id);
            DefaultHttpContext httpMock = new DefaultHttpContext();
            httpMock.Request.Headers["Cookie"] = "jwt=" + jwtToken;

            //Act
            _userContextMiddleware.InvokeAsync(httpMock, (context) => Task.CompletedTask); //Send request to middleware
            User? actualUser = _userContextService.GetUser();
            List<UserRole>? actualUserRoles = _userContextService.GetAllUserRoles();

            //ASSERT
            User expectedUser = userWithUserRole;

            //Assert user
            Assert.NotNull(actualUser);
            Assert.Equal(expectedUser.Id, actualUser.Id);

            //Assert userrole belongs to user
            Assert.NotNull(actualUserRoles);
            Assert.True(actualUserRoles.All(x => x.User == expectedUser));

            //Assert users accesible establishment
            Assert.True(actualUserRoles.All(x => allEstablishments.Contains(x.Establishment)));

            //Assert users roles per establishment
            Assert.True(actualUserRoles.Any(x => x.Establishment == establishment1 && x.Role == Role.Admin));
        }

        [Fact]
        public void valid_user_with_no_userRole()
        {
            //ARRANGE
            var jwtToken = _authService.GenerateJwtToken(userNoUserRole.Id);
            DefaultHttpContext httpMock = new DefaultHttpContext();
            httpMock.Request.Headers["Cookie"] = "jwt=" + jwtToken;

            //ACT
            _userContextMiddleware.InvokeAsync(httpMock, (context) => Task.CompletedTask); //Send request to middleware
            User? actualUser = _userContextService.GetUser();
            ICollection<UserRole>? actualUserRoles = _userContextService.GetAllUserRoles();

            //ASSERT
            User expectedUser = userNoUserRole;

            //Assert user
            Assert.NotNull(actualUser);
            Assert.Equal(expectedUser.Id, actualUser.Id);

            //Assert userroles
            Assert.Null(actualUserRoles);
        }

        [Fact]
        public void invalid_user_with_no_userRole()
        {
            //ARRANGE
            var jwtToken = _authService.GenerateJwtToken(Guid.Empty);
            DefaultHttpContext httpMock = new DefaultHttpContext();
            httpMock.Request.Headers["Cookie"] = "jwt=" + jwtToken;

            //ACT
            _userContextMiddleware.InvokeAsync(httpMock, (context) => Task.CompletedTask); //Send request to middleware
            User? actualUser = _userContextService.GetUser();
            ICollection<UserRole>? actualUserRoles = _userContextService.GetAllUserRoles();

            //ASSERT

            //Assert user
            Assert.Null(actualUser);

            //Assert userroles
            Assert.Null(actualUserRoles);

        }
    }
}
