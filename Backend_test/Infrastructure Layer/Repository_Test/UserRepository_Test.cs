using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Data;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;

namespace EstablishmentProject.test.Infrastructure_Layer.DTO
{
    public class UserRepository_Test : IntegrationTest
    {
        private ApplicationDbContext applicationDbContext;
        private EstablishmentRepository establishmentRepository;
        private UserRepository userRepository;
        private Establishment establishment;
        private User user;
        private UserRole userRole;

        public UserRepository_Test() : base([DatabaseTestContainer.CreateAsync().Result])
        {
            applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            establishmentRepository = new EstablishmentRepository(applicationDbContext);
            userRepository = new UserRepository(applicationDbContext);
            establishment = new Establishment("Test Establishment");
            user = new User("Frederik@mail.com", "12345678");
            userRole = user.CreateUserRole(establishment, user, Role.Admin);
            user.AddUserRole(userRole);
        }

        [Fact]
        public async Task UsingLazyLoading_WithUserRoles_ShouldBeAbleToAccessRelatedData()
        {
            // Arrange

            // Act
            User userWithRoles = userRepository.GetById(user.Id);

            // Assert
            Assert.Equal(userRole, userWithRoles.GetUserRoles()[0]);
            Assert.Equal(establishment, userWithRoles.GetUserRoles()[0].Establishment);
        }


        [Fact]
        public async Task IncludeUserRoles_WithUser_ShouldIncludeUserRoles()
        {
            // Arrange

            // Act
            userRepository.IncludeUserRoles();
            User userWithRoles = userRepository.GetById(user.Id);

            // Assert
            Assert.Equal(userRole, userWithRoles.GetUserRoles()[0]);
        }
    }
}
