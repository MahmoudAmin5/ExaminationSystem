using ExaminationSystem.Api.BuildingBlocks.Interfaces;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace ExaminationSystem.Api.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        EmailSettings _settings;

        public EmailService(IConfiguration configuration)
        {
            _settings = configuration.GetSection("EmailSettings").Get<EmailSettings>()!;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
           
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

          
            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();

           
            using var smtp = new SmtpClient();
            try
            {
                
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);

               
                await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.Password);

                
                await smtp.SendAsync(email);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}