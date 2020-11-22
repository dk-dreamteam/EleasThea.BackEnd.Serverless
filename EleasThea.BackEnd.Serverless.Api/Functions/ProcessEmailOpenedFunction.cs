using EleasThea.BackEnd.Contracts.TableStorageModels;
using EleasThea.BackEnd.Extentions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Services.Functions
{
    public static class ProcessEmailOpenedFunction
    {
        [FunctionName("ProcessEmailOpenedFunction")]
        public static async Task RunAsync([QueueTrigger("email-opened-wbhks")] string transmissionId,
                                          [Table("Transmissions")] CloudTable transmissionsTable,
                                          ILogger logger)
        {
            // create a new transmission with an updated opened status.
            var transmissionToUpdate = new Transmission()
            {
                Opened = true,
                LastOpenedOnUtc = DateTime.UtcNow
            };

            // assign row and partition keys as the transmission id to updated in the table.
            transmissionToUpdate.GeneratePartitionAndRowKeys(transmissionId);

            // insert or merge transmission as read.
            var insertOrMergeTableOperation = TableOperation.InsertOrMerge(transmissionToUpdate);

            // execute operation.
            await transmissionsTable.ExecuteAsync(insertOrMergeTableOperation);
        }
    }
}
