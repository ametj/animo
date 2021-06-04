using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Animo.Web.Core.Auth
{
    public interface IJwtTokenFactory
    {
        JwtSecurityToken CreateNewToken(IEnumerable<Claim> claims);
    }

    public record JwtTokenConfiguration : IJwtTokenFactory
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public SigningCredentials SigningCredentials { get; set; }

        public TimeSpan ExpiresIn { get; set; }

        public JwtSecurityToken CreateNewToken(IEnumerable<Claim> claims)
        {
            return new JwtSecurityToken
            (
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: ExpiresIn == TimeSpan.Zero ? null : DateTime.UtcNow.Add(ExpiresIn),
                notBefore: DateTime.UtcNow,
                signingCredentials: SigningCredentials
            );
        }
    }
}