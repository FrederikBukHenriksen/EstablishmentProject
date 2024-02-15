using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using WebApplication1.CommandHandlers;
using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test
{
    public class LoginTest : BaseIntegrationTest
    {
        private const string apiLogin = "/api/authentication/login";
        private const string apiLogout = "/api/authentication/logout";
        private const string apiIsLoggedIn = "/api/authentication/is-logged-in";

        public LoginTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            clearDatabase();

            List<User> users = new List<User> {
                new User("frederik@mail.com","hello123"),
                new User("lydia@mail.com","goodbye123")
                //factoryServiceBuilder.UserBuilder().WithEmail("frederik@mail.com").WithPassword("hello123").Build(),
                //factoryServiceBuilder.UserBuilder().WithEmail("lydia@mail.com").WithPassword("goodbye123").Build(),
            };

            dbContext.Set<User>().AddRange(users);
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task Login_WithCorrectCredentials_ShouldLogIn()
        {
            // ARRANGE
            var username = "frederik@mail.com";
            var password = "hello123";

            // ACT
            var loginResponse = await login(username, password);

            var jwtToken = extractJwtTokenHelper(loginResponse);

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
        public async Task IsLoggedIn_WithUserLogginIn_ShouldReturnTrue()
        {
            //ARRANGE
            Assert.Equal(HttpStatusCode.OK, (await login("frederik@mail.com", "hello123")).StatusCode);

            //ACT
            HttpResponseMessage isLoggedinResponse = await httpClient.GetAsync(apiIsLoggedIn);

            //ASSERT
            Assert.Equal(HttpStatusCode.OK, isLoggedinResponse.StatusCode);

            string responseContent = await isLoggedinResponse.Content.ReadAsStringAsync();
            Assert.Equal("true", responseContent);
        }

        [Fact]
        public async void IsLoggedIn_WithNoUserBeingLoggedIn_ShouldReturnFalse()
        {
            //ARRANGE
            Assert.Equal("false", await (await isLoggedIn()).Content.ReadAsStringAsync());

            //ACT
            HttpResponseMessage isLoggedinResponse = await httpClient.GetAsync(apiIsLoggedIn);

            //ASSERT
            Assert.Equal(HttpStatusCode.OK, isLoggedinResponse.StatusCode);

            string responseContent = await isLoggedinResponse.Content.ReadAsStringAsync();
            Assert.Equal("false", responseContent);
        }

        [Fact]
        public async void LogOut_WhenLoggedinUserLogsOut_ShouldBeLoggedOut()
        {
            //Arrange
            Assert.Equal(HttpStatusCode.OK, (await login("frederik@mail.com", "hello123")).StatusCode);
            Assert.Equal("true", await (await isLoggedIn()).Content.ReadAsStringAsync());

            //ACT
            HttpResponseMessage logoutResponse = await logOut();

            //ASSERT
            Assert.Equal(HttpStatusCode.OK, logoutResponse.StatusCode);
            var read = await (await isLoggedIn()).Content.ReadAsStringAsync();
            Assert.Equal("false", await (await isLoggedIn()).Content.ReadAsStringAsync());
            Assert.False(httpClient.DefaultRequestHeaders.TryGetValues("Cookie", out var cookieHeaderValues));
        }

        [Fact]
        public async void LogOut_WhenNoUserLoggedIn_ShouldNotBeLoggedIn()
        {
            //Arrange
            Assert.Equal("false", await (await isLoggedIn()).Content.ReadAsStringAsync());

            //ACT
            HttpResponseMessage logoutResponse = await logOut();

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, logoutResponse.StatusCode);
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

        private async Task<HttpResponseMessage> login(string username, string password)
        {
            var loginCommand = new LoginCommand
            {
                Username = username,
                Password = password
            };
            var response = await httpClient.PostAsJsonAsync(apiLogin, loginCommand);
            var jwtToken = extractJwtTokenHelper(response);
            httpClient.DefaultRequestHeaders.Add("Cookie", $"jwt={jwtToken}");
            return response;
        }

        private async Task<HttpResponseMessage> isLoggedIn()
        {
            var response = await httpClient.GetAsync(apiIsLoggedIn);

            return response;

        }

        private async Task<HttpResponseMessage> logOut()
        {
            var response = await httpClient.GetAsync(apiLogout);
            httpClient = webApplicationFactory.CreateDefaultClient();
            return response;
        }
    }
}