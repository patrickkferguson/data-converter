using System;
using System.Text.Json.Serialization;

namespace DataConverter.Models
{
    public class WeatherDataForMonth
    {
        private WeatherDataForMonth(int monthNumber, string monthName)
        {
            MonthNumber = monthNumber;
            MonthName = monthName;
        }

        [JsonIgnore]
        public int MonthNumber { get; }

        [JsonPropertyName("Month")]
        public string MonthName { get; }
        
        public DateTime? FirstRecordedDate { get; set; }
        
        public DateTime? LastRecordedDate { get; set; }
        
        public decimal TotalRainfall { get; set; }
        
        public decimal AverageDailyRainfall { get; set; }
        
        public decimal MedianDailyRainfall { get; set; }
        
        public int DaysWithNoRainfall { get; set; }
        
        public int DaysWithRainfall { get; set; }

        public static WeatherDataForMonth FromDate(DateTime dateTime)
        {
            return new WeatherDataForMonth(dateTime.Month, dateTime.ToString("MMMM"));
        }
    }
}
