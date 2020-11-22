using EleasThea.BackEnd.Contracts.InputModels;
using EleasThea.BackEnd.Contracts.QueueModels;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace EleasThea.BackEnd.Serverless.Services.Functions
{
    public static class ProcessFeedbackFunction
    {
        [return: Table("Feedbacks")]
        [FunctionName("ProcessFeedbackFunction")]
        public static void Run([QueueTrigger("feedback-msgs")] FeedbackMessage inputFeedback,
                                   [Table("Feedbacks")] CloudTable feedbacksTable,
                                   [Queue("send-emails")] ICollector<SendEmailQueueItem> sendEmailsQueueCollector,
                                   ILogger logger)
        {
            // todo : use these in process reservation.
            //// create json serializer settings to include class type.
            //var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            //var inputMessageItem = JsonConvert.DeserializeObject(inputFeedback, jsonSerializerSettings);

            // use custom extention method to map input model to table entity derived class model.
            //var feedback = inputFeedback.MapToTableEntity();

            //// generate partition and row keys.
            //feedback.GeneratePartitionAndRowKeys(feedback.Email);

            // steps...
            // save to table storage.

            // create email body.

            // enqueue to send emails queue.


        }
    }
}
