using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Options
{
    public class DecimalCommandOptionTests
    {
        [Theory]
        [InlineData("12.34", "12.34")]
        public void TestDecimalOptionParsingFromConstructor(string rawText, string expectedDateAsStr)
        {
            decimal expected = decimal.Parse(expectedDateAsStr);
            DecimalCommandOption option  = new DecimalCommandOption(CommandOptionKind.Date, rawText);

            Assert.Equal(expected, option.GetValue(null));
        }
        
        [Fact]
        public void TestDecimalOptionParsing()
        {
            decimal value1 = 12.34m;

            DecimalCommandOption option  = new DecimalCommandOption(CommandOptionKind.Date);
            decimal parsedData;
            bool successful = option.TryParseData("12.34", out parsedData);

            Assert.True(successful);
            Assert.Equal(value1, parsedData);

            successful = option.SetData("asdf");
            Assert.False(successful);
            Assert.False(option.IsDataValid);

            successful = option.SetData("34.567");
            Assert.True(successful);
            Assert.True(option.IsDataValid);
        }
    }
}