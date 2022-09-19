using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Blog.BLL.Services.ExternalServices
{
    public class EmailService
    {
        private const string FromName = "Blog Website Administration";
        private const string FromAddress = "blogcampproject12@gmail.com";
        private const string FromPassword = "mabjnflgbeylyhmm";
        private const string SmtpServer = "smtp.gmail.com";
        private const int SmtpPort = 25;

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(FromName, FromAddress));
            emailMessage.To.Add(new MailboxAddress(string.Empty, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = false;
                await client.ConnectAsync(SmtpServer, SmtpPort, SecureSocketOptions.Auto);
                await client.AuthenticateAsync(FromAddress, FromPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
