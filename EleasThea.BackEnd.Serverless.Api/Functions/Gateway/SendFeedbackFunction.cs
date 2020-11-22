using EleasThea.BackEnd.Contracts.InputModels;
using EleasThea.BackEnd.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Services.Functions.Gateway
{
    public static class SendFeedbackFunction
    {
        [FunctionName("SendFeedbackFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Feedback")] HttpRequest req,
            [Queue("feedback-msgs")] ICollector<FeedbackMessage> feedbackMessagesQueue,
            ILogger logger)
        {
            try
            {
                // read body, bind to model.
                var feedback = await req.GetBodyAsObjectAsync<FeedbackMessage>();
                logger.LogInformation($"{nameof(SendFeedbackFunction)} triggered.", feedback);

                // validate model. if there are errors, return bad request.
                if (!feedback.IsModelValid(out var feedValidationResults)) return new BadRequestObjectResult(feedValidationResults);

                // enqueue item.
                feedbackMessagesQueue.Add(feedback);
                logger.LogInformation($"Message enqueued.");

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
