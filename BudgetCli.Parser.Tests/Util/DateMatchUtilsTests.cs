using System;
using BudgetCli.Parser.Util;
using Xunit;

namespace BudgetCli.Parser.Tests.Util
{
    public class DateMatchUtilsTests
    {
        [Theory]
        [InlineData("02-02-2020", "02/02/2020")]
        public void TryParseExplicitDate(string inputStr, string expectedDateStr)
        {
            DateTime expected = DateTime.Parse(expectedDateStr);

            bool success = DateMatchUtils.TryMatchDate(inputStr, out DateTime actual);

            Assert.True(success);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryParseYesterday()
        {
            DateTime currentDate = DateTime.Parse("01/02/2020");
            DateTime expected = DateTime.Parse("01/01/2020");

            bool success = DateMatchUtils.TryMatchDate("yesterday", currentDate, out DateTime actual);

            Assert.True(success);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("last tuesday", "01/01/2020", "12/24/2019")]
        [InlineData("last saturday", "01/01/2020", "12/28/2019")]
        [InlineData("last sunday", "01/01/2020", "12/22/2019")]
        public void TryParseLastDayOfWeek(string inputStr, string currentDateStr, string expectedDateStr)
        {
            DateTime expected = DateTime.Parse(expectedDateStr);
            DateTime currentDate = DateTime.Parse(currentDateStr);

            bool success = DateMatchUtils.TryMatchDate(inputStr, currentDate, out DateTime actual);

            Assert.True(success);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("last november 23", "04/01/2020", "11/23/2019")]
        [InlineData("last January 1", "04/01/2020", "01/01/2019")]
        [InlineData("last April 1", "04/01/2020", "04/01/2019")]
        public void TryParseLastDayOfMonth(string inputStr, string currentDateStr, string expectedDateStr)
        {
            DateTime expected = DateTime.Parse(expectedDateStr);
            DateTime currentDate = DateTime.Parse(currentDateStr);

            bool success = DateMatchUtils.TryMatchDate(inputStr, currentDate, out DateTime actual);

            Assert.True(success);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("1 year ago", "04/01/2020", "04/01/2019")]
        [InlineData("1 month ago", "04/01/2020", "03/01/2020")]
        [InlineData("1 week ago", "04/01/2020", "03/25/2020")]
        [InlineData("1 day ago", "04/01/2020", "03/31/2020")]
        public void TryParseRelativeDate(string inputStr, string currentDateStr, string expectedDateStr)
        {
            DateTime expected = DateTime.Parse(expectedDateStr);
            DateTime currentDate = DateTime.Parse(currentDateStr);

            bool success = DateMatchUtils.TryMatchDate(inputStr, currentDate, out DateTime actual);

            Assert.True(success);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("1 shmonth ago")]
        [InlineData("1 week")]
        [InlineData("not_a_number week ago")]
        [InlineData("1 week Shmago")]
        [InlineData("last Shmonday")]
        [InlineData("last July not_a_number")]
        [InlineData("last Shmuly 3")]
        [InlineData("not a date")]
        public void TryParseDateFails(string inputStr)
        {
            bool success = DateMatchUtils.TryMatchDate(inputStr, out DateTime date);
            Assert.False(success);
        }
    }
}