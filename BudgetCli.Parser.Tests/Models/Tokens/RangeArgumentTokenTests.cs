using System;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Util;
using BudgetCli.Util.Models;
using Xunit;

namespace BudgetCli.Parser.Tests.Models.Tokens
{
    public class RangeArgumentTokenTests
    {

        [Theory]
        [InlineData(new []{"(10,100)"}, 0, 10, 100)]
        [InlineData(new []{"( 10,100)"}, 0, 10, 100)]
        [InlineData(new []{"(10 ,100)"}, 0, 10, 100)]
        [InlineData(new []{"(10, 100)"}, 0, 10, 100)]
        [InlineData(new []{"(10,100 )"}, 0, 10, 100)]
        [InlineData(new []{"( 10 , 100 )"}, 0, 10, 100)]
        public void TryParseIntRangeArg(string[] tokens, int startIdx, int expectedStart, int expectedEnd)
        {
            RangeArgumentToken<int> token = new RangeArgumentToken<int>("arg", false, int.TryParse);

            var result = token.Matches(tokens, startIdx);

            Range<int> range;
            bool argValueExists = result.TryGetArgValue(token, out range);


            Assert.True(result.MatchOutcome == MatchOutcome.Full, "Outcome is not full match");
            Assert.True(argValueExists, "Argument value does not exist");
            Assert.Equal(expectedStart, range.From);
            Assert.Equal(expectedEnd, range.To);
        }

        [Theory]
        [InlineData(new []{"(02-02-2020,12-12-2020)"}, 0, "02-02-2020", "12-12-2020")]
        public void TryParseDateRangeArg(string[] tokens, int startIdx, string expectedStartStr, string expectedEndStr)
        {
            DateTime expectedStart = DateTime.Parse(expectedStartStr);
            DateTime expectedEnd = DateTime.Parse(expectedEndStr);

            RangeArgumentToken<DateTime> token = new RangeArgumentToken<DateTime>("arg", false, DateMatchUtils.TryMatchDate);

            var result = token.Matches(tokens, startIdx);

            Range<DateTime> range;
            bool argValueExists = result.TryGetArgValue(token, out range);


            Assert.True(result.MatchOutcome == MatchOutcome.Full, "Outcome is not full match");
            Assert.True(argValueExists, "Argument value does not exist");
            Assert.Equal(expectedStart, range.From);
            Assert.Equal(expectedEnd, range.To);
        }

        [Theory]
        [InlineData(new []{"(2 days ago, yesterday)"}, 0)]
        [InlineData(new []{"(last january 3, last march 5)"}, 0)]
        [InlineData(new []{"(last monday, last friday)"}, 0)]
        public void TryParseDateRangeArg_2(string[] tokens, int startIdx)
        {
            RangeArgumentToken<DateTime> token = new RangeArgumentToken<DateTime>("arg", false, DateMatchUtils.TryMatchDate);

            var result = token.Matches(tokens, startIdx);

            Range<DateTime> range;
            bool argValueExists = result.TryGetArgValue(token, out range);


            Assert.True(result.MatchOutcome == MatchOutcome.Full, "Outcome is not full match");
            Assert.True(argValueExists, "Argument value does not exist");
        }
    }
}