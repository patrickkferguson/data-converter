using System.Collections.Generic;
using System.Linq;
using DataConverter.Models;

namespace DataConverter
{
    public class WeatherDataAggregator
    {
        private readonly Dictionary<int, WeatherDataForYear> _yearlyData;

        public WeatherDataAggregator()
        {
            _yearlyData = new Dictionary<int, WeatherDataForYear>();
        }

        public void Aggregate(BomRainfallData bomRainfallData)
        {
            if (!bomRainfallData.Validate())
            {
                return;
            }

            var date = bomRainfallData.GetDate();

            if (!_yearlyData.TryGetValue(date.Year, out var yearData))
            {
                yearData = new WeatherDataForYear()
                {
                    Year = date.Year
                };

                _yearlyData.Add(date.Year, yearData);
            }

            AggregateYearData(yearData, bomRainfallData);
        }

        public WeatherDataSummary GetSummary()
        {
            var summary = new WeatherDataSummary();

            foreach (var yearData in _yearlyData.OrderBy(kvp => kvp.Key))
            {
                var dataForYear = yearData.Value;

                dataForYear.AverageDailyRainfall = dataForYear.TotalRainfall /
                                                   (dataForYear.DaysWithNoRainfall + dataForYear.DaysWithRainfall);

                foreach (var dataForMonth in dataForYear.MonthlyAggregates)
                {
                    dataForMonth.AverageDailyRainfall = dataForMonth.TotalRainfall /
                                                     (dataForMonth.DaysWithNoRainfall + dataForMonth.DaysWithRainfall);
                }

                summary.WeatherData.Add(dataForYear);
            }

            return summary;
        }

        private static void AggregateYearData(WeatherDataForYear yearData, BomRainfallData bomRainfallData)
        {
            var date = bomRainfallData.GetDate();

            if (yearData.FirstRecordedDate == null || date < yearData.FirstRecordedDate)
            {
                yearData.FirstRecordedDate = date;
            }

            if (yearData.LastRecordedDate == null || date > yearData.LastRecordedDate)
            {
                yearData.LastRecordedDate = date;
            }

            var rainfall = bomRainfallData.GetRainfall();

            if (rainfall > 0)
            {
                yearData.TotalRainfall += rainfall;
                yearData.DaysWithRainfall++;
            }
            else
            {
                yearData.DaysWithNoRainfall++;
            }

            var monthData = yearData.MonthlyAggregates.FirstOrDefault(month => month.Month == date.Month);

            if (monthData == null)
            {
                monthData = new WeatherDataForMonth()
                {
                    Month = date.Month
                };

                yearData.MonthlyAggregates.Add(monthData);
            }

            AggregateMonthData(monthData, bomRainfallData);
        }

        private static void AggregateMonthData(WeatherDataForMonth monthData, BomRainfallData bomRainfallData)
        {
            var date = bomRainfallData.GetDate();

            if (monthData.FirstRecordedDate == null || date < monthData.FirstRecordedDate)
            {
                monthData.FirstRecordedDate = date;
            }

            if (monthData.LastRecordedDate == null || date > monthData.LastRecordedDate)
            {
                monthData.LastRecordedDate = date;
            }

            var rainfall = bomRainfallData.GetRainfall();

            if (rainfall > 0)
            {
                monthData.TotalRainfall += rainfall;
                monthData.DaysWithRainfall++;
            }
            else
            {
                monthData.DaysWithNoRainfall++;
            }
        }
    }
}
