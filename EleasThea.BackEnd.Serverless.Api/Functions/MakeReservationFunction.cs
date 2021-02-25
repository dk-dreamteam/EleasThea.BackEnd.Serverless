using EleasThea.BackEnd.Contracts.Enums;
using EleasThea.BackEnd.Contracts.InputModels;
using EleasThea.BackEnd.Contracts.QueueModels;
using EleasThea.BackEnd.Contracts.TableStorageModels;
using EleasThea.BackEnd.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Services.Functions
{
    public static class MakeReservationFunction
    {
        [FunctionName(nameof(MakeReservationFunction))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "reservations/{language}/{reservationType}")] HttpRequest req,
            string language,
            string reservationType,
            [Table("%ReservationsTableName%")] CloudTable reservationsTable,
            [Queue("%SendEmailQueueName%")] ICollector<SendEmailQueueItem> sendEmailsQueue,
            [Blob("templates")] CloudBlobContainer container,
            ILogger logger)
        {
            try
            {
                logger.LogInformation($"Function:{nameof(MakeReservationFunction)} started executing.");

                // read input reservation and validate.
                var reservationMessage = await req.GetBodyAsObjectAsync<ReservationMessage>();

                logger.LogInformation($"ReservationMakerEmail:{reservationMessage.Email}");

                if (!reservationMessage.IsModelValid(out var reservationValidationResults))
                    return new BadRequestObjectResult(reservationValidationResults);


                // map input model to reservation instance and fill properties according to type.
                var reservation = reservationMessage.MapToTableEntity()
                                                    .AssignReservationType(reservationType)
                                                    .AssignLanguage(language)
                                                    .GeneratePartitionAndRowKeys<Reservation>(reservationMessage.Email, null);


                // save reservation to the reservations table.
                var insertIntoTableOperation = TableOperation.Insert(reservation);
                var insertionRes = await reservationsTable.ExecuteAsync(insertIntoTableOperation);

                logger.LogInformation($"Reservation Insertion result:{insertionRes.HttpStatusCode}");


                // retrieve email html templates for both customer and restaurant.
                using var customerEmailHtmlStream = new MemoryStream();
                await container.GetBlockBlobReference($"customer/reservation/{reservation.MadeInLanguage.ToString().ToLower()}/{GetHtmlTemplateForCustomerFileName(reservation.Type)}")
                               .DownloadToStreamAsync(customerEmailHtmlStream);
                var customerEmailHtmlAsString = Encoding.UTF8.GetString(customerEmailHtmlStream.ToArray());
                // todo: replace template vars with real data.

                using var restaurantEmailHtmlStream = new MemoryStream();
                await container.GetBlockBlobReference($"restaurant/{GetHtmlReservationTemplateForRestaurantFileName(reservation.Type)}")
                               .DownloadToStreamAsync(customerEmailHtmlStream);
                var restaurantEmailHtmlAsString = Encoding.UTF8.GetString(restaurantEmailHtmlStream.ToArray());
                // todo: replace template vars with real data.


                // enque emails for customer and restaurant.
                sendEmailsQueue.Add(new SendEmailQueueItem()
                {
                    ReferenceToReservationRowKey = reservation.RowKey,
                    HtmlContent = customerEmailHtmlAsString,
                    ReciepientAddress = reservation.Email,
                    Subject = GetCustomerEmailSubjectForReservationInLanguage(reservation.MadeInLanguage)
                });

                sendEmailsQueue.Add(new SendEmailQueueItem()
                {
                    ReferenceToReservationRowKey = reservation.RowKey,
                    HtmlContent = restaurantEmailHtmlAsString,
                    ReciepientAddress = "kon.kri@outlook.com", // todo: replace with real email.
                    Subject = "Νέα Κράτηση"
                });

                logger.LogInformation($"Email object enqueued. Returning accepted result. stopping function...");
                return new AcceptedResult();
            }
            catch (ArgumentException argExc)
            {
                logger.LogError($"Message: {argExc.Message}");
                return new BadRequestObjectResult(argExc.Message);
            }
            catch (Exception exc)
            {
                logger.LogError($"Message: {exc.Message}, InnerExc: {exc.InnerException.Message}");
                return new ContentResult { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }

        /// <summary>
        /// Get Html Template file name depending on the reservation type.
        /// </summary>
        /// <param name="reservationType">Reservation type.</param>
        /// <returns></returns>
        private static string GetHtmlTemplateForCustomerFileName(ReservationType reservationType)
        {
            switch (reservationType)
            {
                case ReservationType.Table:
                    return "ThankYouForYourTableReservation.html";
                case ReservationType.CookingClass:
                    return "ThankYouForYourCookingClassReservation.html";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Get Html Template file name depending on the reservation type.
        /// </summary>
        /// <param name="reservationType">Reservation type.</param>
        /// <returns></returns>
        private static string GetHtmlReservationTemplateForRestaurantFileName(ReservationType reservationType)
        {
            switch (reservationType)
            {
                case ReservationType.Table:
                    return "NewTableReservation.html";
                case ReservationType.CookingClass:
                    return "NewCookingClassReservation.html";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Get Email subject depending on the language.
        /// </summary>
        /// <param name="language">Language given.</param>
        /// <returns></returns>
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
