using System;
using System.Text.Json.Serialization;

namespace weatherapp_route_to_code
{
    public class WeatherReport
    {
        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("forecast")]
        public string Forecast { get; set; }
    }
}
