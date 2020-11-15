using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Api.Utitlities
{
    public class EmailUtility
    {
        private readonly SmtpClient _client;

        public EmailUtility(SmtpClient client)
        {
            _client = client;
        }

        public async Task SendEmailAsync(string[] recipients, string subject, string emailBody)
        {
            try
            {
                
                var emailAddressesToSendTo = string.Join(",", recipients);//Create a string of all recievers...

                var msg = new MailMessage();
                msg.To.Add(emailAddressesToSendTo);
                msg.From = new MailAddress("eleasthea.noreply@yahoo.com");
                msg.Subject = subject;
                msg.Body = emailBody;
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
