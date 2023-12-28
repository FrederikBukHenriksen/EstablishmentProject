using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using WebApplication1.CommandHandlers;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain_Layer.Services.Entity_builders;

namespace EstablishmentProject.test
{
    [Collection("Login")]
    public class LoginTest : BaseIntegrationTest
    {
        private const string apiLogin = "/api/authentication/login";
        private const string apiLogout = "/api/authentication/logout";
        private const string apiIsLoggedIn = "/api/authentication/is-logged-in";
        private const string apiGetLoggedInUser = "/api/authentication/get-logged-in-user";

        private IFactoryServiceBuilder factoryServiceBuilder;

        public LoginTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            factoryServiceBuilder = scope.ServiceProvider.GetRequiredService<IFactoryServiceBuilder>();

            List<User> users = new List<User> {
                factoryServiceBuilder.UserBuilder().WithEmail("frederik@mail.com").WithPassword("hello123").Build(),
                factoryServiceBuilder.UserBuilder().WithEmail("lydia@mail.com").WithPassword("goodbye123").Build(),
            };

            dbContext.Set<User>().AddRange(users);
            dbContext.SaveChanges();
        }

        [Theory]
        [InlineData("frederik@mail.com", "hello123")]
        [InlineData("Frederik@Mail.COM", "hello123")]
        public async void Login__Success(string username, string password)
        {
            //ARRANGE
            LoginCommand LoginCommand = new LoginCommand
            {
                Username = username,
                Password = password
            };

            //ACT
            HttpResponseMessage loginResponse = await httpClient.PostAsJsonAsync(apiLogin, LoginCommand);

            string jwtToken = extractJwtTokenHelper(loginResponse);
            httpClient.DefaultRequestHeaders.Add("Cookie", $"jwt={jwtToken}");


            //ASSERT
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
            Assert.NotNull(jwtToken);

        }

        [Theory]
        [InlineData("rederik@mail.com", "hello123")] //Wrong username
        [InlineData("frederik@mail.com", "hallo123")] //Misspelled password
        [InlineData("frederik@mail.com", "Hallo123")] //Usser case password
        public async void Login__Failure__Wrong_Credentials(string username, string password)
        {
            //ARRANGE
            LoginCommand LoginCommand = new LoginCommand
            {
                Username = username,
                Password = password
            };

            //ACT
            HttpResponseMessage loginResponse = await httpClient.PostAsJsonAsync(apiLogin, LoginCommand);

            string jwtToken = extractJwtTokenHelper(loginResponse);
            httpClient.DefaultRequestHeaders.Add("Cookie", $"jwt={jwtToken}");


            //ASSERT
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
            Assert.NotNull(jwtToken);

        }

        public async void IsLoggedIn__Success__A_User_Is_Logged_In()
        {
            //ARRANGE
            LoginCommand LoginCommand = new LoginCommand
            {
                Username = "frederik@mail.com",
                Password = "hello123"
            };

            //ACT
            HttpResponseMessage loginResponse = await httpClient.PostAsJsonAsync(apiLogin, LoginCommand);

            HttpResponseMessage isLoggedinResponse = await httpClient.GetAsync(apiIsLoggedIn);


            //ASSERT
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, isLoggedinResponse.StatusCode);

            string responseContent = await isLoggedinResponse.Content.ReadAsStringAsync();
            Assert.Equal("true", responseContent);
        }

        [Fact]
        public async void IsLoggedIn__Success__A_User_Is_Not_Logged_in()
        {
            //ACT
            HttpResponseMessage isLoggedinResponse = await httpClient.GetAsync(apiIsLoggedIn);

            //ASSERT
            Assert.Equal(HttpStatusCode.OK, isLoggedinResponse.StatusCode);

            string responseContent = await isLoggedinResponse.Content.ReadAsStringAsync();
            Assert.Equal("false", responseContent);
        }

        [Fact]
        public async void Logout__Success()
        {
            //ARRANGE
            LoginCommand LoginCommand = new LoginCommand
            {
                Username = "frederik@mail.com",
                Password = "hello123"
            };

            HttpResponseMessage loginResponse = await httpClient.PostAsJsonAsync(apiLogin, LoginCommand);
            HttpResponseMessage isLoggedinResponsePre = await httpClient.GetAsync(apiIsLoggedIn);

            //ACT
            HttpResponseMessage logoutResponse = await httpClient.GetAsync(apiLogout);

            //ASSERT
            //Assert the user is logged in
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, isLoggedinResponsePre.StatusCode);
            string isLoggedinResponsePreContent = await isLoggedinResponsePre.Content.ReadAsStringAsync();
            Assert.Equal("true", isLoggedinResponsePreContent);

            //Assert the logout has happened
            Assert.Equal(HttpStatusCode.OK, logoutResponse.StatusCode);

            //Assert the user is logged out
            HttpResponseMessage isLoggedinResponsePost = await httpClient.GetAsync(apiIsLoggedIn);
            string isLoggedinResponsePostContent = await isLoggedinResponsePre.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, isLoggedinResponsePost.StatusCode);
            Assert.Equal("false", isLoggedinResponsePostContent);


        }

        private string extractJwtTokenHelper(HttpResponseMessage response)
        {
            if (!response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string>? cookieValues))
            {
                throw new Exception("Set-Cookie header not found in the response.");
            }

            string cookieString = cookieValues.FirstOrDefault();

            string pattern = @"jwt=([^;]+)";
            Match match = Regex.Match(cookieString, pattern);

            if (!match.Success)
            {
                throw new Exception("JWT token not found in the Set-Cookie header.");
            }

            return match.Groups[1].Value;
        }

    }
}