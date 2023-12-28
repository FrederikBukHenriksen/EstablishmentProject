using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain_Layer.Services.Entity_builders;

namespace EstablishmentProject.test.Domain.Builders
{
    public class UserBuilderTest : BaseIntegrationTest
    {
        private IFactoryServiceBuilder factoryServiceBuilder;
        public UserBuilderTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            factoryServiceBuilder = scope.ServiceProvider.GetRequiredService<IFactoryServiceBuilder>();
        }

        [Fact]
        public void UseExistingEstablishment__Success()
        {
            //Arrange
            var user = factoryServiceBuilder.UserBuilder().WithEmail("Frederik@mail.com").WithPassword("12345678").Build();

            //Act
            var existingUser = factoryServiceBuilder.UserBuilder(user).Build();

            //Assert
            Assert.NotNull(user);
            Assert.NotNull(existingUser);
            Assert.Equal(user, existingUser);
        }

        [Fact]
        public void WithEmail__Success()
        {
            // Arrange
            var userBuilder = factoryServiceBuilder.UserBuilder().WithPassword("12345678");
            var email = "Frederik@mail.com";

            // Act
            var user = userBuilder.WithEmail(email).Build();

            // Assert
            Assert.NotNull(user);
            Assert.Equal(email, user.Email);
        }

        [Theory]
        [InlineData("Frederikmail.com")]
        [InlineData("Frederik@mailcom")]
        [InlineData("Fredrik @ mail.com")]
        [InlineData("@mail.com")]

        public void WithEmail__Failure__Email_Is_Not_Valid(string email)
        {
            //Arrange
            var userBuilder = factoryServiceBuilder.UserBuilder().WithPassword("12345678");

            User? user = null;

            //Act
            var exception = Record.Exception(() =>
            {
                user = userBuilder.WithEmail(email).Build();
            });

            //Assert
            Assert.NotNull(exception);
            Assert.IsType(typeof(System.Exception), exception);
            Assert.Equal("Email is not valid", exception.Message);
        }

        [Fact]
        public void WithEmail__Failure__Email_Is_Not_Unique()
        {
            //Arrange
            var user1 = factoryServiceBuilder.UserBuilder().WithEmail("Frederik@mail.com").WithPassword("12345678").Build();
            dbContext.Set<User>().Add(user1);
            dbContext.SaveChanges();

            var userBuilder2 = factoryServiceBuilder.UserBuilder();
            User? user2 = null;

            //Act
            var exception = Record.Exception(() =>
            {
                user2 = userBuilder2.WithEmail("Frederik@mail.com").WithPassword("12345678").Build();
            });

            //Assert
            Assert.Null(user2);
            Assert.NotNull(exception);
            Assert.IsType(typeof(System.Exception), exception);
            Assert.Equal("Email is not unique", exception.Message);
        }

        [Fact]
        public void WithPassword__Success()
        {
            //Arrange
            var userBuilder = factoryServiceBuilder.UserBuilder().WithEmail("Frederik@mail.com");
            var password = "12345678";

            //Act
            var user = userBuilder.WithPassword(password).Build();

            //Assert
            Assert.NotNull(user);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1234567")]

        public void WithPassword__Failure__Password_Is_Not_Valid(string password)
        {
            //Arrange
            var userBuilder = factoryServiceBuilder.UserBuilder().WithEmail("Frederik@mail.com");
            User? user = null;

            //Act
            var exception = Record.Exception(() =>
            {
                user = userBuilder.WithPassword(password).Build();
            });

            //Assert
            Assert.Null(user);
            Assert.NotNull(exception);
            Assert.IsType(typeof(System.Exception), exception);
            Assert.Equal("Password is not valid", exception.Message);
        }
    }
}
