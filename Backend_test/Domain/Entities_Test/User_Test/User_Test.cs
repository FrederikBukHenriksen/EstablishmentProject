using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test
{

    public class User_Test
    {
        private Establishment establishment;

        public User_Test()
        {
        }

        [Fact]
        public void Constructor_ShouldCreateUser()
        {
            // Arrange
            string email = "Frederik@mail.com";
            string password = "12345678";

            // Act
            User user = new User(email, password);

            //Arrange
            Assert.NotNull(user);
            Assert.Equal(email, user.Email);
            Assert.Equal(password, user.Password);
        }

        [Fact]
        public void SetEmail_WithValidEmail_ShouldSetEmail()
        {
            // Arrange
            User user = new User("name@mail.com", "12345678");
            string email = "Frederik@mail.com";

            // Act
            user.SetEmail(email);

            // Assert
            Assert.Equal(email, user.Email);
        }

        [Fact]
        public void SetEmail_WithEmailWithoutAt_ShouldNotSetEmail()
        {
            User user = new User("name@mail.com", "12345678");
            string email = "Frederikmail.com";

            // Act
            Action act = () => user.SetEmail(email);

            // Assert
            Assert.Throws<ArgumentException>(act);
            Assert.NotEqual(email, user.Email);
        }

        [Fact]
        public void SetEmail_WithEmailDotDoCom_ShouldNotSetEmail()
        {
            User user = new User("name@mail.com", "12345678");
            string email = "Frederikmailcom";

            // Act
            Action act = () => user.SetEmail(email);

            // Assert
            Assert.Throws<ArgumentException>(act);
            Assert.NotEqual(email, user.Email);
        }

        [Fact]
        public void SetPassword_WithValidPassword_ShouldSetPassword()
        {
            // Arrange
            User user = new User("Frederik@mail.com", "abcdefgh");
            string password = "1234567";

            // Act
            Action act = () => user.SetPassword(password);

            // Assert
            Assert.Throws<ArgumentException>(act);
            Assert.NotEqual(password, user.Password);

        }
    }
}
