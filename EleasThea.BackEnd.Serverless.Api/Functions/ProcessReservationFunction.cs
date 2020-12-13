using EleasThea.BackEnd.Contracts.Enums;
using EleasThea.BackEnd.Contracts.QueueModels;
using EleasThea.BackEnd.Extentions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;
using System.Text;

namespace EleasThea.BackEnd.Serverless.Services.Functions
{
    public static class ProcessReservationFunction
    {
        [FunctionName("ProcessReservationFunction")]
        public static async System.Threading.Tasks.Task RunAsync([QueueTrigger("reservation-msgs")] ReservationQueueItem reservationQueueItem,
                               [Table("Reservations")] CloudTable reservationsTable,
                               [Queue("send-emails")] ICollector<SendEmailQueueItem> sendEmailsQueue,
                               [Blob("resources")] CloudBlobContainer container,
                               ILogger logger)
        {
            #region save reservation to table.
            // use custom extention method to map input model to table entity derived class model.
            var reservation = reservationQueueItem.MapToTableEntity();

            // generate partition and row keys.
            reservation.GeneratePartitionAndRowKeys(reservation.Email, null);
            var insertIntoTableOperation = TableOperation.Insert(reservation);
            await reservationsTable.ExecuteAsync(insertIntoTableOperation);
            #endregion

            string customerEmailHtmlStr;
            string restaurantEmailHtmlStr;

            switch (reservationQueueItem.Type)
            {
                case ReservationType.Table:
                    {
                        #region prepare email body for customer.
                        // get template from blob storage for correct language..
                        using var customerEmailHtmlStream = new MemoryStream();

                        await container.GetBlockBlobReference($"templates/customer/reservation/{reservationQueueItem.MadeInLanguage.ToString().ToLower()}/ThankYouForYourTableReservation.html")
                                       .DownloadToStreamAsync(customerEmailHtmlStream);

                        customerEmailHtmlStr = Encoding.UTF8.GetString(customerEmailHtmlStream.ToArray());

                        // todo: implement template value replacing here.
                        #endregion

                        #region prepare email body for restaurant.
                        // get template from blob storage.
                        using var restaurantEmailHtmlStream = new MemoryStream();

                        await container.GetBlockBlobReference("templates/restaurant/NewTableReservation.html")
                                       .DownloadToStreamAsync(restaurantEmailHtmlStream);

                        restaurantEmailHtmlStr = Encoding.UTF8.GetString(restaurantEmailHtmlStream.ToArray());

                        // todo: implement template value replacing here.
                        #endregion
                    }
                    break;
                case ReservationType.CookingClass:
                    {
                        #region prepare email body for customer.
                        // get template from blob storage for correct language..
                        using var customerEmailHtmlStream = new MemoryStream();

                        await container.GetBlockBlobReference($"templates/customer/reservation/{reservationQueueItem.MadeInLanguage.ToString().ToLower()}/ThankYouForYourCookingClassReservation.html")
                                       .DownloadToStreamAsync(customerEmailHtmlStream);

                        customerEmailHtmlStr = Encoding.UTF8.GetString(customerEmailHtmlStream.ToArray());

                        // todo: implement template value replacing here.
                        #endregion

                        #region prepare email body for restaurant.
                        // get template from blob storage.
                        using var restaurantEmailHtmlStream = new MemoryStream();

                        await container.GetBlockBlobReference("templates/restaurant/NewCookingClassReservation.html")
                                       .DownloadToStreamAsync(restaurantEmailHtmlStream);

                        restaurantEmailHtmlStr = Encoding.UTF8.GetString(restaurantEmailHtmlStream.ToArray());

                        // todo: implement template value replacing here.
                        #endregion
                    }
                    break;
                default:
                    {
                        customerEmailHtmlStr = string.Empty;
                        restaurantEmailHtmlStr = string.Empty;
                    }
                    break;
            }

            #region send email to customer.
            // enqueue to send emails queue.
            sendEmailsQueue.Add(new SendEmailQueueItem()
            {
                ReferenceToReservationRowKey = reservation.RowKey,
                HtmlContent = customerEmailHtmlStr,
                ReciepientAddress = reservation.Email,
                Subject = GetCustomerEmailSubjectForReservationInLanguage(reservation.MadeInLanguage)
            });
            #endregion

            #region send email to restaurant.
            // enqueue to send emails queue.
            sendEmailsQueue.Add(new SendEmailQueueItem()
            {
                ReferenceToReservationRowKey = reservation.RowKey,
                HtmlContent = restaurantEmailHtmlStr,
                ReciepientAddress = "eleasthea@", // todo: replace with real email.
                Subject = "Νέα Κράτηση"
            });
            #endregion
        }

        /// <summary>
        /// Get Email subject depending on language.
        /// </summary>
        /// <param name="language">The language in which the reservation was made.</param>
        /// <returns>Email Subject in correct language.</returns>
        private static string GetCustomerEmailSubjectForReservationInLanguage(Language language)
        {
            switch (language)
            {
                case Language.EN:
                    return "Thank you for your table reservation!";
                case Language.FR:
                    return "Merci pour votre réservation de table!";
                case Language.IT:
                    return "Grazie per la prenotazione del tavolo!";
                case Language.GR:
                    return "Ευχαριστούμε για την κράτηση τραπεζιού";
                default:
                    return string.Empty;
            }
        }
    }
}
