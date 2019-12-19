using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Options
{
    public class StringCommandOptionTests
    {
        [Theory]
        [InlineData("100", "100")]
        [InlineData("-30", "-30")]
        [InlineData("asdf", "asdf")]
        public void TestStringOptionParsingFromConstructor(string rawText, string expected)
        {
            StringCommandOption option  = new StringCommandOption(CommandOptionKind.Date, rawText);

            Assert.Equal(expected, option.GetValue(null));
        }
        
        [Fact]
        public void TestStringOptionParsing()
        {
            string value1 = "test string";

            StringCommandOption option  = new StringCommandOption(CommandOptionKind.Date);
            string parsedData;
            bool successful = option.TryParseData("test string", out parsedData);

            Assert.True(successful);
            Assert.Equal(value1, parsedData);

            successful = option.SetData("asdf");
            Assert.True(successful);
            Assert.True(option.IsDataValid);
        }
    }
}