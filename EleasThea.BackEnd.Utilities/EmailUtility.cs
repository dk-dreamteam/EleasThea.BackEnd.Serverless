using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Services.Utitlities
{
    public class EmailUtility : IEmailUtility
    {
        private readonly SmtpClient _client;
        private readonly string FromEmailAddress;

        public EmailUtility(SmtpClient client, string fromEmailAddress)
        {
            _client = client;
            FromEmailAddress = fromEmailAddress;
        }

        public async Task SendEmailAsync(string recipient, string subject, string body)
        {
            try
            {
                var msg = new MailMessage();
                msg.To.Add(new MailAddress(recipient));
                msg.From = new MailAddress(FromEmailAddress);
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = true;
                await _client.SendMailAsync(msg);
                return;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
