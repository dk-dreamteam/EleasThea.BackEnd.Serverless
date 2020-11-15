using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Api.Functions
{
    public static class WebhookEmailOpenedFunction
    {
        [FunctionName("WebhookEmailOpenedFunction")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            var responseΜessage = req.CreateResponse(HttpStatusCode.OK);
            responseΜessage.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = new TimeSpan(0),
                Private = true,
                NoCache = true,
                NoStore = true,
                MustRevalidate = true,
            };
            responseΜessage.Headers.Pragma.Add(new NameValueHeaderValue("no-cache"));
            return responseΜessage;

            //req.HttpContext.Response.Headers["Cache-Control"] = "private, max-age=0";

            //return new OkResult();

        }
    }
}
