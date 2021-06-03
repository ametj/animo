using Animo.Web.Core.Dto;
using Animo.Web.Core.Extensions;
using Animo.Web.Core.Models.Users;
using System.Net;
using System.Net.Http;
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
            var admin = DefaultUsers.Admin;
            var response = await GetLoginResponse(admin.UserName, admin.UserName);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_NonExistingUser_ShouldNotLogin()
        {
            var response = await GetLoginResponse("123_aBc", "123_aBc");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_ExistingUser_ShouldGetToken()
        {
            var member = DefaultUsers.Member;
            var response = await GetLoginResponse(member.UserName, member.UserName);
            var result = await response.Content.ReadAsAsync<LoginToken>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result.Token);
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

            var loginResponse = await GetLoginResponse(userName, password);
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        }
    }
}