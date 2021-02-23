using System;

namespace DataConverter.Models
{
    public class BomRainfallData
    {
        private DateTime _date;
        private decimal _rainfallMillimetres;

        public string ProductCode { get; private set; }
        
        public string BranchNumber { get; private set; }
        
        public string Year { get; private set; }
        
        public string Month { get; private set; }
        
        public string Day { get; private set; }
        
        public string RainfallMillimetres { get; private set; }
        
        public string RainfallPeriodDays { get; private set; }
        
        public string QualityControl { get; private set; }

        public static bool TryParse(string input, out BomRainfallData data)
        {
            data = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            var inputSplit = input.Split(',');

            if (inputSplit.Length != 8)
            {
                return false;
            }

            data = new BomRainfallData();

            data.ProductCode = inputSplit[0];
            data.BranchNumber = inputSplit[1];
            data.Year = inputSplit[2];
            data.Month = inputSplit[3];
            data.Day = inputSplit[4];
            data.RainfallMillimetres = inputSplit[5];
            data.RainfallPeriodDays = inputSplit[6];
            data.QualityControl = inputSplit[7];

            return true;
        }

        public bool Validate()
        {
            if (!int.TryParse(Year, out var year) || year <= 0)
            {
                return false;
            }

            if (!int.TryParse(Month, out var month) || month <= 0 || month > 12)
            {
                return false;
            }

            if (!int.TryParse(Day, out var day) || day <= 0)
            {
                return false;
            }

            if (!decimal.TryParse(RainfallMillimetres, out _rainfallMillimetres) || _rainfallMillimetres < 0)
            {
                return false;
            }

            _date = new DateTime(year, month, day);



            return true;
        }

        public DateTime GetDate()
        {
            return _date;
        }

        public decimal GetRainfall()
        {
            return _rainfallMillimetres;
        }
    }
}
