using Animo.Web.Core.Dto;
using FluentValidation;

namespace Animo.Web.Api.Validators
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(m => m.UserNameOrEmail).NotEmpty();
            RuleFor(m => m.Password).NotEmpty();
        }
    }

    public class RegisterValidator : AbstractValidator<Register>
    {
        public RegisterValidator()
        {
            RuleFor(m => m.UserName).NotEmpty();
            RuleFor(m => m.Email).NotEmpty().EmailAddress();
        }
    }

    public class ForgotPasswordValidator : AbstractValidator<ForgotPassword>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(m => m.UserNameOrEmail).NotEmpty();
        }
    }
}