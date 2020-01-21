using System;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models;
using BudgetCli.Parser.Models.Tokens;
using Xunit;

namespace BudgetCli.Parser.Tests.Models
{
    public class CommandUsageTests
    {
        [Fact]
        public void TestNoExamples()
        {
            new CommandUsage.Builder().Description("description").WithToken(new VerbToken(new Name("token1"))).Build();

            //Should not throw an exception, passes automatically
        }

        [Fact]
        public void TestNoTokens()
        {
            Assert.Throws<ArgumentException>(() => 
            {
                new CommandUsage.Builder().Description("description").Build();
            });
        }

        [Fact]
        public void TestBuild()
        {
            ICommandToken token = new VerbToken(new Name("token"));
            var usage = new CommandUsage.Builder().Description("description").WithExample("example1").WithToken(token).Build();

            Assert.Equal("description", usage.Description);
            Assert.Equal(1, usage.Tokens.Length);
            Assert.Same(token, usage.Tokens[0]);
            Assert.Equal(1, usage.Examples.Length);
            Assert.Equal("example1", usage.Examples[0]);
        }
    }
}