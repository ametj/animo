using Animo.Web.Core.Models.Users;
using Animo.Web.Core.Services;
using MimeKit;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Animo.Web.Tests.Integration.Mock
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

    public class TestMailService : IMailService
    {
        public static ConcurrentDictionary<string, string> ResetTokens { get; set; } = new ConcurrentDictionary<string, string>();

        public async Task SendMailAsync(MimeMessage message)
        {
            throw new NotImplementedException();
        }

        public async Task SendResetPasswordMailAsync(User user, string resetToken)
        {
            ResetTokens.TryAdd(user.UserName, resetToken);
        }
    }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}