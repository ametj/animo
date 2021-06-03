using Animo.Web.Core.Dto;
using Animo.Web.Core.Extensions;
using Animo.Web.Core.Models.Users;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace Animo.Web.Tests.Integration
{
    [Collection("CustomWebApplicationFactory")]
    public class TestBase
    {
        protected CustomWebApplicationFactory _factory;
        protected HttpClient _client;

        public TestBase(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        protected void SetAuthorizationHeader(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        protected async Task<HttpResponseMessage> GetLoginResponse(string userName, string password)
        {
            var login = new Login(userName, password);

            return await _client.PostAsync("/api/Account/Login", login.ToStringConent());
        }

        protected async Task Login(string userName, string password)
        {
            var response = await GetLoginResponse(userName, password);
            var content = await response.Content.ReadAsAsync<LoginToken>();
            SetAuthorizationHeader(content.Token);
        }

        protected async Task LoginAsAdmin()
        {
            var admin = DefaultUsers.Admin;
            await Login(admin.UserName, admin.UserName);
        }

        protected async Task LoginAsMember()
        {
            var member = DefaultUsers.Member;
            await Login(member.UserName, member.UserName);
        }
    }
}