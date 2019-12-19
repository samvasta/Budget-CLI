using System;
using Xunit;
using BudgetCli.Core.Tests.TestHarness;
using BudgetCli.Data.Enums;

namespace BudgetCli.Core.Tests.Models.Options
{
    public class CommandOptionBaseTest
    {
        [Fact]
        public void TestCommandOptionBaseConstructor()
        {
            FakeCommandOptionBase option = new FakeCommandOptionBase(CommandOptionKind.Date);

            Assert.Equal(CommandOptionKind.Date, option.OptionKind);
            Assert.False(option.IsDataValid);
        }

        [Fact]
        public void TestCommandOptionBaseConstructor2()
        {
            FakeCommandOptionBase option = new FakeCommandOptionBase(CommandOptionKind.Date, "12.34");

            Assert.Equal(CommandOptionKind.Date, option.OptionKind);
            Assert.True(option.IsDataValid);
            Assert.Equal(12.34, option.GetValue(0));
        }
        
        [Fact]
        public void TestCommandOptionBaseConstructor3()
        {
            FakeCommandOptionBase option = new FakeCommandOptionBase(CommandOptionKind.Date, 12.34);

            Assert.Equal(CommandOptionKind.Date, option.OptionKind);
            Assert.True(option.IsDataValid);
            Assert.Equal(12.34, option.GetValue(0));
        }
        
        [Fact]
        public void TestCommandOptionBase_GetValue()
        {
            FakeCommandOptionBase option = new FakeCommandOptionBase(CommandOptionKind.Date, 12.34);

            Assert.Equal(12.34, option.GetValue(0));
            Assert.Equal(12.34, option.GetValue(0.0d));
            Assert.Equal(12.34, option.GetValue(0.0m));
            Assert.Equal(12.34, option.GetValue("string"));
        }
        
        
        [Fact]
        public void TestCommandOptionBase_GetValue2()
        {
            FakeCommandOptionBase option = new FakeCommandOptionBase(CommandOptionKind.Date);

            Assert.Equal(0.0d, option.GetValue(0.0d));
            Assert.Equal("string", option.GetValue("string"));
            Assert.Equal(0.0m, option.GetValue(0.0m));
        }
    }
}
