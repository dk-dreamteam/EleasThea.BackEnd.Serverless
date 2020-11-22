using EleasThea.BackEnd.Contracts.InputModels;
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
        public static void Run([QueueTrigger("reservation-msgs")] ReservationMessage inputReservation,
                                   [Table("TableReservations")] CloudTable tableReservationsTable,
                                   [Table("CookingClassReservations")] CloudTable cookingClassReservationsTable,
                                   [Queue("send-emails")] ICollector<SendEmailQueueItem> sendEmailsQueueCollector,
                                   [Blob("resources")] CloudBlobContainer container,
                                   ILogger logger)
        {
            // create json serializer settings to include class type.
            //var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            //var inputMessageItem = JsonConvert.DeserializeObject(inputReservation, jsonSerializerSettings);
        }
    }
}
