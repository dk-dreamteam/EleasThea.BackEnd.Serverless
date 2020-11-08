using EleasThea.BackEnd.Serverless.Api.Extentions;
using EleasThea.BackEnd.Serverless.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Api.Functions
{
    public static class FeedbackFunction
    {
        [FunctionName("FeedbackFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // read body, parse to class instance and validate object.
            var feedback = await req.GetBodyAsObjectAsync<Feedback>();                                   
            if (!feedback.IsValid())
                return new BadRequestResult();

            // send email.

            // return ok.
        }
    }
}
