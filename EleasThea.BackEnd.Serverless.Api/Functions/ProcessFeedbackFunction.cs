using EleasThea.BackEnd.Contracts.InputModels;
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
        public static async Task RunAsync([QueueTrigger("feedback-msgs")] FeedbackMessage inputFeedback,
                                   [Table("Feedbacks")] CloudTable feedbacksTable,
                                   [Queue("send-emails")] ICollector<SendEmailQueueItem> sendEmailsQueueCollector,
                                   [Blob("resources")] CloudBlobContainer container,
                                   ILogger logger)
        {
            #region save feedback to table.
            // use custom extention method to map input model to table entity derived class model.
            var feedback = inputFeedback.MapToTableEntity();
            // generate partition and row keys.
            feedback.GeneratePartitionAndRowKeys(feedback.Email);

            var insertIntoTableOperation = TableOperation.Insert(feedback);
            await feedbacksTable.ExecuteAsync(insertIntoTableOperation);
            #endregion


            #region prepare email body.
            // get template from blob storage.
            var customerEmailHtmlReference = container.GetBlockBlobReference("templates/ThankYouForYourFeedback.html");
            var customerEmailHtmlStream = new MemoryStream();
            await customerEmailHtmlReference.DownloadToStreamAsync(customerEmailHtmlStream);
            var customerEmailHtmlStr = Encoding.UTF8.GetString(customerEmailHtmlStream.ToArray());

            // todo: implement template value replacing here.
            #endregion


            #region send email to customer.
            // enqueue to send emails queue.
            sendEmailsQueueCollector.Add(new SendEmailQueueItem()
            {
                HtmlContent = customerEmailHtmlStr,
                ReciepientAddress = feedback.Email,
                Subject = "Ευχαριστούμε που μας είπατε τη γνώμη σας"
            });
            #endregion
        }
    }
}
