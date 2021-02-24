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
        
        [JsonIgnore]
        public DateTime? FirstDate { get; set; }

        public string FirstRecordedDate => FirstDate.HasValue ? FirstDate.Value.ToString("yyyy-MM-dd") : string.Empty;

        [JsonIgnore]
        public DateTime? LastDate { get; set; }

        public string LastRecordedDate => LastDate.HasValue ? LastDate.Value.ToString("yyyy-MM-dd") : string.Empty;

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
