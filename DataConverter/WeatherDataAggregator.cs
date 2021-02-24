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

                dataForYear.LongestNumberOfDaysRaining = FindLongestNumberOfDaysRaining(dataForYear.AllReadings);

                foreach (var dataForMonth in dataForYear.MonthlyAggregates)
                {
                    SummariseMonth(dataForMonth, dataForYear.AllReadings);
                }

                summary.WeatherData.Add(dataForYear);
            }

            return summary;
        }

        private static void AggregateYearData(WeatherDataForYear yearData, BomRainfallData bomRainfallData)
        {
            var rainfallReading = new RainfallReading(bomRainfallData.GetDate(), bomRainfallData.GetRainfall());

            if (yearData.FirstRecordedDate == null || rainfallReading.Date < yearData.FirstRecordedDate)
            {
                yearData.FirstRecordedDate = rainfallReading.Date;
            }

            if (yearData.LastRecordedDate == null || rainfallReading.Date > yearData.LastRecordedDate)
            {
                yearData.LastRecordedDate = rainfallReading.Date;
            }

            if (rainfallReading.Rainfall > 0)
            {
                yearData.TotalRainfall += rainfallReading.Rainfall;
                yearData.DaysWithRainfall++;
            }
            else
            {
                yearData.DaysWithNoRainfall++;
            }

            yearData.AllReadings.Add(rainfallReading);

            var monthData = yearData.MonthlyAggregates.FirstOrDefault(month => month.Month == rainfallReading.Date.Month);

            if (monthData == null)
            {
                monthData = new WeatherDataForMonth()
                {
                    Month = rainfallReading.Date.Month
                };

                yearData.MonthlyAggregates.Add(monthData);
            }

            AggregateMonthData(monthData, rainfallReading);
        }

        private static void AggregateMonthData(WeatherDataForMonth monthData, RainfallReading rainfallReading)
        {
            if (monthData.FirstRecordedDate == null || rainfallReading.Date < monthData.FirstRecordedDate)
            {
                monthData.FirstRecordedDate = rainfallReading.Date;
            }

            if (monthData.LastRecordedDate == null || rainfallReading.Date > monthData.LastRecordedDate)
            {
                monthData.LastRecordedDate = rainfallReading.Date;
            }

            if (rainfallReading.Rainfall > 0)
            {
                monthData.TotalRainfall += rainfallReading.Rainfall;
                monthData.DaysWithRainfall++;
            }
            else
            {
                monthData.DaysWithNoRainfall++;
            }
        }

        private static void SummariseMonth(WeatherDataForMonth dataForMonth, IList<RainfallReading> allReadingsForYear)
        {
            dataForMonth.AverageDailyRainfall = dataForMonth.TotalRainfall /
                                                (dataForMonth.DaysWithNoRainfall + dataForMonth.DaysWithRainfall);
            
            dataForMonth.MedianDailyRainfall = GetMedian(allReadingsForYear.Where(r => r.Date.Month == dataForMonth.Month));
        }

        private static int FindLongestNumberOfDaysRaining(IList<RainfallReading> readings)
        {
            if (readings.Count == 0)
            {
                return 0;
            }

            if (readings.Count == 1)
            {
                return 1;
            }

            var sortedData = readings.OrderBy(r => r.Date).ToList();

            var longestNumberOfDaysRaining = 0;
            var currentNumberOfDaysRaining = 1;
            var lastDate = readings[0].Date;

            for (var i = 1; i < sortedData.Count; i ++)
            {
                if (sortedData[i].Rainfall > 0 && sortedData[i].Date == lastDate.AddDays(1))
                {
                    currentNumberOfDaysRaining++;
                }
                else
                {
                    if (currentNumberOfDaysRaining > longestNumberOfDaysRaining)
                    {
                        longestNumberOfDaysRaining = currentNumberOfDaysRaining;
                    }

                    currentNumberOfDaysRaining = 1;
                }

                lastDate = readings[i].Date;
            }

            return currentNumberOfDaysRaining > longestNumberOfDaysRaining ? currentNumberOfDaysRaining : longestNumberOfDaysRaining;
        }

        private static decimal GetMedian(IEnumerable<RainfallReading> data)
        {
            var sortedData = data.OrderBy(r => r.Rainfall).ToList();

            if (sortedData.Count == 0)
            {
                return 0;
            }

            if (sortedData.Count == 1)
            {
                return sortedData[0].Rainfall;
            }

            if (sortedData.Count % 2 == 1)
            {
                return sortedData[sortedData.Count / 2].Rainfall;
            }

            var lower = sortedData[(sortedData.Count - 1) / 2].Rainfall;
            var upper = sortedData[(sortedData.Count + 1) / 2].Rainfall;

            return (lower + upper) / 2;
        }
    }
}
