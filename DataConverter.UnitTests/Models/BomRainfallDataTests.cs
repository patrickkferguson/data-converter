using DataConverter.Models;
using FluentAssertions;
using Xunit;

namespace DataConverter.UnitTests.Models
{
    public class BomRainfallDataTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("some junk")]
        [InlineData("not,enough,fields,not,enough,fields,not")]
        public void TryParseWithInvalidInputShouldReturnFalse(string input)
        {
            var result = BomRainfallData.TryParse(input, out var data);

            result.Should().BeFalse();
            data.Should().BeNull();
        }

        [Fact]
        public void TryParseWithValidInputShouldReturnData()
        {
            var input = "field 1,field 2,field 3,field 4,field 5,field 6,field 7,field 8";

            var result = BomRainfallData.TryParse(input, out var data);

            result.Should().BeTrue();
            data.Should().NotBeNull();

            data.ProductCode.Should().Be("field 1");
            data.BranchNumber.Should().Be("field 2");
            data.Year.Should().Be("field 3");
            data.Month.Should().Be("field 4");
            data.Day.Should().Be("field 5");
            data.RainfallMillimetres.Should().Be("field 6");
            data.RainfallPeriodDays.Should().Be("field 7");
            data.QualityControl.Should().Be("field 8");
        }

        [Theory]
        [InlineData(",,,,,,,")]
        [InlineData(",,X,,,,,")]
        [InlineData(",,0,,,,,")]
        [InlineData(",,1,,,,,")]
        [InlineData(",,1,X,,,,")]
        [InlineData(",,1,0,,,,")]
        [InlineData(",,1,13,,,,")]
        [InlineData(",,1,1,,,,")]
        [InlineData(",,1,1,X,,,")]
        [InlineData(",,1,1,0,,,")]
        [InlineData(",,1,1,1,,,")]
        [InlineData(",,1,1,1,X,,")]
        [InlineData(",,1,1,1,-1,,")]
        public void ValidateWithInvalidDataShouldReturnFalse(string input)
        {
            var parseResult = BomRainfallData.TryParse(input, out var data);

            parseResult.Should().BeTrue();
            data.Should().NotBeNull();

            var validateResult = data.Validate();

            validateResult.Should().BeFalse();
        }

        [Theory]
        [InlineData(",,1,1,1,1,,")]
        public void ValidateWithValidDataShouldReturnTrue(string input)
        {
            var parseResult = BomRainfallData.TryParse(input, out var data);

            parseResult.Should().BeTrue();
            data.Should().NotBeNull();

            var validateResult = data.Validate();

            validateResult.Should().BeTrue();
        }
    }
}
