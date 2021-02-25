using EleasThea.BackEnd.Contracts.Enums;
using EleasThea.BackEnd.Contracts.QueueModels;
using EleasThea.BackEnd.Contracts.TableStorageModels;
using EleasThea.BackEnd.Extentions;
using EleasThea.BackEnd.Serverless.Services.Utitlities;
using HtmlAgilityPack;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Services.Functions
{
    public class SendEmailFunction
    {
        private readonly IEmailUtility _emailUtil;

        public SendEmailFunction(IEmailUtility emailUtil)
        {
            _emailUtil = emailUtil;
        }

        [return: Table("%TransmissionsTableName%")]
        [FunctionName(nameof(SendEmailFunction))]
        public async Task<DynamicTableEntity> RunAsync([QueueTrigger("%SendEmailQueueName%")] SendEmailQueueItem sendEmailQueueItem,
                                                 ILogger logger)
        {
            logger.LogInformation($"Function:{nameof(SendEmailFunction)} started executing.");

            var transmission = new Transmission()
            {
                TransmittedOnUtc = DateTime.UtcNow,
                ReferenceToFeedbackRowKey = string.IsNullOrWhiteSpace(sendEmailQueueItem.ReferenceToFeedbackRowKey) ? null : sendEmailQueueItem.ReferenceToFeedbackRowKey,
                ReferenceToReservationRowKey = string.IsNullOrWhiteSpace(sendEmailQueueItem.ReferenceToReservationRowKey) ? null : sendEmailQueueItem.ReferenceToReservationRowKey,
                HtmlContent = sendEmailQueueItem.HtmlContent
            };

            var operationContext = new OperationContext();
            var dynamicTableEntity = new DynamicTableEntity(transmission.PartitionKey, transmission.RowKey)
            {
                Properties = TableEntity.Flatten(transmission, operationContext)
            };

            transmission.GeneratePartitionAndRowKeys(sendEmailQueueItem.ReciepientAddress, null);

            // add img element to html content for registering open events in this webhook uri.
            var body = AddWebhookUrl(sendEmailQueueItem.HtmlContent, $"https://webhook.free.beeceptor.com?tId={transmission.RowKey}");

            var tries = 1;
            bool transmissionWasSuccessful = false;
            // make 3 attempts to send an email.
            do
            {
                try
                {
                    await _emailUtil.SendEmailAsync(sendEmailQueueItem.ReciepientAddress, sendEmailQueueItem.Subject, body);
                    transmissionWasSuccessful = true;
                    logger.LogInformation($"Transmission with partition key:{transmission.PartitionKey} and row key:{transmission.RowKey} successfully sent.");
                }
                catch (Exception exc)
                {
                    transmissionWasSuccessful = false;
                    logger.LogError($"Transmission with partition key:{transmission.PartitionKey} and row key:{transmission.RowKey} was not successfully sent. exception message: {exc.Message}, inner exception message: {exc.InnerException?.Message}");
                }
                finally
                {
                    tries++;
                }
            } while (tries < 3 && !transmissionWasSuccessful);

            // update transmission's status upon transmission attempts.
            transmission.Status = transmissionWasSuccessful ? TransmissionStatus.Sent : TransmissionStatus.Unsuccessful;

            // return and add to transmissions table.
            return dynamicTableEntity;
        }

        /// <summary>
        /// Append a non visible img element at the bottom of the <body> element.
        /// </summary>
        /// <param name="html">HTML code to append the img tag.</param>
        /// <param name="webhookUri">The URI to hit when the email is read.</param>
        /// <returns></returns>
        private string AddWebhookUrl(string html, string webhookUri)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var imgWebHookNode = HtmlNode.CreateNode($"<img style=\"visibility:hidden;\" src=\"{webhookUri}\"/>");
            var bodyHtmlNode = htmlDoc.DocumentNode.AppendChild(imgWebHookNode);

            return htmlDoc.DocumentNode.OuterHtml;
        }
    }
}
