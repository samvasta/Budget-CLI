using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Options
{
    public class EnumCommandOptionTests
    {
        public enum FakeEnum
        {
            FirstItem,
            SecondItem,
            ThirdItem
        }

        [Theory]
        [InlineData("FirstItem", FakeEnum.FirstItem)]
        [InlineData("SecondItem", FakeEnum.SecondItem)]
        public void TestEnumOptionParsingFromConstructor(string rawText, FakeEnum expected)
        {
            EnumCommandOption<FakeEnum> option  = new EnumCommandOption<FakeEnum>(CommandOptionKind.Date, rawText);

            Assert.Equal(expected, option.GetValue(null));
        }
        
        [Fact]
        public void TestEnumOptionParsing()
        {
            FakeEnum value1 = FakeEnum.SecondItem;

            EnumCommandOption<FakeEnum> option  = new EnumCommandOption<FakeEnum>(CommandOptionKind.Date);
            FakeEnum parsedData;
            bool successful = option.TryParseData("SecondItem", out parsedData);

            Assert.True(successful);
            Assert.Equal(value1, parsedData);

            successful = option.SetData("asdf");
            Assert.False(successful);
            Assert.False(option.IsDataValid);

            successful = option.SetData("ThirdItem");
            Assert.True(successful);
            Assert.True(option.IsDataValid);
        }
    }
}