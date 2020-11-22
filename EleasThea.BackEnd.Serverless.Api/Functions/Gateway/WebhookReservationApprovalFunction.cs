using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Services.Functions.Gateway
{
    public static class WebhookReservationApprovalFunction
    {
        [FunctionName("WebhookReservationApprovalFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Reservation/Approval/{reservationType}")] HttpRequest req,
            string reservationType,
            [Queue("reservation-approved-wbhks")] ICollector<string> approvedReservationIdsCollector,
            ILogger logger)
        {
            var reservationId = req.Query["rId"];
            approvedReservationIdsCollector.Add(reservationId);
            return new OkObjectResult($"H Αποδοχή κράτησης για κωδικό κράτησης {reservationId} έχει δρομολογηθεί. Ο χρήστης που έκανε την κράτηση θα ενημερωθεί άμμεσα με email. Ευχαριστούμε πολύ.");
        }
    }
}
