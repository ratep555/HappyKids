using System.IO;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private UserManager<ApplicationUser> _userManager;
        public EmailService(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task SendEmail(string toEmail, string subject, string content)
        {
            var apiKey = _config["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("petar.sardelic@outlook.com", "HappyKids Demo");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }

        public async Task SendEmailForGeneralCardSlipOrBirthdayOrderAcceptance(string toEmail, string subject, string content, int orderNo)
        {
            var apiKey = _config["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("petar.sardelic@outlook.com", "Happykids Demo");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);

            using (var fileStream = File.OpenRead("C:\\Users\\petar\\source\\repos\\" + orderNo.ToString() + ".pdf"))
            {
                await msg.AddAttachmentAsync("TestPDF2.pdf", fileStream);
                var response = await client.SendEmailAsync(msg);
            }
        }
    }
}













