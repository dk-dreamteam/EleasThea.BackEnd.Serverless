using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Services.Utitlities
{
    public interface IEmailUtility
    {
        Task SendEmailAsync(string recipient, string subject, string body);
    }
}