using System.Collections.Concurrent;

namespace App
{
    public class WeatherSuperComputer
    {
        private readonly ConcurrentDictionary<string, WeatherForecast> _weather = new ConcurrentDictionary<string, WeatherForecast>();

        public WeatherForecast GetForecast(string place)
        {
            return _weather.GetOrAdd(place, WeatherForecast.MakeRandom);
        }

        public void UpdateForecast(string place, WeatherForecast forecast)
        {
            _weather[place] = forecast;
        }
    }
}
