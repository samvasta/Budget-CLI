using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using BudgetCli.Util.Models;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Options
{
    public class MoneyRangeCommandOptionTests
    {
        [Theory]
        [InlineData("[$1234, 6789]", 1234, 6789, true, true)]
        [InlineData("[1234.78, 6789)", 1234.78, 6789, true, false)]
        [InlineData("(1234, $6789]", 1234, 6789, false, true)]
        [InlineData("$1234.67", 1234.67, 1234.67, true, true)]
        public void TestMoneyRangeOptionParsingFromConstructor(string rawText, double expectedMoneyRangeStart, double expectedMoneyRangeEnd, bool isFromInclusive, bool isToInclusive)
        {
            Range<Money> expected = new Range<Money>(expectedMoneyRangeStart, expectedMoneyRangeEnd, isFromInclusive, isToInclusive);


            MoneyRangeCommandOption option  = new MoneyRangeCommandOption(CommandOptionKind.Date, rawText);

            Assert.Equal(expected, option.GetValue(null));
        }
        
        [Fact]
        public void TestMoneyRangeOptionParsing()
        {
            Money from = 123456.00;
            Money to = 987654.99;
            Range<Money> range = new Range<Money>(from, to);

            MoneyRangeCommandOption option  = new MoneyRangeCommandOption(CommandOptionKind.Date);
            Range<Money> parsedData;
            bool successful = option.TryParseData("[123456.00, 987654.99]", out parsedData);

            Assert.True(successful);
            Assert.Equal(range, parsedData);

            successful = option.SetData("asdf");
            Assert.False(successful);
            Assert.False(option.IsDataValid);

            successful = option.SetData("-6634.65");
            Assert.True(successful);
            Assert.True(option.IsDataValid);
        }
    }
}