using EleasThea.BackEnd.Contracts.InputModels;
using EleasThea.BackEnd.Contracts.QueueDTOs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EleasThea.BackEnd.Serverless.Api.Functions
{
    public static class ProcessInputMessageFunction
    {
        [FunctionName("ProcessInputMessageFunction")]
        public static void Run([QueueTrigger("input-messages")] string inputMessageItemStr,
                               ILogger logger)
        {
            // create json serializer settings to include class type.
            var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };

            var inputMessageItem = JsonConvert.DeserializeObject(inputMessageItemStr, jsonSerializerSettings);

        }
    }
}
