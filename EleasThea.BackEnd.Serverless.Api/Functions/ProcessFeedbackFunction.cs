using EleasThea.BackEnd.Contracts.InputModels;
using EleasThea.BackEnd.Contracts.TableStorageModels;
using EleasThea.BackEnd.Extentions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace EleasThea.BackEnd.Serverless.Api.Functions
{
    public static class ProcessFeedbackFunction
    {
        [return: Table("Feedbacks")]
        [FunctionName("ProcessInputMessageFunction")]
        public static Feedback Run([QueueTrigger("feedback-msgs")] FeedbackMessage inputFeedback,
                               ILogger logger)
        {
            // todo : use these in process reservation.
            //// create json serializer settings to include class type.
            //var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            //var inputMessageItem = JsonConvert.DeserializeObject(inputFeedback, jsonSerializerSettings);

            // use custom extention method to map input model to table entity derived class model.
            var feedback = inputFeedback.MapToTableEntity();

            // generate partition and row keys.
            feedback.GeneratePartitionAndRowKeys(feedback.Email);

            return feedback;
        }
    }
}
