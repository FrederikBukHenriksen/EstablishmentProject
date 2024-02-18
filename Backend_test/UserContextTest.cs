using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Application_Layer.Services;
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
        private IUnitOfWork unitOfWork;
        private IAuthService _authService;

        //Arrange
        private Establishment establishment;
        private Establishment establishment2;

        private User userWithUserRole;
        private User userNoUserRole;

        public UserContextTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            clearDatabase();
            //Services
            _authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
            _userContextMiddleware = scope.ServiceProvider.GetRequiredService<UserContextMiddleware>();
            _userContextService = scope.ServiceProvider.GetRequiredService<IUserContextService>();
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            CommonArrange();
        }

        private void CommonArrange()
        {
            establishment = new Establishment("Test Establishment");

            userWithUserRole = new User("Frederik@mail.com", "12345678");
            var userRole = userWithUserRole.CreateUserRole(establishment, userWithUserRole, Role.Admin);
            userWithUserRole.AddUserRole(userRole);

            userNoUserRole = new User("Lydia@mail.com", "12345678");

            using (var uow = unitOfWork)
            {
                uow.establishmentRepository.Add(establishment);
                uow.userRepository.Add(userWithUserRole);
                uow.userRepository.Add(userNoUserRole);
            }
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

            //ASSERT
            Assert.NotNull(actualUser);
            Assert.Equal(userWithUserRole.Id, actualUser.Id);
            Assert.True(!actualUser.UserRoles.IsNullOrEmpty());
            Assert.Equal(Role.Admin, actualUser.UserRoles.First().Role);
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

            //ASSERT
            Assert.NotNull(actualUser);
            Assert.Equal(userNoUserRole.Id, actualUser.Id);
            Assert.True(actualUser.UserRoles.IsNullOrEmpty());
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
            Action act = () => _userContextService.GetUser();

            //Assert
            Assert.Throws<InvalidOperationException>(act);
        }
    }
}
