using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataConverter.Models
{
    public class WeatherDataForMonth
    {
        public int Month { get; set; }
        
        public DateTime? FirstRecordedDate { get; set; }
        
        public DateTime? LastRecordedDate { get; set; }
        
        public decimal TotalRainfall { get; set; }
        
        public decimal AverageDailyRainfall { get; set; }
        
        public decimal MedianDailyRainfall { get; set; }
        
        public int DaysWithNoRainfall { get; set; }
        
        public int DaysWithRainfall { get; set; }

        [JsonIgnore]
        public IList<decimal> AllReadings { get; } = new List<decimal>();
    }
}
