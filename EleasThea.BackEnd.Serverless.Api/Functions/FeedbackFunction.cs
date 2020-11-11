using EleasThea.BackEnd.Contracts.InputModels;
using EleasThea.BackEnd.Contracts.QueueDTOs;
using EleasThea.BackEnd.Serverless.Api.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Api.Functions
{
    public static class FeedbackFunction
    {
        [FunctionName("FeedbackFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SendFeedback")] HttpRequest req,
            [Queue("input-messages")] ICollector<InputMessageItemDTO> inputMessagesQueue,
            ILogger logger)
        {
            try
            {
                // read body, bind to model.
                var feedback = await req.GetBodyAsObjectAsync<SendFeedback>();

                // validate model. if there are errors, return bad request.
                if (!feedback.IsValid()) return new BadRequestResult();

                // map input object to queue dto object.
                var feedbackDTO = new FeedbackDTO
                {
                    Email = feedback.Email,
                    FullName = feedback.FullName,
                    Message = feedback.Message,
                    Tel = feedback.Tel
                };

                // enqueue item.
                inputMessagesQueue.Add(feedbackDTO as InputMessageItemDTO);

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
