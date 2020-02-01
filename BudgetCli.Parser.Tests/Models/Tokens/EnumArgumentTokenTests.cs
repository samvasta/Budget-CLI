using System;
using Xunit;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Enums;
using System.ComponentModel;

namespace BudgetCli.Parser.Tests.Models.Tokens
{
    public class EnumArgumentTokenTests
    {
        public enum FakeEnum
        {
            Value1,
            Value2,
            Value3,

            ValueWithMultipleWords,
            [Description("Custom Description")]
            ValueWithDescription,
        }

        [Fact]
        public void TestPossibleValues()
        {
            EnumArgumentToken<FakeEnum> token = new EnumArgumentToken<FakeEnum>.Builder().Name("name").IsOptional(false).Build();
            
            Assert.Contains("Value 1", token.PossibleValues);
            Assert.Contains("Value 2", token.PossibleValues);
            Assert.Contains("Value 3", token.PossibleValues);
            Assert.Contains("Value With Multiple Words", token.PossibleValues);
            Assert.Contains("Custom Description", token.PossibleValues);
            Assert.DoesNotContain("Value With Description", token.PossibleValues);
        }

        [Theory]
        [InlineData("Value 1", FakeEnum.Value1)]
        [InlineData("Value 2", FakeEnum.Value2)]
        [InlineData("Value 3", FakeEnum.Value3)]
        [InlineData("Value With Multiple Words", FakeEnum.ValueWithMultipleWords)]
        [InlineData("Custom Description", FakeEnum.ValueWithDescription)]
        public void TestParse(string input, FakeEnum expectedEnumValue)
        {
            EnumArgumentToken<FakeEnum> token = new EnumArgumentToken<FakeEnum>.Builder().Name("name").IsOptional(false).Build();
            
            FakeEnum enumValue;
            bool match = EnumArgumentToken<FakeEnum>.TryParse(input, out enumValue);

            Assert.True(match);
            Assert.Equal(expectedEnumValue, enumValue);
        }
    }
}