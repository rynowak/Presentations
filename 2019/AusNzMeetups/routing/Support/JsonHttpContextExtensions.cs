using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpContextJsonExtensions
    {
        private static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions();

        public static T Get<T>(this RouteValueDictionary values, string key) where T : IConvertible
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return (T)Convert.ChangeType(values[key], typeof(T));
        }

        public static ValueTask<T> ReadJsonAsync<T>(this HttpRequest request, JsonSerializerOptions options = null)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            options ??= (JsonSerializerOptions)request.HttpContext.RequestServices.GetService(typeof(JsonSerializerOptions)) ?? DefaultOptions;
            return JsonSerializer.DeserializeAsync<T>(request.Body, options);
        }

        public static Task WriteJsonAsync<T>(this HttpResponse response, T value, JsonSerializerOptions options = null)
        {
            if (response is null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            response.ContentType ??= "application/json;charset=\"UTF-8\"";

            options ??= (JsonSerializerOptions)response.HttpContext.RequestServices.GetService(typeof(JsonSerializerOptions)) ?? DefaultOptions;
            return JsonSerializer.SerializeAsync<T>(response.Body, value, options);
        }
    }
}
