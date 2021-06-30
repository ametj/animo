namespace Animo.Web.Core.Dto
{
    public record Register(string UserName, string Email, string Password);

    public record Login(string UserNameOrEmail, string Password);

    public record LoginToken(string Token);

    public record ChangePassword(string CurrentPassword, string NewPassword);

    public record ForgotPassword(string UserNameOrEmail);

    public record ResetPassword(string UserNameOrEmail, string Password, string Token);

}