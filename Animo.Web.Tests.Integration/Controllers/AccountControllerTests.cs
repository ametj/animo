using Animo.Web.Core.Dto;
using Animo.Web.Core.Extensions;
using Animo.Web.Core.Models.Users;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Animo.Web.Tests.Integration.Controllers
{
    public class AccountControllerTests : TestBase
    {
        public const string ValidPassword = "123_aBc*";

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
            var result = await response.Content.ReadAsAsync<LoginToken>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(result.Token);
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
            var email = "registerNewName@mail.com";

            var newUser = new Register(userName, email, ValidPassword);
            var response = await _client.PostAsync("/api/Account/Register", newUser.ToStringConent());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var loginResponse = await GetLoginResponse(userName, ValidPassword);
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        }

        [Theory]
        [InlineData("Admin", "newValidMail@mail.com")]
        [InlineData("NewName", "Admin@mail.com")]
        public async Task Register_ExistingUser_ShouldNotRegister(string userName, string email)
        {
            var newUser = new Register(userName, email, ValidPassword);
            var response = await _client.PostAsync("/api/Account/Register", newUser.ToStringConent());
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(null, 6)]
        [InlineData("", 6)]
        [InlineData("5H*rt", 1)]
        [InlineData("No_Number", 1)]
        [InlineData("no_upper1", 1)]
        [InlineData("NO_LOWER1", 1)]
        [InlineData("OnlyAlphanumeric1", 1)]
        public async Task Register_NotValidPassword_ShouldReturnValidationErrors(string password, int numberOfValidationsFailed)
        {
            var newUser = new Register("NewName", "NewName@mail.com", password);
            var response = await _client.PostAsync("/api/Account/Register", newUser.ToStringConent());
            var result = await response.Content.ReadAsAsync<ValidationProblemDetails>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal(numberOfValidationsFailed, result.Errors.Count);
        }
    }
}