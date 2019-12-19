using System;
using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using BudgetCli.Util.Models;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Options
{
    public class DateRangeCommandOptionTests
    {
        [Theory]
        [InlineData("[2020-04-18, 2021-11-01]", "2020-04-18", "2021-11-01", true, true)]
        [InlineData("[2020/04/18, 2021/11/01]", "2020-04-18", "2021-11-01", true, true)]
        [InlineData("[04/18/2020, 2021-11-01]", "2020-04-18", "2021-11-01", true, true)]
        [InlineData("[4/18/2020, 2021-11-01]", "2020-04-18", "2021-11-01", true, true)]
        [InlineData("[April 18 2020, 2021-11-01]", "2020-04-18", "2021-11-01", true, true)]
        [InlineData("April 18 2020", "2020-04-18", "2020-04-18", true, true)]
        public void TestDateRangeOptionParsingFromConstructor(string rawText, string expectedDateRangeStartAsStr, string expectedDateRangeEndAsStr, bool isFromInclusive, bool isToInclusive)
        {
            DateTime start = DateTime.MinValue;
            if(!String.IsNullOrEmpty(expectedDateRangeStartAsStr))
            {
                start = DateTime.Parse(expectedDateRangeStartAsStr);
            }
            DateTime end = DateTime.MaxValue;
            if(!String.IsNullOrEmpty(expectedDateRangeEndAsStr))
            {
                end = DateTime.Parse(expectedDateRangeEndAsStr);
            }

            Range<DateTime> expected = new Range<DateTime>(start, end, isFromInclusive, isToInclusive);


            DateRangeCommandOption option  = new DateRangeCommandOption(CommandOptionKind.Date, rawText);

            Assert.Equal(expected, option.GetValue(null));
        }
        
        [Fact]
        public void TestDateRangeOptionParsing()
        {
            DateTime from = new DateTime(2020, 4, 18);
            DateTime to = new DateTime(2022, 8, 3);
            Range<DateTime> range = new Range<DateTime>(from, to);

            DateRangeCommandOption option  = new DateRangeCommandOption(CommandOptionKind.Date);
            Range<DateTime> parsedData;
            bool successful = option.TryParseData("[2020/4/18,2022/8/3]", out parsedData);

            Assert.True(successful);
            Assert.Equal(range, parsedData);

            successful = option.SetData("asdf");
            Assert.False(successful);
            Assert.False(option.IsDataValid);

            successful = option.SetData("2020/4/18");
            Assert.True(successful);
            Assert.True(option.IsDataValid);
        }
    }
}