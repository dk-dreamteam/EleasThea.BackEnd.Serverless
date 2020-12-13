using EleasThea.BackEnd.Contracts.InputModels;
using EleasThea.BackEnd.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Services.Functions.Gateway
{
    public static class SendFeedbackFunction
    {
        [FunctionName("SendFeedbackFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Feedback")] HttpRequest req,
            [Queue("feedback-msgs")] ICollector<string> feedbackMsgsQueue,
            ILogger logger)
        {
            try
            {
                // read body, bind to model.
                var feedbackMessage = await req.GetBodyAsObjectAsync<FeedbackMessage>();

                // validate model. if there are errors, return bad request.
                if (!feedbackMessage.IsModelValid(out var feedValidationResults))
                    return new BadRequestObjectResult(feedValidationResults);

                // map input message to queue item dto.
                var feedbackQueueItemDto = feedbackMessage.MapToQueueItem();

                // enqueue item.
                feedbackMsgsQueue.Add(JsonConvert.SerializeObject(feedbackQueueItemDto));

                return new AcceptedResult();
            }
            catch (Exception exc)
            {
                // log error.
                logger.LogError($"Message: {exc.Message}, InnerExc: {exc.InnerException.Message}");

                // return internal server error.
                return new ContentResult { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
