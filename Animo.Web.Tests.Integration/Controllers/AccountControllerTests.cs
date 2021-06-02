using System.Net;
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
            var responseLogin = await LoginAsAdminUserAsync();
            Assert.Equal(HttpStatusCode.OK, responseLogin.StatusCode);
        }

        [Fact]
        public async Task Login_NonExistingUser_ShouldNotLogin()
        {
            var responseLogin = await Login("123_aBc", "123_aBc");
            Assert.Equal(HttpStatusCode.BadRequest, responseLogin.StatusCode);
        }
    }
}