using Common;
using System.Threading.Tasks;

namespace Services.Email
{
    public interface IEmailSender
    {
        Task SendEmail(EmailSetting email, EmailMessage message);
        Task SendNewSubscribe(string email);
        Task LogSystem(string logDescription);
        Task SendEmailtoAdmin(string body);
    }
}
