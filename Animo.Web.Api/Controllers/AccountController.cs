using Animo.Web.Core.Auth;
using Animo.Web.Core.Dto;
using Animo.Web.Core.Models.Permissions;
using Animo.Web.Core.Models.Roles;
using Animo.Web.Core.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Animo.Web.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenConfiguration _jwtTokenConfiguration;
        private readonly IConfiguration _configuration;
        private readonly SmtpClient _smtpClient;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<User> userManager,
            IOptions<JwtTokenConfiguration> jwtTokenConfiguration,
            IConfiguration configuration,
            SmtpClient smtpClient,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _smtpClient = smtpClient;
            _logger = logger;
            _jwtTokenConfiguration = jwtTokenConfiguration.Value;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<LoginToken>> Login([FromBody] Login login)
        {
            var user = await CreateClaimsIdentityAsync(login.NameOrEmail, login.Password);
            if (user == null)
            {
                _logger.LogInformation($"Login fail: {login.NameOrEmail}");

                ModelState.AddModelError("UserNameOrPassword", "The user name or password is incorrect!");

                return ValidationProblem();
            }

            var token = new JwtSecurityToken
            (
                issuer: _jwtTokenConfiguration.Issuer,
                audience: _jwtTokenConfiguration.Audience,
                claims: user.Claims,
                expires: _jwtTokenConfiguration.EndDate,
                notBefore: _jwtTokenConfiguration.StartDate,
                signingCredentials: _jwtTokenConfiguration.SigningCredentials
            );

            _logger.LogInformation($"Login success: {login.NameOrEmail}");

            return Ok(new LoginToken(new JwtSecurityTokenHandler().WriteToken(token)));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Register([FromBody] Register body)
        {
            var byName = await _userManager.FindByNameAsync(body.Name);
            var byEmail = await _userManager.FindByEmailAsync(body.Email);

            if (byName != null)
            {
                ModelState.AddModelError("Name", $"User with name '{body.Name}' already exists!");
            }

            if (byEmail != null)
            {
                ModelState.AddModelError("Email", $"User with email {body.Email} already exists!");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"Registration fail.");
                return ValidationProblem();
            }

            var applicationUser = new User
            {
                UserName = body.Name,
                Email = body.Email,
                EmailConfirmed = false
            };

            // To get password requirements when password is null
            var result = await _userManager.CreateAsync(applicationUser, body.Password ?? string.Empty);

            if (!ProcessIdentityValidation(result))
            {
                _logger.LogInformation($"Registration fail.");
                return ValidationProblem();
            }

            var currentUser = await _userManager.FindByNameAsync(body.Name);
            await _userManager.AddToRoleAsync(currentUser, DefaultRoles.Member.Name);

            _logger.LogInformation($"Registration success: {applicationUser}");
            return Ok();
        }

        [HttpPut("Password")]
        [Authorize(Policy = DefaultPermissions.PermissionNameForMemberAccess)]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePassword password)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var result = await _userManager.ChangePasswordAsync(user, password.CurrentPassword, password.NewPassword);

            if (!ProcessIdentityValidation(result))
            {
                _logger.LogInformation($"Password change fail: {user}");
                return ValidationProblem();
            }
            _logger.LogInformation($"Password changed: {user}");
            return Ok();
        }

        [HttpPost("Password/Reset")]
        public async Task<ActionResult<ForgotPasswordToken>> ForgotPassword([FromBody] ForgotPassword body)
        {
            var user = await FindUserByUserNameOrEmail(body.NameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("UserNameOrEmail", $"User {body.NameOrEmail} not found!");
                return ValidationProblem();
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = _configuration["App:ClientUrl"] + "/account/reset-password?token=" + resetToken;

            // TODO: Move to right place and make it pretty 
            var message = new MailMessage(
                from: _configuration["Email:Smtp:Username"],
                to: user.Email,
                subject: "Password Reset - Animo",
                body: "<h3>Password Reset</h3>" +
                      $"<p>Hi {user.UserName},<br/>" +
                      "We got a request to reset your Animo password.</p>" +
                      $"<p><a href='{callbackUrl}'>Reset password</a></p>" +
                      "<p>If you ignore this message, your password won't be changed.</p>");
            message.IsBodyHtml = true;

#if !DEBUG
            try
            {
                await _smtpClient.SendMailAsync(message);
            }
            catch (Exception e)
            {
                var msg = $"Could not send reset password email to user {user}";
                _logger.LogError(e, msg);
                return Problem(msg);
            }
#endif
            _logger.LogInformation($"Reset password for {user.UserName} sent to {user.Email}: {callbackUrl}");
            return Ok(new ForgotPasswordToken(resetToken));
        }

        [HttpPut("Password/Reset")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPassword body)
        {
            var user = await FindUserByUserNameOrEmail(body.NameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("NameOrEmail", $"User '{body.NameOrEmail}' not found!");
                return ValidationProblem();
            }

            var result = await _userManager.ResetPasswordAsync(user, body.Token, body.Password);
            if (!ProcessIdentityValidation(result))
            {
                _logger.LogInformation($"Password reset fail: {body.NameOrEmail}");
                return ValidationProblem();
            }

            return Ok();
        }

        private async Task<ClaimsIdentity> CreateClaimsIdentityAsync(string userNameOrEmail, string password)
        {
            if (string.IsNullOrEmpty(userNameOrEmail) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var userToVerify = await FindUserByUserNameOrEmail(userNameOrEmail);

            if (userToVerify == null)
            {
                return null;
            }

            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return new ClaimsIdentity(new GenericIdentity(userNameOrEmail, "Token"), new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userNameOrEmail),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, userToVerify.Id.ToString())
                });
            }

            return null;
        }

        private async Task<User> FindUserByUserNameOrEmail(string userNameOrEmail)
        {
            return await _userManager.FindByNameAsync(userNameOrEmail) ??
                   await _userManager.FindByEmailAsync(userNameOrEmail);
        }
    }
}