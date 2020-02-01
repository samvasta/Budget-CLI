using System;
using BudgetCli.Parser.Models;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Util.Models;
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
            Assert.Throws<ArgumentException>(() => 
            {
                new CommandRoot.Builder().Description("description").WithUsage(BuildUsage()).Build();
            });
        }

        [Fact]
        public void TestBuild()
        {
            var usage = BuildUsage();
            var command = new CommandRoot.Builder().WithToken("command").Description("description").WithUsage(usage).Build();

            Assert.Equal(1, command.CommonTokens.Length);
            Assert.True(command.CommonTokens[0].Name.Equals("command"));
            Assert.Equal("description", command.Description);
            Assert.Equal(1, command.Usages.Length);
            Assert.Same(usage, command.Usages[0]);
        }
    }
}