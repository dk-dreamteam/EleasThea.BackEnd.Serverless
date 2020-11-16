using EleasThea.BackEnd.Serverless.Api.Utitlities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace EleasThea.BackEnd.Serverless.Api.Functions
{
    public class SendEmailFunction
    {
        private readonly EmailUtility _emailUtil;

        public SendEmailFunction(EmailUtility emailUtil)
        {
            _emailUtil = emailUtil;
        }

        [FunctionName("SendEmailFunction")]
        public void Run([QueueTrigger("send-email")] string myQueueItem, ILogger logger)
        {
            JsonConvert.Get

            _emailUtil.SendEmailAsync()
        }
    }
}
