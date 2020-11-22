using EleasThea.BackEnd.Contracts.TableStorageModels;
using EleasThea.BackEnd.Extentions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Services.Functions
{
    public static class ProcessReservationApprovalFunction
    {
        [FunctionName("ProcessReservationApprovalFunction")]
        public static async Task RunAsync(
            [QueueTrigger("reservation-approved-wbhks")] string approvedReservationId,
            [Table("Reservations")] CloudTable reservationsTable,
            ILogger logger)
        {
            var reservationToUpdate = new Reservation()
            {
                Approved = true,
                ApprovedOnUtc = DateTime.UtcNow
            };

            // assign row and partition keys as the transmission id to updated in the table.
            reservationToUpdate.GeneratePartitionAndRowKeys(approvedReservationId);

            // insert or merge transmission as read.
            var insertOrMergeTableOperation = TableOperation.InsertOrMerge(reservationToUpdate);

            // execute operation.
            await reservationsTable.ExecuteAsync(insertOrMergeTableOperation);



        }
    }
}
