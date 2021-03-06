using EleasThea.BackEnd.Contracts.Enums;
using EleasThea.BackEnd.Contracts.InputModels;
using EleasThea.BackEnd.Contracts.QueueModels;
using EleasThea.BackEnd.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Services.Functions.Gateway
{
    public static class MakeReservationFunction
    {
        [FunctionName("MakeReservationFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "{language}/Reservation/{reservationType}")] HttpRequest req,
            string language,
            string reservationType,
            [Queue("reservation-msgs")] ICollector<ReservationQueueItem> reservationMsgsQueue,
            ILogger logger)
        {
            try
            {
                // read body, bind to reservation model.
                var reservationMessage = await req.GetBodyAsObjectAsync<ReservationMessage>();

                // validate model. if there are errors, return bad request.
                if (!reservationMessage.IsModelValid(out var reservationValidationResults))
                    return new BadRequestObjectResult(reservationValidationResults);

                // map input message to queue item dto.
                var reservationQueueItemDto = reservationMessage.MapToQueueItem();

                // assign type to reservation.
                if (reservationType.ToLower() == ReservationType.CookingClass.ToString().ToLower())
                    reservationQueueItemDto.Type = ReservationType.CookingClass;
                else if (reservationType.ToLower() == ReservationType.Table.ToString().ToLower())
                    reservationQueueItemDto.Type = ReservationType.Table;
                else
                    return new BadRequestObjectResult($"{reservationType} is not a valid Reservation Type. Please use \"CookingClass\" or \"Table\"");

                // assign language to reservation.
                if (language.ToLower() == Language.EN.ToString().ToLower())
                    reservationQueueItemDto.MadeInLanguage = Language.EN;
                else if (language.ToLower() == Language.FR.ToString().ToLower())
                    reservationQueueItemDto.MadeInLanguage = Language.FR;
                else if (language.ToLower() == Language.IT.ToString().ToLower())
                    reservationQueueItemDto.MadeInLanguage = Language.IT;
                else if (language.ToLower() == Language.GR.ToString().ToLower())
                    reservationQueueItemDto.MadeInLanguage = Language.GR;
                else
                    return new BadRequestObjectResult($"{language} is not a valid language. Please use \"en\", \"fr\", \"it\" or \"gr\"");

                // enqueue item.
                reservationMsgsQueue.Add(reservationQueueItemDto);

                return new AcceptedResult();
            }
            catch (Exception exc)
            {
                // log error.
                logger.LogError($"Message: {exc.Message}, InnerExc: {exc.InnerException.Message}");

                // return internal server error.
                return new ContentResult { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
