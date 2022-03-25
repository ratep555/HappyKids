using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string toEmail, string subject, string content);
        Task SendEmailForGeneralCardSlipOrBirthdayOrderAcceptance(string toEmail, string subject, string content, int orderNo);
        Task ConfirmEmailAsync(string email, string token);
    }
}