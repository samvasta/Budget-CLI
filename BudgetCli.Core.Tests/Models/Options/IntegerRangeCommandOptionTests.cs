using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using BudgetCli.Util.Models;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Options
{
    public class IntegerRangeCommandOptionTests
    {
        [Theory]
        [InlineData("[1234, 6789]", 1234, 6789, true, true)]
        [InlineData("[1234, 6789)", 1234, 6789, true, false)]
        [InlineData("(1234, 6789]", 1234, 6789, false, true)]
        [InlineData("1234", 1234, 1234, true, true)]
        public void TestIntegerRangeOptionParsingFromConstructor(string rawText, long expectedIntegerRangeStart, long expectedIntegerRangeEnd, bool isFromInclusive, bool isToInclusive)
        {

            Range<long> expected = new Range<long>(expectedIntegerRangeStart, expectedIntegerRangeEnd, isFromInclusive, isToInclusive);


            IntegerRangeCommandOption option  = new IntegerRangeCommandOption(CommandOptionKind.Date, rawText);

            Assert.Equal(expected, option.GetValue(null));
        }
        
        [Fact]
        public void TestIntegerRangeOptionParsing()
        {
            long from = 123456;
            long to = 987654;
            Range<long> range = new Range<long>(from, to);

            IntegerRangeCommandOption option  = new IntegerRangeCommandOption(CommandOptionKind.Date);
            Range<long> parsedData;
            bool successful = option.TryParseData("[123456, 987654]", out parsedData);

            Assert.True(successful);
            Assert.Equal(range, parsedData);

            successful = option.SetData("asdf");
            Assert.False(successful);
            Assert.False(option.IsDataValid);

            successful = option.SetData("-6634");
            Assert.True(successful);
            Assert.True(option.IsDataValid);
        }
    }
}