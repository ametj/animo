using Animo.Web.Core.Models.Users;
using MimeKit;
using System.Threading.Tasks;

namespace Animo.Web.Core.Services
{
    public interface IMailService
    {
        Task SendMailAsync(MimeMessage message);

        Task SendResetPasswordMailAsync(User user, string resetToken);
    }
}