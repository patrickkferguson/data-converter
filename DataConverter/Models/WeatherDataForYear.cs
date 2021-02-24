using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataConverter.Models
{
    public class WeatherDataForYear
    {
        public WeatherDataForYear()
        {
            MonthlyAggregates = new List<WeatherDataForMonth>();
        }

        public int Year { get; set; }
        
        public DateTime? FirstRecordedDate { get; set; }

        public DateTime? LastRecordedDate { get; set; }

        public decimal TotalRainfall { get; set; }

        public decimal AverageDailyRainfall { get; set; }

        public int DaysWithNoRainfall { get; set; }

        public int DaysWithRainfall { get; set; }

        public int LongestNumberOfDaysRaining { get; set; }

        public IList<WeatherDataForMonth> MonthlyAggregates { get; }

        [JsonIgnore]
        public IList<RainfallReading> AllReadings { get; } = new List<RainfallReading>();
    }
}
