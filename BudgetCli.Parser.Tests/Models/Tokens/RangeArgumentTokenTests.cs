using System;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Parsing;
using BudgetCli.Parser.Util;
using BudgetCli.Util.Models;
using Xunit;

namespace BudgetCli.Parser.Tests.Models.Tokens
{
    public class RangeArgumentTokenTests
    {
        [Theory]
        [InlineData("( 3 , 6")] //No close paren
        [InlineData("( 3  6 )")] //No comma
        [InlineData("( , 6 ]")] //No first arg
        [InlineData("( 3 , ]")] //No second arg
        public void TryParseRange_Failure(string text)
        {
            string[] tokens = CommandParser.Tokenize(text);
            RangeArgumentToken<int> token = new RangeArgumentToken<int>("arg", false, int.TryParse);

            var result = token.Matches(tokens, 0);

            Range<int> range;
            bool argValueExists = result.TryGetArgValue(token, out range);


            Assert.True(result.MatchOutcome == MatchOutcome.None, "Outcome is not full match. Test Case = " + text);
            Assert.False(argValueExists, "Argument value should not exist but does. Test Case = " + text);
        }

        [Theory]
        [InlineData("(10,100)", 10, 100)]
        [InlineData("( 10,100)", 10, 100)]
        [InlineData("(10 ,100)", 10, 100)]
        [InlineData("(10, 100)", 10, 100)]
        [InlineData("(10,100 )", 10, 100)]
        [InlineData("( 10 , 100 )", 10, 100)]
        public void TryParseIntRangeArg(string text, int expectedStart, int expectedEnd)
        {
            string[] tokens = CommandParser.Tokenize(text);
            RangeArgumentToken<int> token = new RangeArgumentToken<int>("arg", false, int.TryParse);

            var result = token.Matches(tokens, 0);

            Range<int> range;
            bool argValueExists = result.TryGetArgValue(token, out range);


            Assert.True(result.MatchOutcome == MatchOutcome.Full, "Outcome is not full match");
            Assert.True(argValueExists, "Argument value does not exist");
            Assert.Equal(expectedStart, range.From);
            Assert.Equal(expectedEnd, range.To);
        }

        [Theory]
        [InlineData(new []{"(02-02-2020,12-12-2020)"}, "02-02-2020", "12-12-2020")]
        public void TryParseDateRangeArg(string[] tokens, string expectedStartStr, string expectedEndStr)
        {
            DateTime expectedStart = DateTime.Parse(expectedStartStr);
            DateTime expectedEnd = DateTime.Parse(expectedEndStr);

            RangeArgumentToken<DateTime> token = new RangeArgumentToken<DateTime>("arg", false, DateMatchUtils.TryMatchDate);

            var result = token.Matches(tokens, 0);

            Range<DateTime> range;
            bool argValueExists = result.TryGetArgValue(token, out range);


            Assert.True(result.MatchOutcome == MatchOutcome.Full, "Outcome is not full match");
            Assert.True(argValueExists, "Argument value does not exist");
            Assert.Equal(expectedStart, range.From);
            Assert.Equal(expectedEnd, range.To);
        }

        [Theory]
        [InlineData("(2 days ago, yesterday)")]
        [InlineData("(last january 3, last march 5)")]
        [InlineData("(last monday, last friday)")]
        public void TryParseDateRangeArg_2(string text)
        {
            string[] tokens = CommandParser.Tokenize(text);
            RangeArgumentToken<DateTime> token = new RangeArgumentToken<DateTime>("arg", false, DateMatchUtils.TryMatchDate);

            var result = token.Matches(tokens, 0);

            Range<DateTime> range;
            bool argValueExists = result.TryGetArgValue(token, out range);


            Assert.True(result.MatchOutcome == MatchOutcome.Full, "Outcome is not full match");
            Assert.True(argValueExists, "Argument value does not exist");
        }
    }
}