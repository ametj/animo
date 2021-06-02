using Microsoft.IdentityModel.Tokens;
using System;

namespace Animo.Web.Core.Auth
{
    public record JwtTokenConfiguration
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public SigningCredentials SigningCredentials { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? StartDate { get; set; }
    }
}