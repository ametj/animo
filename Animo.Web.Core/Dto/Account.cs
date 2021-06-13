namespace Animo.Web.Core.Dto
{
    public record Register(string Name, string Email, string Password);

    public record Login(string UserNameOrEmail, string Password);

    public record LoginToken(string Token);

    public record ChangePassword(string CurrentPassword, string NewPassword);

    public record ForgotPassword(string NameOrEmail);

    public record ForgotPasswordToken(string Token);

    public record ResetPassword(string NameOrEmail, string Password, string Token);

}