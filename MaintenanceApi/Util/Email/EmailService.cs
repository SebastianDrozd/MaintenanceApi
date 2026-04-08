using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net.Mail;

namespace MaintenanceApi.Util.Email
{
    public class EmailService
    {
        public async Task SendEmail(string recipientAddress, string subject, string body)
        {
            Console.WriteLine($"{recipientAddress},{subject}, {body}");
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Maintenance", "MaintenanceSystem@bobak.com"));
            message.To.Add(new MailboxAddress("", $"{recipientAddress}@bobak.com"));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("mail.smtp2go.com", 2525, SecureSocketOptions.StartTls);

                // 🔑 Replace with your real SMTP2GO credentials
                await client.AuthenticateAsync("bobakmaintenance", "0BAzvnalKxB8jpcI");

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}