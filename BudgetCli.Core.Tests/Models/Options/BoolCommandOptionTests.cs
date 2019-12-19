using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Options
{
    public class BoolCommandOptionTests
    {
        
        [Theory]
        [InlineData("True", true)]
        [InlineData("true", true)]
        public void TestBoolOptionParsingFromConstructor(string rawText, bool expectedValue)
        {
            BoolCommandOption option = new BoolCommandOption(CommandOptionKind.Date, rawText);

            Assert.Equal(expectedValue, option.GetValue(null));
        }

        [Fact]
        public void TestBoolOptionParsing()
        {
            BoolCommandOption option = new BoolCommandOption(CommandOptionKind.Date);

            bool parsedData;
            bool successful = option.TryParseData("False", out parsedData);

            Assert.True(successful);
            Assert.False(parsedData);

            successful = option.SetData("asdf");
            Assert.False(successful);
            Assert.False(option.IsDataValid);

            successful = option.SetData("True");
            Assert.True(successful);
            Assert.True(option.IsDataValid);
        }
    }
}