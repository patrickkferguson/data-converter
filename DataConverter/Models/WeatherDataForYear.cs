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
        
        [JsonIgnore]
        public DateTime? FirstDate { get; set; }

        public string FirstRecordedDate => FirstDate.HasValue ? FirstDate.Value.ToString("yyyy-MM-dd") : string.Empty;

        [JsonIgnore]
        public DateTime? LastDate { get; set; }

        public string LastRecordedDate => LastDate.HasValue ? LastDate.Value.ToString("yyyy-MM-dd") : string.Empty;

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
