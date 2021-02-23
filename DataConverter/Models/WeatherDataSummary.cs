using System.Collections.Generic;

namespace DataConverter.Models
{
    public class WeatherDataSummary
    {
        public WeatherDataSummary()
        {
            WeatherData = new List<WeatherDataForYear>();
        }

        public IList<WeatherDataForYear> WeatherData { get; }
    }
}
