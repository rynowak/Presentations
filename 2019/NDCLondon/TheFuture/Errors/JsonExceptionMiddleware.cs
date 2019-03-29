using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Endpoints;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace TheFuture.Errors
{
    public class JsonExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public JsonExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception) when (!httpContext.Response.HasStarted)
            {
                var severity = Severity.NawRelaxItsOk;

                var response = new
                {
                    message = "An error occurred. Gee we're real sorry about that!",
                    severity = severity.ToString(),
                };

                using (var writer = new HttpResponseStreamWriter(httpContext.Response.Body, Encoding.UTF8))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(writer, response);
                }
            }
        }
    }
}
