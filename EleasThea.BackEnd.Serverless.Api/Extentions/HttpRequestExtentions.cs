using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;

namespace EleasThea.BackEnd.Serverless.Api.Extentions
{
    public static class HttpRequestExtentions
    {
        /// <summary>
        /// Get C# class instance from HTTP request body.
        /// </summary>
        /// <typeparam name="T">The type of class you wish to deserialize into.</typeparam>
        /// <param name="req">The HTTP request.</param>
        /// <returns>An instance of requested class type.</returns>
        public static async Task<T> GetBodyAsObjectAsync<T>(this HttpRequest req) where T : class
        {
            using (var requestBodyAsStream = new StreamReader(req.Body))
            {
                return JsonConvert.DeserializeObject<T>((await requestBodyAsStream.ReadToEndAsync()), new JsonSerializerSettings { });
            }
        }
    }
}
