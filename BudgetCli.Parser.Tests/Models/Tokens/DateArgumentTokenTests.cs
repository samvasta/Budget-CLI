using System;
using Xunit;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Enums;

namespace BudgetCli.Parser.Tests.Models.Tokens
{
    public class DateArgumentTokenTests
    {
        [Theory]
        [InlineData(new []{"sdf", "02-02-2020"}, 1, "02/02/2020")]
        public void TryParseExplicitDate(string[] tokens, int startIdx, string expectedDateStr)
        {
            DateTime expected = DateTime.Parse(expectedDateStr);

            DateArgumentToken token = new DateArgumentToken("arg", false);

            var result = token.Matches(tokens, startIdx);

            DateTime parsedDate;
            bool argValueExists = result.TryGetArgValue(token, out parsedDate);


            Assert.True(result.MatchOutcome == MatchOutcome.Full);
            Assert.True(argValueExists);
            Assert.Equal(expected, parsedDate);
        }

        [Fact]
        public void TryParseYesterday()
        {
            DateTime currentDate = DateTime.Parse("01/02/2020");
            DateTime expected = DateTime.Parse("01/01/2020");

            DateArgumentToken token = new DateArgumentToken("arg", false, currentDate);

            var result = token.Matches(new []{"yesterday"}, 0);

            DateTime parsedDate;
            bool argValueExists = result.TryGetArgValue(token, out parsedDate);


            Assert.True(result.MatchOutcome == MatchOutcome.Full);
            Assert.True(argValueExists);
            Assert.Equal(expected, parsedDate);
        }

        [Theory]
        [InlineData(new []{"last", "tuesday"}, 0, "01/01/2020", "12/24/2019")]
        [InlineData(new []{"last", "saturday"}, 0, "01/01/2020", "12/28/2019")]
        [InlineData(new []{"last", "sunday"}, 0, "01/01/2020", "12/22/2019")]
        public void TryParseLastDayOfWeek(string[] tokens, int startIdx, string currentDateStr, string expectedDateStr)
        {
            DateTime expected = DateTime.Parse(expectedDateStr);
            DateTime currentDate = DateTime.Parse(currentDateStr);

            DateArgumentToken token = new DateArgumentToken("arg", false, currentDate);

            var result = token.Matches(tokens, startIdx);

            DateTime parsedDate;
            bool argValueExists = result.TryGetArgValue(token, out parsedDate);


            Assert.True(result.MatchOutcome == MatchOutcome.Full);
            Assert.True(argValueExists);
            Assert.Equal(expected, parsedDate);
        }

        [Theory]
        [InlineData(new []{"last", "november", "23"}, 0, "04/01/2020", "11/23/2019")]
        [InlineData(new []{"last", "January", "1"}, 0, "04/01/2020", "01/01/2019")]
        [InlineData(new []{"last", "April", "1"}, 0, "04/01/2020", "04/01/2019")]
        public void TryParseLastDayOfMonth(string[] tokens, int startIdx, string currentDateStr, string expectedDateStr)
        {
            DateTime expected = DateTime.Parse(expectedDateStr);
            DateTime currentDate = DateTime.Parse(currentDateStr);

            DateArgumentToken token = new DateArgumentToken("arg", false, currentDate);

            var result = token.Matches(tokens, startIdx);

            DateTime parsedDate;
            bool argValueExists = result.TryGetArgValue(token, out parsedDate);


            Assert.True(result.MatchOutcome == MatchOutcome.Full);
            Assert.True(argValueExists);
            Assert.Equal(expected, parsedDate);
        }

        [Theory]
        [InlineData(new []{"1", "year", "ago"}, 0, "04/01/2020", "04/01/2019")]
        [InlineData(new []{"1", "month", "ago"}, 0, "04/01/2020", "03/01/2020")]
        [InlineData(new []{"1", "week", "ago"}, 0, "04/01/2020", "03/25/2020")]
        [InlineData(new []{"1", "day", "ago"}, 0, "04/01/2020", "03/31/2020")]
        public void TryParseRelativeDate(string[] tokens, int startIdx, string currentDateStr, string expectedDateStr)
        {
            DateTime expected = DateTime.Parse(expectedDateStr);
            DateTime currentDate = DateTime.Parse(currentDateStr);

            DateArgumentToken token = new DateArgumentToken("arg", false, currentDate);

            var result = token.Matches(tokens, startIdx);

            DateTime parsedDate;
            bool argValueExists = result.TryGetArgValue(token, out parsedDate);


            Assert.True(result.MatchOutcome == MatchOutcome.Full);
            Assert.True(argValueExists);
            Assert.Equal(expected, parsedDate);
        }

        [Theory]
        [InlineData(new []{"test"}, 0)]
        [InlineData(new []{"1", "shmonth", "ago"}, 0)]
        [InlineData(new []{"1", "week"}, 0)]
        [InlineData(new []{"not_a_number", "week", "ago"}, 0)]
        [InlineData(new []{"1", "week", "Shmago"}, 0)]
        [InlineData(new []{"last", "Shmonday"}, 0)]
        [InlineData(new []{"last", "July", "not_a_number"}, 0)]
        [InlineData(new []{"last", "Shmuly", "3"}, 0)]
        [InlineData(new []{"not", "a", "date"}, 0)]
        public void TryParseDateFails(string[] tokens, int startIdx)
        {
            DateArgumentToken token = new DateArgumentToken("arg", false);

            var result = token.Matches(tokens, startIdx);

            DateTime parsedDate;
            bool argValueExists = result.TryGetArgValue(token, out parsedDate);

            Assert.Equal(TokenMatchResult.None, result);
            Assert.False(argValueExists);
        }
    }
}