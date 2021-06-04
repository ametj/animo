using Microsoft.Extensions.Configuration;
using System.Net;

namespace Animo.Web.Core.Auth
{
    public class SmtpConfiguration
    {
        public SmtpConfiguration(IConfiguration configuration)
        {
            Host = configuration["Email:Smtp:Host"];
            Port = int.Parse(configuration["Email:Smtp:Port"]);
            Credentials = new NetworkCredential(configuration["Email:Smtp:Username"], configuration["Email:Smtp:Password"]);
            EnableSsl = bool.Parse(configuration["Email:Smtp:EnableSsl"]);
        }

        public string Host { get; init; }
        public int Port { get; init; }
        public bool EnableSsl { get; init; }
        public NetworkCredential Credentials { get; init; }
    }
}