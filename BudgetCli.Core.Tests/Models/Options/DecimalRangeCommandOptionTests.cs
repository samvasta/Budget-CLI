using System;
using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using BudgetCli.Util.Models;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Options
{
    public class DecimalRangeCommandOptionTests
    {
        [Theory]
        [InlineData("[12.34, 67.89]", "12.34", "67.89", true, true)]
        [InlineData("[12.34, 67.89)", "12.34", "67.89", true, false)]
        [InlineData("(12.34, 67.89]", "12.34", "67.89", false, true)]
        [InlineData("12.34", "12.34", "12.34", true, true)]
        public void TestDecimalRangeOptionParsingFromConstructor(string rawText, string expectedDecimalRangeStartAsStr, string expectedDecimalRangeEndAsStr, bool isFromInclusive, bool isToInclusive)
        {
            decimal start = decimal.Parse(expectedDecimalRangeStartAsStr);
            decimal end = decimal.Parse(expectedDecimalRangeEndAsStr);

            Range<decimal> expected = new Range<decimal>(start, end, isFromInclusive, isToInclusive);


            DecimalRangeCommandOption option  = new DecimalRangeCommandOption(CommandOptionKind.Date, rawText);

            Assert.Equal(expected, option.GetValue(null));
        }
        
        [Fact]
        public void TestDecimalRangeOptionParsing()
        {
            decimal from = 12.34m;
            decimal to = 67.89m;
            Range<decimal> range = new Range<decimal>(from, to);

            DecimalRangeCommandOption option  = new DecimalRangeCommandOption(CommandOptionKind.Date);
            Range<decimal> parsedData;
            bool successful = option.TryParseData("[12.34, 67.89]", out parsedData);

            Assert.True(successful);
            Assert.Equal(range, parsedData);

            successful = option.SetData("asdf");
            Assert.False(successful);
            Assert.False(option.IsDataValid);

            successful = option.SetData("44.44");
            Assert.True(successful);
            Assert.True(option.IsDataValid);
        }
    }
}