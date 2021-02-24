using System;
using DataConverter.Models;
using FluentAssertions;
using Xunit;

namespace DataConverter.UnitTests.Models
{
    public class WeatherDataForMonthTests
    {
        [Theory]
        [InlineData("2021-01-01", "January")]
        [InlineData("2021-02-01", "February")]
        [InlineData("2021-03-01", "March")]
        [InlineData("2021-04-01", "April")]
        [InlineData("2021-05-01", "May")]
        [InlineData("2021-06-01", "June")]
        [InlineData("2021-07-01", "July")]
        [InlineData("2021-08-01", "August")]
        [InlineData("2021-09-01", "September")]
        [InlineData("2021-10-01", "October")]
        [InlineData("2021-11-01", "November")]
        [InlineData("2021-12-01", "December")]
        public void FromDateSetsMonthName(string date, string expectedMonthName)
        {
            var data = WeatherDataForMonth.FromDate(DateTime.Parse(date));

            data.MonthName.Should().Be(expectedMonthName);
        }
    }
}
