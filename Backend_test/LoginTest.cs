using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using WebApplication1.CommandHandlers;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Entity_builders;

namespace EstablishmentProject.test
{
    public class LoginTest : BaseIntegrationTest
    {
        private const string apiLogin = "/api/authentication/login";
        private const string apiLogout = "/api/authentication/logout";
        private const string apiIsLoggedIn = "/api/authentication/is-logged-in";
        private const string apiGetLoggedInUser = "/api/authentication/get-logged-in-user";

        private IFactoryServiceBuilder factoryServiceBuilder;

        public LoginTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            clearDatabase();
            factoryServiceBuilder = scope.ServiceProvider.GetRequiredService<IFactoryServiceBuilder>();

            List<User> users = new List<User> {
                factoryServiceBuilder.UserBuilder().WithEmail("frederik@mail.com").WithPassword("hello123").Build(),
                factoryServiceBuilder.UserBuilder().WithEmail("lydia@mail.com").WithPassword("goodbye123").Build(),
            };

            dbContext.Set<User>().AddRange(users);
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task Login_WithCorrectCredentials_WithExactCaing_ShouldLogIn()
        {
            // ARRANGE
            var loginCommand = new LoginCommand
            {
                Username = "frederik@mail.com",
                Password = "hello123"
            };

            // ACT
            var loginResponse = await httpClient.PostAsJsonAsync(apiLogin, loginCommand);

            var jwtToken = extractJwtTokenHelper(loginResponse);
            httpClient.DefaultRequestHeaders.Add("Cookie", $"jwt={jwtToken}");

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
            Assert.NotNull(jwtToken);
        }


        [Fact]
        public async Task Login_WithCorrectCredentials_WithMixedCaseUsername_ShouldLogIn()
        {
            // ARRANGE
            var loginCommand = new LoginCommand
            {
                Username = "FrEDErIk@MAiL.cOM",
                Password = "hello123"
            };

            // ACT
            var loginResponse = await httpClient.PostAsJsonAsync(apiLogin, loginCommand);

            var jwtToken = extractJwtTokenHelper(loginResponse);
            httpClient.DefaultRequestHeaders.Add("Cookie", $"jwt={jwtToken}");

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
            Assert.NotNull(jwtToken);
        }


        [Fact]
        public async Task Login_Success_ValidUsernameAndPassword()
        {
            // ARRANGE
            var loginCommand = new LoginCommand
            {
                Username = "frederik@mail.com",
                Password = "hello123"
            };

            // ACT
            var loginResponse = await httpClient.PostAsJsonAsync(apiLogin, loginCommand);

            var jwtToken = extractJwtTokenHelper(loginResponse);
            httpClient.DefaultRequestHeaders.Add("Cookie", $"jwt={jwtToken}");

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
            Assert.NotNull(jwtToken);
        }

        [Fact]
        public async Task Login_Success_CaseInsensitiveUsername()
        {
            // ARRANGE
            var loginCommand = new LoginCommand
            {
                Username = "Frederik@Mail.COM",
                Password = "hello123"
            };

            // ACT
            var loginResponse = await httpClient.PostAsJsonAsync(apiLogin, loginCommand);

            var jwtToken = extractJwtTokenHelper(loginResponse);
            httpClient.DefaultRequestHeaders.Add("Cookie", $"jwt={jwtToken}");

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
            Assert.NotNull(jwtToken);
        }


        [Fact]
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
        public async void LogOut_WhenUserLogsOut_ShouldLogOut()
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