using Animo.Web.Core.Auth;
using Animo.Web.Core.Dto;
using Animo.Web.Core.Extensions;
using Animo.Web.Core.Models.Permissions;
using Animo.Web.Core.Models.Roles;
using Animo.Web.Core.Models.Users;
using Animo.Web.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Animo.Web.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenFactory _jwtTokenFactory;
        private readonly IMailService _mailService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<User> userManager,
            IJwtTokenFactory jwtTokenFactory,
            IMailService mailService,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _mailService = mailService;
            _logger = logger;
            _jwtTokenFactory = jwtTokenFactory;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<LoginToken>> Login([FromBody] Login login)
        {
            var user = await CreateClaimsIdentityAsync(login.UserNameOrEmail, login.Password);
            if (user == null)
            {
                _logger.LogInformation($"Login fail: {login.UserNameOrEmail}");

                ModelState.AddModelError("UserNameOrPassword", "The user name or password is incorrect!");

                return ValidationProblem();
            }

            var token = _jwtTokenFactory.CreateNewToken(user.Claims);

            _logger.LogInformation($"Login success: {login.UserNameOrEmail}");

            return Ok(new LoginToken(new JwtSecurityTokenHandler().WriteToken(token)));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Register([FromBody] Register body)
        {
            if (!body.UserName.IsNullOrEmpty() && await _userManager.FindByNameAsync(body.UserName) != null)
            {
                ModelState.AddModelError("Name", $"User with name '{body.UserName}' already exists!");
            }

            if (!body.Email.IsNullOrEmpty() && await _userManager.FindByEmailAsync(body.Email) != null)
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
                UserName = body.UserName,
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

            var currentUser = await _userManager.FindByNameAsync(body.UserName);
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
            var user = await FindUserByUserNameOrEmail(body.UserNameOrEmail);
            if (user == null)
            {
                return NotFound();
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            try
            {
                await _mailService.SendResetPasswordMailAsync(user, resetToken);
            }
            catch (Exception e)
            {
                var msg = $"Could not send reset password email to user {user}";
                _logger.LogError(e, msg);
                return Problem();
            }

            return Ok(new ForgotPasswordToken(resetToken));
        }

        [HttpPut("Password/Reset")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPassword body)
        {
            var user = await FindUserByUserNameOrEmail(body.UserNameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("NameOrEmail", $"User '{body.UserNameOrEmail}' not found!");
                return ValidationProblem();
            }

            var result = await _userManager.ResetPasswordAsync(user, body.Token, body.Password);
            if (!ProcessIdentityValidation(result))
            {
                _logger.LogInformation($"Password reset fail: {body.UserNameOrEmail}");
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
                    new Claim(JwtRegisteredClaimNames.Sub, userToVerify.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, userToVerify.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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