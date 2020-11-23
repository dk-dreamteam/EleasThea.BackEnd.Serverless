using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Services.Utitlities
{
    public interface IEmailUtility
    {
        /// <summary>
        /// Send an email to a recipient using SMTP.
        /// </summary>
        /// <param name="recipient">Email address of the recipient.</param>
        /// <param name="subject">Subject of the email message.</param>
        /// <param name="body">Html Body of the email message.</param>
        /// <returns></returns>
        Task SendEmailAsync(string recipient, string subject, string body);
    }
}