using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Options
{
    public class IntegerCommandOptionTests
    {
        [Theory]
        [InlineData("100", 100)]
        [InlineData("-30", -30)]
        public void TestIntegerOptionParsingFromConstructor(string rawText, long expected)
        {
            IntegerCommandOption option  = new IntegerCommandOption(CommandOptionKind.Date, rawText);

            Assert.Equal(expected, option.GetValue(null));
        }
        
        [Fact]
        public void TestIntegerOptionParsing()
        {
            long value1 = 200;

            IntegerCommandOption option  = new IntegerCommandOption(CommandOptionKind.Date);
            long parsedData;
            bool successful = option.TryParseData("200", out parsedData);

            Assert.True(successful);
            Assert.Equal(value1, parsedData);

            successful = option.SetData("asdf");
            Assert.False(successful);
            Assert.False(option.IsDataValid);

            successful = option.SetData("67");
            Assert.True(successful);
            Assert.True(option.IsDataValid);
        }
    }
}