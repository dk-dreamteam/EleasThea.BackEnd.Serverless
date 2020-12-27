using EleasThea.BackEnd.Contracts.InputModels;
using EleasThea.BackEnd.Contracts.QueueModels;
using EleasThea.BackEnd.Contracts.TableStorageModels;
using EleasThea.BackEnd.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Services.Functions
{
    public class SendFeedbackFunction
    {
        [FunctionName(nameof(SendFeedbackFunction))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "feedbacks")] HttpRequest req,
            [Table("%FeedbacksTableName%")] CloudTable feedbacksTable,
            [Queue("%SendEmailQueueName%")] ICollector<SendEmailQueueItem> sendEmailsQueue,
            [Blob("templates")] CloudBlobContainer container,
            ILogger logger)
        {
            try
            {
                logger.LogInformation($"Function:{nameof(SendFeedbackFunction)} started executing.");

                // validate input feedback message.
                var feedbackMessage = await req.GetBodyAsObjectAsync<FeedbackMessage>();

                logger.LogInformation($"FeedbackSenderEmail:{feedbackMessage.Email}");

                if (!feedbackMessage.IsModelValid(out var feedValidationResults))
                    return new BadRequestObjectResult(feedValidationResults);

                // map feedback message to feedback obj and generate partition and rowkeys.
                var feedback = feedbackMessage.MapToTableEntity()
                                              .GeneratePartitionAndRowKeys<Feedback>(feedbackMessage.Email, null);

                // insert feedback into feedback table storage table.
                var insertIntoTableOperation = TableOperation.Insert(feedback);
                var insertionResult = await feedbacksTable.ExecuteAsync(insertIntoTableOperation);

                logger.LogInformation($"Feedback Insertion result:{insertionResult.Result}");

                // retrieve feedback email template and replace variables with real data.
                using var restaurantEmailHtmlStream = new MemoryStream();
                await container.GetBlockBlobReference("restaurant/NewFeedback.html")
                               .DownloadToStreamAsync(restaurantEmailHtmlStream);
                var restaurantEmailHtmlAsString = Encoding.UTF8.GetString(restaurantEmailHtmlStream.ToArray());
                // todo: implement template value replacing here.

                // enqueue mail to send to notify restaurant for new feedback.
                sendEmailsQueue.Add(new SendEmailQueueItem()
                {
                    ReferenceToFeedbackRowKey = feedback.RowKey,
                    HtmlContent = restaurantEmailHtmlAsString,
                    ReciepientAddress = "kon.kri@outlook.com", // todo: fix restaurant address.
                    Subject = "Νέο Feedback!",
                });

                logger.LogInformation($"Email object enqueued. Returning accepted result. stopping function...");
                return new AcceptedResult();
            }
            catch (Exception exc)
            {
                logger.LogError($"Message: {exc.Message}, InnerExc: {exc.InnerException.Message}");
                return new ContentResult { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
