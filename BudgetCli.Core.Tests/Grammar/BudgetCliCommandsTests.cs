using System;
using System.Linq;
using BudgetCli.Core.Grammar;
using BudgetCli.Data.Enums;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Parsing;
using BudgetCli.Parser.Util;
using Xunit;

namespace BudgetCli.Core.Tests.Grammar
{
    public class BudgetCliCommandsTests
    {
        private bool HasCommand(CommandLibrary library, params VerbToken[] tokens)
        {
            return library.TryGetCommand(TokenUtils.GetMatchText(tokens.Select(x => x.Name.Preferred).ToArray(), 0, tokens.Length), out _);
        }

        private bool HasHelpOption(ICommandRoot commandRoot)
        {
            return commandRoot.Usages.Any(x => x.Tokens.Contains(BudgetCliCommands.OPT_HELP));
        }

        [Fact]
        public void HasHelpCommand()
        {
            var library = BudgetCliCommands.BuildCommandLibrary();
            Assert.True(HasCommand(library, BudgetCliCommands.VERB_HELP));
        }

        [Fact]
        public void HasExitCommand()
        {
            var library = BudgetCliCommands.BuildCommandLibrary();
            Assert.True(HasCommand(library, BudgetCliCommands.VERB_EXIT));
        }

        [Fact]
        public void HasClearCommand()
        {
            var library = BudgetCliCommands.BuildCommandLibrary();
            Assert.True(HasCommand(library, BudgetCliCommands.VERB_CLEAR));
        }

        [Fact]
        public void HasVersionCommand()
        {
            var library = BudgetCliCommands.BuildCommandLibrary();
            Assert.True(HasCommand(library, BudgetCliCommands.VERB_VERSION));
        }

        [Fact]
        public void HasNewAccountCommand()
        {
            var library = BudgetCliCommands.BuildCommandLibrary();
            Assert.True(HasCommand(library, BudgetCliCommands.VERB_NEW, BudgetCliCommands.VERB_ACCOUNT));
        }

        [Fact]
        public void HasRemoveAccountCommand()
        {
            var library = BudgetCliCommands.BuildCommandLibrary();
            Assert.True(HasCommand(library, BudgetCliCommands.VERB_REMOVE, BudgetCliCommands.VERB_ACCOUNT));
        }

        [Fact]
        public void HasSetAccountCommand()
        {
            var library = BudgetCliCommands.BuildCommandLibrary();
            Assert.True(HasCommand(library, BudgetCliCommands.VERB_SET, BudgetCliCommands.VERB_ACCOUNT));
        }

        [Fact]
        public void HasListAccountCommand()
        {
            var library = BudgetCliCommands.BuildCommandLibrary();
            Assert.True(HasCommand(library, BudgetCliCommands.VERB_LIST, BudgetCliCommands.VERB_ACCOUNT));
        }

        [Fact]
        public void HasDetailAccountCommand()
        {
            var library = BudgetCliCommands.BuildCommandLibrary();
            Assert.True(HasCommand(library, BudgetCliCommands.VERB_DETAIL, BudgetCliCommands.VERB_ACCOUNT));
        }

        [Fact]
        public void AllCommandsHaveHelpOption()
        {
            var library = BudgetCliCommands.BuildCommandLibrary();

            foreach(var command in library.GetAllCommands())
            {
                if(command.CommandId == (int)CommandActionKind.Help ||
                   command.CommandId == (int)CommandActionKind.Version ||
                   command.CommandId == (int)CommandActionKind.Clear ||
                   command.CommandId == (int)CommandActionKind.Exit)
                {
                    //System commands don't need help usages
                    continue;   
                }
                string commandStr = string.Join(" ", command.CommonTokens.Select(x => x.Name.Preferred));
                Assert.True(HasHelpOption(command), $"{commandStr} has no help option");
            }
        }
    }
}