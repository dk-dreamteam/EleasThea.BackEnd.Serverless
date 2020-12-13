using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Services.Functions
{
    public static class WebhookEmailOpenedFunction
    {
        [FunctionName("WebhookEmailOpenedFunction")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Queue("email-opened-wbhks")] ICollector<string> emailOpenedTransIdsCollector,
            ILogger log)
        {
            // get transmission row key from query param.
            var transmissionRowKey = req.Query["trk"];

            // enqueue transmissionId to be processed later.
            emailOpenedTransIdsCollector.Add(transmissionRowKey);

            // create response to avoid caching and be able to reregister next opening event.
            req.HttpContext.Response.Headers.Add("Cache-Control", "no-store, max-age=0"); // for HTTP 1.1
            req.HttpContext.Response.Headers.Add("Pragma", "no-cache"); // for HTTP 1.0.
            return new OkResult();
        }
    }
}
