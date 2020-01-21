using System;
using BudgetCli.Parser.Models;
using BudgetCli.Parser.Models.Tokens;
using Xunit;

namespace BudgetCli.Parser.Tests.Models
{
    public class CommandRootTests
    {
        private CommandUsage BuildUsage()
        {
            return new CommandUsage.Builder()
                                    .Description("description")
                                    .WithToken(new VerbToken(new Name("token1")))
                                    .WithToken(new VerbToken(new Name("token2")))
                                    .WithExample("token1 token2")
                                    .Build();
        }

        [Fact]
        public void TestNullName()
        {
            Assert.Throws<ArgumentNullException>(() => 
            {
                new CommandRoot.Builder().Description("description").WithUsage(BuildUsage()).Build();
            });
        }

        [Fact]
        public void TestNoUsages()
        {
            Assert.Throws<ArgumentException>(() => 
            {
                new CommandRoot.Builder().Name("command").Description("description").Build();
            });
        }

        [Fact]
        public void TestBuild()
        {
            var usage = BuildUsage();
            var command = new CommandRoot.Builder().Name("command").Description("description").WithUsage(usage).Build();

            Assert.True(command.CommandName.Equals("command"));
            Assert.Equal("description", command.Description);
            Assert.Equal(1, command.Usages.Length);
            Assert.Same(usage, command.Usages[0]);
        }
    }
}