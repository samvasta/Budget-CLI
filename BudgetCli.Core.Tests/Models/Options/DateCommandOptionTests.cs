using System;
using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Options
{
    public class DateCommandOptionTests
    {
        [Theory]
        [InlineData("2020-04-18", "2020-04-18")]
        [InlineData("2020/04/18", "2020-04-18")]
        [InlineData("2020/4/18", "2020-04-18")]
        [InlineData("04/18/2020", "2020-04-18")]
        [InlineData("4/18/2020", "2020-04-18")]
        [InlineData("April 18, 2020", "2020-04-18")]
        public void TestDateOptionParsingFromConstructor(string rawText, string expectedDateAsStr)
        {
            DateTime expected = DateTime.Parse(expectedDateAsStr);
            DateCommandOption option  = new DateCommandOption(CommandOptionKind.Date, rawText);

            Assert.Equal(expected, option.GetValue(null));
        }
        
        [Fact]
        public void TestDateOptionParsing()
        {
            DateTime time1 = new DateTime(2020, 4, 18);

            DateCommandOption option  = new DateCommandOption(CommandOptionKind.Date);
            DateTime parsedData;
            bool successful = option.TryParseData("2020/4/18", out parsedData);

            Assert.True(successful);
            Assert.Equal(time1, parsedData);

            successful = option.SetData("asdf");
            Assert.False(successful);
            Assert.False(option.IsDataValid);

            successful = option.SetData("2020/4/18");
            Assert.True(successful);
            Assert.True(option.IsDataValid);
        }
        
    }
}