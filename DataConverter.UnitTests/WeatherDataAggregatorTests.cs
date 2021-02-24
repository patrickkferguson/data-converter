using System;
using DataConverter.Models;
using FluentAssertions;
using Xunit;

namespace DataConverter.UnitTests
{
    public class WeatherDataAggregatorTests
    {
        private static readonly string[] SampleData = {
            "IDCJAC0009,066062,2020,10,01,7.3,,",
            "IDCJAC0009,066062,2020,11,01,8.4,,",
            "IDCJAC0009,066062,2021,01,01,10.45,,",
            "IDCJAC0009,066062,2021,02,05,3.1,,",
            "IDCJAC0009,066062,2021,02,06,3.1,,",
            "IDCJAC0009,066062,2021,02,07,0,,",
            "IDCJAC0009,066062,2021,02,23,7.8,,",
            "IDCJAC0009,066062,2021,03,14,1.2,,",
            "IDCJAC0009,066062,2021,03,15,2.2,,",
            "IDCJAC0009,066062,2021,03,16,8.5,,",
            "IDCJAC0009,066062,2021,03,17,12.46,,",
            "IDCJAC0009,066062,2021,03,18,0,, "
        };

        [Fact]
        public void AggregateShouldAggregateYears()
        {
            var aggregator = AggregateAllSampleData();
            var summary = aggregator.GetSummary();

            summary.WeatherData.Count.Should().Be(2);
            
            summary.WeatherData[0].Year.Should().Be(2020);
            summary.WeatherData[0].FirstDate.Should().Be(new DateTime(2020, 10, 1));
            summary.WeatherData[0].LastDate.Should().Be(new DateTime(2020, 11, 1));
            summary.WeatherData[0].TotalRainfall.Should().Be(15.7m);
            summary.WeatherData[0].AverageDailyRainfall.Should().Be(15.7m / 2);
            summary.WeatherData[0].DaysWithNoRainfall.Should().Be(0);
            summary.WeatherData[0].DaysWithRainfall.Should().Be(2);
            summary.WeatherData[0].LongestNumberOfDaysRaining.Should().Be(1);

            summary.WeatherData[1].Year.Should().Be(2021);
            summary.WeatherData[1].FirstDate.Should().Be(new DateTime(2021, 1, 1));
            summary.WeatherData[1].LastDate.Should().Be(new DateTime(2021, 3, 18));
            summary.WeatherData[1].TotalRainfall.Should().Be(48.81m);
            summary.WeatherData[1].AverageDailyRainfall.Should().Be(48.81m / 10);
            summary.WeatherData[1].DaysWithNoRainfall.Should().Be(2);
            summary.WeatherData[1].DaysWithRainfall.Should().Be(8);
            summary.WeatherData[1].LongestNumberOfDaysRaining.Should().Be(4);
        }

        [Fact]
        public void AggregateShouldAggregateMonths()
        {
            var aggregator = AggregateAllSampleData();
            var summary = aggregator.GetSummary();

            summary.WeatherData.Count.Should().Be(2);

            summary.WeatherData[0].MonthlyAggregates.Count.Should().Be(2);
            summary.WeatherData[0].MonthlyAggregates[0].MonthNumber.Should().Be(10);
            summary.WeatherData[0].MonthlyAggregates[1].MonthNumber.Should().Be(11);
            
            summary.WeatherData[1].MonthlyAggregates.Count.Should().Be(3);
            summary.WeatherData[1].MonthlyAggregates[0].MonthNumber.Should().Be(1);
            summary.WeatherData[1].MonthlyAggregates[1].MonthNumber.Should().Be(2);
            summary.WeatherData[1].MonthlyAggregates[2].MonthNumber.Should().Be(3);
        }


        [Fact]
        public void AggregateShouldCalculateMonthlyAverageAndMedian()
        {
            var aggregator = AggregateAllSampleData();
            var summary = aggregator.GetSummary();

            summary.WeatherData[1].MonthlyAggregates[0].AverageDailyRainfall.Should().Be(10.45m);
            summary.WeatherData[1].MonthlyAggregates[0].MedianDailyRainfall.Should().Be(10.45m);
            summary.WeatherData[1].MonthlyAggregates[1].AverageDailyRainfall.Should().Be(3.5m);
            summary.WeatherData[1].MonthlyAggregates[1].MedianDailyRainfall.Should().Be(3.1m);
            summary.WeatherData[1].MonthlyAggregates[2].AverageDailyRainfall.Should().Be(4.872m);
            summary.WeatherData[1].MonthlyAggregates[2].MedianDailyRainfall.Should().Be(2.2m);
        }

        private static WeatherDataAggregator AggregateAllSampleData()
        {
            var aggregator = new WeatherDataAggregator();

            foreach (var line in SampleData)
            {
                BomRainfallData.TryParse(line, out var data);
                aggregator.Aggregate(data);
            }

            return aggregator;
        }
    }
}
