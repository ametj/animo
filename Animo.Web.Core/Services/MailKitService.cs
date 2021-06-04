using Animo.Web.Core.Auth;
using Animo.Web.Core.Models.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using System;
using System.Threading.Tasks;

namespace Animo.Web.Core.Services
{
    public class MailKitService : IMailService
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly SmtpConfiguration _smtpConfiguration;
#pragma warning restore IDE0052 // Remove unread private members
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public MailKitService(IConfiguration configuration, SmtpConfiguration smtpConfiguration, ILogger<MailKitService> logger)
        {
            _configuration = configuration;
            _smtpConfiguration = smtpConfiguration;
            _logger = logger;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task SendMailAsync(MimeMessage message)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
#if !DEBUG
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(_smtpConfiguration.Host, _smtpConfiguration.Port, _smtpConfiguration.EnableSsl);
                client.Authenticate(_smtpConfiguration.Credentials);
                await client.SendAsync(message);
            }
#endif
        }

        public async Task SendResetPasswordMailAsync(User user, string resetToken)
        {
            var callbackUrl = _configuration["App:ClientUrl"] + "/account/reset-password?token=" + resetToken;

            var from = GetFrom();

            if (from == null) return;

            var message = new MimeMessage();

            message.To.Add(MailboxAddress.Parse(user.Email));
            message.From.Add(from);
            message.Subject = "Password Reset - Animo";

            // TODO: Move to right place and make it pretty
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = "<h3>Password Reset</h3>" +
                       $"<p>Hi {user.UserName},<br/>" +
                       "We got a request to reset your Animo password.</p>" +
                       $"<p><a href='{callbackUrl}'>Reset password</a></p>" +
                       "<p>If you ignore this message, your password won't be changed.</p>"
            };

            await SendMailAsync(message);
            _logger.LogInformation($"Reset password for {user.UserName} sent to {user.Email}");
        }

        private MailboxAddress GetFrom()
        {
            try
            {
                return MailboxAddress.Parse(_configuration["Email:Smtp:Username"]);
            }
            catch (Exception)
            {
                _logger.LogWarning("Email address is not configured properly");
                return null;
            }
        }
    }
}