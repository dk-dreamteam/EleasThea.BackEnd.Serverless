using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Api.Utitlities
{
    public class EmailUtility
    {
        private readonly SmtpClient _client;
        private readonly string FromEmailAddress;

        public EmailUtility(SmtpClient client, string fromEmailAddress)
        {
            _client = client;
            FromEmailAddress = fromEmailAddress;
        }

        public async Task SendEmailAsync(string[] recipients, string subject, string body)
        {
            try
            {
                var emailAddressesToSendTo = string.Join(",", recipients);//Create a string of all recievers...

                var msg = new MailMessage();
                msg.To.Add(emailAddressesToSendTo);
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
