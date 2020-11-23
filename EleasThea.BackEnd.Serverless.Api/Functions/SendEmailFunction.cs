using EleasThea.BackEnd.Contracts.Enums;
using EleasThea.BackEnd.Contracts.QueueModels;
using EleasThea.BackEnd.Contracts.TableStorageModels;
using EleasThea.BackEnd.Extentions;
using EleasThea.BackEnd.Serverless.Services.Utitlities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace EleasThea.BackEnd.Serverless.Services.Functions
{
    public class SendEmailFunction
    {
        private readonly IEmailUtility _emailUtil;

        public SendEmailFunction(IEmailUtility emailUtil)
        {
            _emailUtil = emailUtil;
        }

        [return: Table("Transmissions")]
        [FunctionName("SendEmailFunction")]
        public async Task<Transmission> RunAsync([QueueTrigger("send-email")] SendEmailQueueItem sendEmailQueueItem,
                                   ILogger logger)
        {
            // create transmission object and assign partition and row keys.
            var transmission = new Transmission();
            transmission.GeneratePartitionAndRowKeys();

            // add img element to html content for registering open events in this webhook uri.
            var body = this.AddWebhookUrl(sendEmailQueueItem.HtmlContent, $"asdf.net?tId={transmission.RowKey}");

            var tries = 0;
            bool transmissionWasSuccessful = false;
            // make 3 attempts to send an email.
            do
            {
                try
                {
                    await _emailUtil.SendEmailAsync(sendEmailQueueItem.ReciepientAddress, sendEmailQueueItem.Subject, body);
                    transmissionWasSuccessful = true;
                }
                catch (Exception exc)
                {
                    transmissionWasSuccessful = false;
                }
                finally
                {
                    tries++;
                }
            } while (tries <= 3 && !transmissionWasSuccessful);

            // update transmission's status upon transmission attempts.
            transmission.Status = transmissionWasSuccessful ? TransmissionStatus.Sent : TransmissionStatus.Unsuccessful;

            // return and add to transmissions table.
            return transmission;
        }

        private string AddWebhookUrl(string xml, string webhookUri)
        {
            // create new xml doc and load xml from string.
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            // create xml navigator and select htmlBody node.
            var navigator = doc.CreateNavigator();
            var htmlBodyNode = navigator.SelectSingleNode("html/body");

            // append img element for email opened event webhook.
            htmlBodyNode.AppendChild($"<img width=\"1px\" height=\"1px\" src=\"{webhookUri}\"/>");

            // return xml as string.
            return doc.ToString();
        }
    }
}
