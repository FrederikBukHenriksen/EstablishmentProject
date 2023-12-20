//using EstablishmentProject.Test;
//using System.Net;
//using System.Net.Http.Json;
//using System.Text.RegularExpressions;
//using WebApplication1.CommandHandlers;
//using WebApplication1.CommandsHandlersReturns;
//using WebApplication1.Domain.Entities;

//namespace EstablishmentProject.Test
//{
//    [Collection("Login")]
//    public class LoginTest : BaseIntegrationTest
//    {
//        private const string apiLogin = "/api/authentication/Login";
//        private const string apiIsLoggedIn = "/api/authentication/is-logged-in";

//        public LoginTest(IntegrationTestWebAppFactory factory) : base(factory)
//        {
//            //Arrange
//            List<User> users = new List<User> {
//                new User()
//                {
//                    Username = "Frederik",
//                    Password = "LydiaErSød"
//                },
//                new User()
//                {
//                    Username = "Lydia",
//                    Password = "FrederikErSød"
//                }
//            };
//            dbContext.Set<User>().AddRange(users);
//            dbContext.SaveChanges();
//        }

//        public static class LoginTestParameterData
//        {
//            public static IEnumerable<object[]> Data =>
//                new List<object[]>
//                {
//                    //Correct logins
//                    new object[] { new LoginCommand { Username = "Frederik", Password = "LydiaErSød" }, HttpStatusCode.OK },
//                    new object[] { new LoginCommand { Username = "frederik", Password = "LydiaErSød" }, HttpStatusCode.OK },
//                    //Wrong username and password
//                    new object[] { new LoginCommand { Username = "Frederik", Password = "LydiaErDum" }, HttpStatusCode.Unauthorized },
//                    new object[] { new LoginCommand { Username = "Frederikke", Password = "LydiaErSød" }, HttpStatusCode.Unauthorized },
//                    new object[] { new LoginCommand { Username = "Frederikke", Password = "LydiaErDum" }, HttpStatusCode.Unauthorized },
//                    //Cross credentials
//                    new object[] { new LoginCommand { Username = "Frederik", Password = "FrederikErSød"}, HttpStatusCode.Unauthorized },
//                    //Bad request tests
//                    new object[] { new LoginCommand { Username = "Frederik", Password = null}, HttpStatusCode.BadRequest },
//                    new object[] { new LoginCommand { Username = null, Password = "LydiaErSød" }, HttpStatusCode.BadRequest },
//                    new object[] { new LoginCommand { Username = null, Password = null}, HttpStatusCode.BadRequest },
//                };
//        }

//        [Theory]
//        [MemberData(nameof(LoginTestParameterData.Data), MemberType = typeof(LoginTestParameterData))]
//        public async void LoginWithCredentials(LoginCommand loginCommand, HttpStatusCode loginExpected)
//        {
//            //Act
//            HttpResponseMessage loginResponse = await httpClient.PostAsJsonAsync(apiLogin, loginCommand);

//            string? jwtToken = null;
//            try
//            {
//                jwtToken = extractJwtToken(loginResponse);
//                httpClient.DefaultRequestHeaders.Add("Cookie", $"jwt={jwtToken}");

//            }
//            catch (Exception e)
//            {
//            }

//            HttpResponseMessage isLoggedinResponse = await httpClient.GetAsync("/api/authentication/is-logged-in");

//            //Assert
//            switch (loginExpected)
//            {
//                case HttpStatusCode.OK:
//                    Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
//                    Assert.NotNull(jwtToken);
//                    Assert.Equal(true, bool.Parse(await isLoggedinResponse.Content.ReadAsStringAsync()));
//                    break;
//                case HttpStatusCode.Unauthorized:
//                    Assert.Equal(HttpStatusCode.Unauthorized, loginResponse.StatusCode);
//                    Assert.Null(jwtToken);
//                    Assert.Equal(HttpStatusCode.OK, isLoggedinResponse.StatusCode);
//                    break;
//                case HttpStatusCode.BadRequest:
//                    Assert.Equal(HttpStatusCode.BadRequest, loginResponse.StatusCode);
//                    Assert.Null(jwtToken);
//                    break;
//                default:
//                    Assert.Fail("No assertion match was found");
//                    break;
//            }
//        }

//        private string extractJwtToken(HttpResponseMessage response)
//        {
//            if (!response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string>? cookieValues))
//            {
//                throw new Exception("Set-Cookie header not found in the response.");
//            }

//            string cookieString = cookieValues.FirstOrDefault();

//            string pattern = @"jwt=([^;]+)";
//            Match match = Regex.Match(cookieString, pattern);

//            if (!match.Success)
//            {
//                throw new Exception("JWT token not found in the Set-Cookie header.");
//            }

//            return match.Groups[1].Value;
//        }

//    }
//}