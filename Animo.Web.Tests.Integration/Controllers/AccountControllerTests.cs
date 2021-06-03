using Animo.Web.Core.Dto;
using Animo.Web.Core.Extensions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace Animo.Web.Tests.Integration.Controllers
{
    public class AccountControllerTests : TestBase
    {
        public AccountControllerTests(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Login_ExistingUser_ShouldLogin()
        {
            var response = await LoginAsAdminUserAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_NonExistingUser_ShouldNotLogin()
        {
            var response = await Login("123_aBc", "123_aBc");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_ExistingUser_ShouldGetToken()
        {
            var response = await LoginAsMemberUserAsync();
            var result = await response.Content.ReadAsAsync<LoginToken>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result.Token);
        }

        [Fact]
        public async Task Login_Token_ShouldAllowAccess()
        {
            var loginResponse = await LoginAsAdminUserAsync();
            var loginResult = await loginResponse.Content.ReadAsAsync<LoginToken>();
            var token = loginResult.Token;

            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
            Assert.NotNull(loginResult.Token);

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/users");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Register_NewUser_ShouldRegisterAndBeAbleToLogin()
        {
            var userName = "registerNewName";
            var email = "registerNewEmail@mail.com";
            var password = "123_aBc*";

            var newUser = new Register(userName, email, password);
            var response = await _client.PostAsync("/api/Account/Register", newUser.ToStringConent());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var loginResponse = await Login(userName, password);
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        }
    }
}