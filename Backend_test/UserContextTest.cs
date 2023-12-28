using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Data;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain_Layer.Services.Entity_builders;
using WebApplication1.Middelware;
using WebApplication1.Services;

namespace EstablishmentProject.test
{
    public class UserContextTest : BaseIntegrationTest
    {
        //Services
        private UserContextMiddleware _userContextMiddleware;
        private IUserContextService _userContextService;
        private IFactoryServiceBuilder _factoryServiceBuilder;
        private IAuthService _authService;

        //Arrange
        private Establishment establishment1;
        private Establishment establishment2;
        private Establishment establishment3;
        private List<Establishment> allEstablishments;

        private User userFrederik;
        private User userLydia;
        private List<User> allUsers;

        public UserContextTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            //Services
            _authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
            _userContextMiddleware = scope.ServiceProvider.GetRequiredService<UserContextMiddleware>();
            _userContextService = scope.ServiceProvider.GetRequiredService<IUserContextService>();
            _factoryServiceBuilder = scope.ServiceProvider.GetRequiredService<IFactoryServiceBuilder>();

            var database = dbContext.Database.GetConnectionString();
            var ok = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //Arrange
            establishment1 = _factoryServiceBuilder
                .EstablishmentBuilder()
                .WithName("Cafe 1")
                .Build();
            establishment2 = _factoryServiceBuilder
                .EstablishmentBuilder()
                .WithName("Cafe 2")
                .Build();
            allEstablishments = new List<Establishment> { establishment1, establishment2 };
            dbContext.Set<Establishment>().AddRange(allEstablishments);

            userFrederik = _factoryServiceBuilder
                .UserBuilder()
                .WithEmail("Frederik@mail.com")
                .WithPassword("12345678")
                .WithUserRoles(new List<(Establishment, Role)> { (establishment1, Role.Admin) })
                .Build();
            userLydia = _factoryServiceBuilder
               .UserBuilder()
               .WithEmail("Lydia@mail.com")
               .WithPassword("12345678")
               .Build();
            allUsers = new List<User> { userFrederik, userLydia };
            dbContext.Set<User>().AddRange(allUsers);

            dbContext.SaveChanges();
        }

        [Fact]
        public void valid_user_with_userRole()
        {
            //Arrange
            var jwtToken = _authService.GenerateJwtToken(userFrederik.Id);
            DefaultHttpContext httpMock = new DefaultHttpContext();
            httpMock.Request.Headers["Cookie"] = "jwt=" + jwtToken;

            //Act
            _userContextMiddleware.InvokeAsync(httpMock, (context) => Task.CompletedTask); //Send request to middleware
            User? actualUser = _userContextService.GetUser();
            List<UserRole>? actualUserRoles = _userContextService.GetAllUserRoles();

            //ASSERT
            User expectedUser = userFrederik;

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
            var jwtToken = _authService.GenerateJwtToken(userLydia.Id);
            DefaultHttpContext httpMock = new DefaultHttpContext();
            httpMock.Request.Headers["Cookie"] = "jwt=" + jwtToken;

            //ACT
            _userContextMiddleware.InvokeAsync(httpMock, (context) => Task.CompletedTask); //Send request to middleware
            User? actualUser = _userContextService.GetUser();
            ICollection<UserRole>? actualUserRoles = _userContextService.GetAllUserRoles();

            //ASSERT
            User expectedUser = userLydia;

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
