using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test
{
    public class User_UserRole_Test
    {
        private Establishment establishment;
        private User user;

        public User_UserRole_Test()
        {
            CommonArrange();
        }
        private void CommonArrange()
        {
            establishment = new Establishment("Test establishment");
            user = new User("Frederik@mail.com", "12345678");
        }

        [Fact]
        public void CreateUserRole_ShouldCreateUserRole()
        {
            // Arrange
            Role role = Role.Admin;

            // Act
            UserRole userRole = user.CreateUserRole(establishment, user, role);

            // Assert
            Assert.NotNull(userRole);
            Assert.Equal(user, userRole.User);
            Assert.Equal(establishment, userRole.Establishment);
            Assert.Equal(role, userRole.Role);
        }

        [Fact]

        public void AddUserRole_ShouldAddUserRole()
        {
            // Arrange
            UserRole userRole = user.CreateUserRole(establishment, user, Role.Admin);

            // Act
            user.AddUserRole(userRole);

            // Assert
            Assert.Contains(userRole, user.GetUserRoles());
        }

        [Fact]
        public void AddUserRole_WithUserAlreadyHavingUserRoleForEstablishment_ShouldNotAddUserRole()
        {
            // Arrange
            UserRole existingUserRole = user.CreateUserRole(establishment, user, Role.Admin);
            user.AddUserRole(existingUserRole);

            UserRole userRole = user.CreateUserRole(establishment, user, Role.Admin);

            // Act
            Action act = () => user.AddUserRole(userRole);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void AddUserRole_WithUserRoleCreatedForDifferentUser_ShouldNotAddUserRole()
        {
            // Arrange
            User otherUser = new User("Lydia@mail.com", "12345678");
            UserRole otherUserRole = otherUser.CreateUserRole(establishment, otherUser, Role.Admin);
            otherUser.AddUserRole(otherUserRole);

            // Act
            Action act = () => user.AddUserRole(otherUserRole);

            //Assert
            Assert.Throws<InvalidOperationException>(act);
            Assert.Empty(user.GetUserRoles());
        }

        [Fact]
        public void RemoveUserRole_WithValidUserRole_ShouldRemoveUserRole()
        {
            // Arrange
            UserRole userRole = user.CreateUserRole(establishment, user, Role.Admin);
            user.AddUserRole(userRole);

            // Act
            user.RemoveUserRole(userRole);

            // Assert
            Assert.DoesNotContain(userRole, user.GetUserRoles());
        }


        [Fact]
        public void RemoveUserRole_WithUserRoleNotExisting_ShouldNotRemoveUserRole()
        {
            // Arrange
            UserRole userRole = user.CreateUserRole(establishment, user, Role.Admin);

            // Act
            Action act = () => user.RemoveUserRole(userRole);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void GetUserRoles_ShouldReturnUserRoles()
        {
            // Arrange
            UserRole userRole = user.CreateUserRole(establishment, user, Role.Admin);
            user.AddUserRole(userRole);

            // Act
            var result = user.GetUserRoles();

            // Assert
            Assert.Contains(userRole, result);
        }


    }
}
