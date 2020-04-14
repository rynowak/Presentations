using System;
using System.Text.Json.Serialization;

namespace forecastapp_route_to_code
{
    public class WeatherForecast
    {
        [JsonPropertyName("weather")]
        public string Weather { get; set; }
    }
}
