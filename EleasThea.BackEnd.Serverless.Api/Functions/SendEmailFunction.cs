using EleasThea.BackEnd.Contracts.QueueModels;
using EleasThea.BackEnd.Serverless.Api.Utitlities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Api.Functions
{
    public class SendEmailFunction
    {
        private readonly EmailUtility _emailUtil;

        public SendEmailFunction(EmailUtility emailUtil)
        {
            _emailUtil = emailUtil;
        }

        [return: Table("Transmissions")]
        [FunctionName("SendEmailFunction")]
        public async Task RunAsync([QueueTrigger("send-email")] SendEmailQueueItem sendEmailQueueItem,
                                   ILogger logger)
        {
            bool transmitionWasSuccessful;
            do
            {
                try
                {
                    // todo: figure a way to pass an array or a string as an email address.
                    await _emailUtil.SendEmailAsync(new string[] { sendEmailQueueItem.Entity.ToString() }, sendEmailQueueItem.Subject, sendEmailQueueItem.HtmlContent);
                    transmitionWasSuccessful = true;
                }
                catch (Exception exc)
                {
                    transmitionWasSuccessful = false;
                }
            } while (true);
        }
    }
}
