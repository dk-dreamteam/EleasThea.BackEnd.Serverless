using EleasThea.BackEnd.Contracts.QueueModels;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace EleasThea.BackEnd.Serverless.Services.Functions
{
    public static class ProcessReservationFunction
    {
        [FunctionName("ProcessReservationFunction")]
        public static void Run([QueueTrigger("reservation-msgs")] ReservationQueueItem reservationQueueItem,
                               [Table("Reservations")] CloudTable reservationsTable,
                               [Queue("send-emails")] ICollector<SendEmailQueueItem> sendEmailsQueue,
                               [Blob("resources")] CloudBlobContainer container,
                               ILogger logger)
        {
            // todo: implement reservation handling here.
        }
    }
}
