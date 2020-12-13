using EleasThea.BackEnd.Contracts.QueueModels;
using EleasThea.BackEnd.Contracts.TableStorageModels;
using EleasThea.BackEnd.Extentions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Services.Functions
{
    public static class ProcessReservationApprovalFunction
    {
        // Non operational.
        [Disable]
        [FunctionName("ProcessReservationApprovalFunction")]
        public static async Task RunAsync(
            [QueueTrigger("reservation-approved-wbhks")] string approvedReservationId,
            [Table("Reservations")] CloudTable reservationsTable,
            [Queue("send-emails")] ICollector<SendEmailQueueItem> sendEmailsQueueCollector,
            [Blob("resources")] CloudBlobContainer container,
            ILogger logger)
        {
            #region update reservation's status
            // form query to get reservation by reservationId.
            var query = new TableQuery<Reservation>().Where(TableQuery.GenerateFilterCondition(nameof(Reservation.RowKey),
                                                                                               QueryComparisons.Equal,
                                                                                               approvedReservationId));
            // execute query and get results.
            var reservation = (await reservationsTable.ExecuteQuerySegmentedAsync(query, null)).Results.FirstOrDefault();

            // set approved properties.
            reservation.Approved = true;
            reservation.ApprovedOnUtc = DateTime.UtcNow;

            // insert or merge transmission as read.
            var insertOrMergeTableOperation = TableOperation.Merge(reservation);

            // execute operation.
            await reservationsTable.ExecuteAsync(insertOrMergeTableOperation);
            #endregion

            #region prepare email body for customer.
            // get template from blob storage.
            var customerEmailHtmlReference = container.GetBlockBlobReference("templates/ReservationAccepted.html");
            var customerEmailHtmlStream = new MemoryStream();
            await customerEmailHtmlReference.DownloadToStreamAsync(customerEmailHtmlStream);
            var customerEmailHtmlStr = Encoding.UTF8.GetString(customerEmailHtmlStream.ToArray());

            // todo: implement template value replacing here.
            #endregion

            #region enqueue for email sending.
            sendEmailsQueueCollector.Add(new SendEmailQueueItem
            {
                HtmlContent = customerEmailHtmlStr,
                ReciepientAddress = reservation.Email,
                Subject = "Reservation Approved"
            });
            #endregion
        }
    }
}
