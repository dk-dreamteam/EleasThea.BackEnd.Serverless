using EleasThea.BackEnd.Contracts.InputModels;
using EleasThea.BackEnd.Extentions;
using EleasThea.BackEnd.Serverless.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Api.Functions.Gateway
{
    public static class MakeReservationFunction
    {
        [FunctionName("MakeReservationFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Reservation/{reservationType}")] HttpRequest req, string reservationType,
            [Queue("input-messages")] ICollector<string> inputMessagesQueue,
            ILogger logger)
        {
            try
            {
                // read body, bind to appopriate model.
                ReservationMessage reservation;

                if (reservationType.ToLower() == ReservationType.CookingClass.ToString().ToLower())
                    reservation = await req.GetBodyAsObjectAsync<CookingClassReservationMessage>();
                else if (reservationType.ToLower() == ReservationType.Table.ToString().ToLower())
                    reservation = await req.GetBodyAsObjectAsync<TableReservationMessage>();
                else
                    return new BadRequestObjectResult($"{reservationType} is not a valid Reservation Type. Please use \"CookingClass\" or \"Table\"");

                // validate model. if there are errors, return bad request.
                if (!reservation.IsModelValid(out var reservationValidationResults)) return new BadRequestObjectResult(reservationValidationResults);

                // create json serializer settings to include class type in order to deserialize on the other side.
                var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };

                // enqueue item.
                inputMessagesQueue.Add(JsonConvert.SerializeObject(reservation, jsonSerializerSettings));

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
