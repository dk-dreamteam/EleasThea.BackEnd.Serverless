using EleasThea.BackEnd.Contracts.QueueDTOs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EleasThea.BackEnd.Serverless.Api.Functions
{
    public static class ProcessInputMessageFunction
    {
        [FunctionName("ProcessInputMessageFunction")]
        public static void Run([QueueTrigger("input-messages")] string inputMessageItemStr, ILogger logger)
        {
            InputMessageDTO inputMessageItem = JsonConvert.DeserializeObject<InputMessageDTO>(inputMessageItemStr);
            var inputMessageItemStr2 = JsonConvert.SerializeObject(inputMessageItem);
            var inputMessageItem2 = JsonConvert.DeserializeObject<FeedbackDTO>(inputMessageItemStr2);

            var feedbackDTO = (FeedbackDTO)inputMessageItem;

            logger.LogInformation($"C# Queue trigger function processed: {inputMessageItemStr}");
        }
    }
}
