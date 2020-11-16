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

namespace EleasThea.BackEnd.Serverless.Api.Functions.Gateway
{
    public static class SendFeedbackFunction
    {
        [FunctionName("FeedbackFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SendFeedback")] HttpRequest req,
            [Queue("input-messages")] ICollector<string> inputMessagesQueue,
            ILogger logger)
        {
            try
            {
                // read body, bind to model.
                var feedback = await req.GetBodyAsObjectAsync<Feedback>();

                // validate model. if there are errors, return bad request.
                if (!feedback.IsModelValid(out var feedValidationResults)) return new BadRequestObjectResult(feedValidationResults);

                // create json serializer settings to include class type in order to deserialize on the other side.
                var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };

                // enqueue item.
                inputMessagesQueue.Add(JsonConvert.SerializeObject(feedback, jsonSerializerSettings));

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
