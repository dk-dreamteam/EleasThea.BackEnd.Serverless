using EleasThea.BackEnd.Contracts.QueueModels;
using EleasThea.BackEnd.Extentions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Services.Functions
{
    public static class ProcessFeedbackFunction
    {
        [FunctionName("ProcessFeedbackFunction")]
        public static async Task RunAsync([QueueTrigger("feedback-msgs")] FeedbackQueueItem feedbackQueueItem,
                                          [Table("Feedbacks")] CloudTable feedbacksTable,
                                          [Queue("send-emails")] ICollector<SendEmailQueueItem> sendEmailsQueue,
                                          [Blob("templates")] CloudBlobContainer container,
                                          ILogger logger)
        {
            #region save feedback to table.
            var feedback = feedbackQueueItem.MapToTableEntity();

            feedback.GeneratePartitionAndRowKeys(feedback.Email, null);

            var insertIntoTableOperation = TableOperation.Insert(feedback);
            await feedbacksTable.ExecuteAsync(insertIntoTableOperation);
            #endregion


            #region prepare email body for restaurant.
            using var restaurantEmailHtmlStream = new MemoryStream();

            await container.GetBlockBlobReference("restaurant/NewFeedback.html")
                           .DownloadToStreamAsync(restaurantEmailHtmlStream);

            var restaurantEmailHtmlStr = Encoding.UTF8.GetString(restaurantEmailHtmlStream.ToArray());
            // todo: implement template value replacing here.
            #endregion


            #region send email to restaurant
            sendEmailsQueue.Add(new SendEmailQueueItem()
            {
                ReferenceToFeedbackRowKey = feedback.RowKey,
                HtmlContent = restaurantEmailHtmlStr,
                ReciepientAddress = "dim.grev@outlook.com", // todo: fix restaurant address.
                Subject = "Νέο Feedback!",
            });
            #endregion
        }
    }
}
