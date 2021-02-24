using System;

namespace DataConverter.Models
{
    public class RainfallReading
    {
        public RainfallReading(DateTime date, decimal rainfall)
        {
            Date = date;
            Rainfall = rainfall;
        }

        public DateTime Date { get; }

        public decimal Rainfall { get; }
    }
}
