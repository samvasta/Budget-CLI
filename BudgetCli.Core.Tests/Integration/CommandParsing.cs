using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using BudgetCli.Core.Grammar;
using BudgetCli.Parser.Parsing;
using BudgetCli.Parser.Models;

namespace BudgetCli.Core.Tests.Integration
{
    public class CommandParsing
    {
        [Theory]
        [InlineData("new account")]
        [InlineData("n a")]
        [InlineData("n accounts")]
        [InlineData("new a")]
        public void ParseNewAccount(string newAccountStr)
        {
            string fullInput = newAccountStr + " \"account name\"";

            CommandLibrary library = BudgetCliCommands.BuildCommandLibrary();

            CommandUsageMatchData matchData = library.Parse(fullInput);

            Assert.True(matchData.IsSuccessful);
        }
    }
}