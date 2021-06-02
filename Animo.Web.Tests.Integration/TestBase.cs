using Animo.Web.Core.Dto;
using Animo.Web.Core.Extensions;
using Animo.Web.Core.Models.Users;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Animo.Web.Tests.Integration
{
    public class TestBase : IClassFixture<CustomWebApplicationFactory>
    {
        protected CustomWebApplicationFactory _factory;

        public TestBase(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        protected async Task<HttpResponseMessage> Login(string userName, string password)
        {
            var client = _factory.CreateClient();

            var content = ObjectExtensions.ToStringConent(new Login(userName, password));
            return await client.PostAsync("/api/Account/Login", content);
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