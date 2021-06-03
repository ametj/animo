using Animo.Web.Core.Dto;
using Animo.Web.Core.Extensions;
using Animo.Web.Core.Models.Users;
using System.Net.Http;
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

        protected async Task<HttpResponseMessage> Login(string userName, string password)
        {
            var login = new Login(userName, password);

            return await _client.PostAsync("/api/Account/Login", login.ToStringConent());
        }

        protected async Task<HttpResponseMessage> LoginAsAdminUserAsync()
        {
            var admin = DefaultUsers.Admin;

            return await Login(admin.UserName, admin.UserName);
        }

        protected async Task<HttpResponseMessage> LoginAsMemberUserAsync()
        {
            var member = DefaultUsers.Member;

            return await Login(member.UserName, member.UserName);
        }
    }
}