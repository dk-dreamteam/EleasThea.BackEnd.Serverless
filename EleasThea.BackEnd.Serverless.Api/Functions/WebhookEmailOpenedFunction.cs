using EleasThea.BackEnd.Contracts.TableStorageModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using EleasThea.BackEnd.Extentions;
using System.Web;

namespace EleasThea.BackEnd.Serverless.Services.Functions
{
    public static class WebhookEmailOpenedFunction
    {
        [FunctionName(nameof(WebhookEmailOpenedFunction))]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("%TransmissionsTableName%")] CloudTable transmissionsTable,
            ILogger log)
        {
            // get transmission row key and email address from query params.
            var transmissionRowKey = req.Query["trk"];
            var emailAddress = HttpUtility.UrlDecode(req.Query["addr"]);

            // create a new transmission with an updated opened status.
            // and assign row and partition keys as the transmission id to updated in the table.
            var transmissionToUpdate = new Transmission()
            {
                Opened = true,
            }
            .GeneratePartitionAndRowKeys(emailAddress, transmissionRowKey);

            // insert or merge transmission as read.
            var insertOrMergeTableOperation = TableOperation.Merge(transmissionToUpdate);

            // execute operation.
            await transmissionsTable.ExecuteAsync(insertOrMergeTableOperation);

            // create response to avoid caching and be able to reregister next opening event.
            req.HttpContext.Response.Headers.Add("Cache-Control", "no-store, max-age=0"); // for HTTP 1.1
            req.HttpContext.Response.Headers.Add("Pragma", "no-cache"); // for HTTP 1.0.
            return new OkResult();
        }
    }
}
