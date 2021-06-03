using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Animo.Web.Tests.Integration
{
    public class AuthorizationTests : AuthorizationTestBase
    {
        public AuthorizationTests(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Authorize_AuthorizedAndHasPermission_ShouldAllowAccess()
        {
            await LoginAsAdmin();

            var response = await GetAsync("/api/users");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Authorize_AuthorizedAndHasNoPermission_ShouldAllowAccess()
        {
            await LoginAsMember();

            var response = await GetAsync("/api/users");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Authorize_Unauthorized_ShouldNotAllowAccess()
        {
            var response = await GetAsync("/api/users");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}