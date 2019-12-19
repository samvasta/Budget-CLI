using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using BudgetCli.Util.Models;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Options
{
    public class MoneyCommandOptionTests
    {
        [Theory]
        [InlineData("$100", 100)]
        [InlineData("-30", -30)]
        [InlineData("-30.4", -30.4)]
        [InlineData("$-30.4", -30.4)]
        public void TestEnumOptionParsingFromConstructor(string rawText, double expectedAsDouble)
        {
            Money expected = expectedAsDouble;
            MoneyCommandOption option  = new MoneyCommandOption(CommandOptionKind.Date, rawText);

            Assert.Equal(expected, option.GetValue(null));
        }
        
        [Fact]
        public void TestEnumOptionParsing()
        {
            Money value1 = 200.45;

            MoneyCommandOption option  = new MoneyCommandOption(CommandOptionKind.Date);
            Money parsedData;
            bool successful = option.TryParseData("200.45", out parsedData);

            Assert.True(successful);
            Assert.Equal(value1, parsedData);

            successful = option.SetData("asdf");
            Assert.False(successful);
            Assert.False(option.IsDataValid);

            successful = option.SetData("67.99");
            Assert.True(successful);
            Assert.True(option.IsDataValid);
        }
    }
}