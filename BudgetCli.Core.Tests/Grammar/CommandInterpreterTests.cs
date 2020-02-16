using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using BudgetCli.Core.Grammar;
using BudgetCli.Parser.Parsing;
using BudgetCli.Parser.Models;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models.Tokens;
using Moq;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Util;
using BudgetCli.Data.Tests.TestHarness;
using BudgetCli.Util.Logging;
using BudgetCli.Core.Models.Commands;
using BudgetCli.Core.Models.Commands.Accounts;

namespace BudgetCli.Core.Tests.Grammar
{
    public class CommandInterpreterTests
    {
        [Fact]
        public void AddAccountCommand()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();

                ICommandRoot root = BudgetCliCommands.CMD_NEW_ACCOUNT;
                ICommandUsage usage = root.Usages.First(x => !x.IsHelp);

                CommandInterpreter interpreter = new CommandInterpreter(SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object), BudgetCliCommands.BuildCommandLibrary());

                ICommandAction action;
                bool success = interpreter.TryParseCommand("new account \"name\"", out action);

                Assert.True(success);
                Assert.IsType<AddAccountCommand>(action);

                AddAccountCommand addAccount = (AddAccountCommand)action;
                Assert.Equal("name", addAccount.AccountName.GetValue(null));
            }
        }

        [Theory]
        [InlineData("-p 4", 4)]
        public void AddAccountCommand_WithOptions(string optionsStr, long? expectedPriority)
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();

                ICommandRoot root = BudgetCliCommands.CMD_NEW_ACCOUNT;
                ICommandUsage usage = root.Usages.First(x => !x.IsHelp);

                CommandInterpreter interpreter = new CommandInterpreter(SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object), BudgetCliCommands.BuildCommandLibrary());

                ICommandAction action;
                bool success = interpreter.TryParseCommand("new account name " + optionsStr, out action);

                Assert.True(success);
                Assert.IsType<AddAccountCommand>(action);

                AddAccountCommand addAccount = (AddAccountCommand)action;
                Assert.Equal("name", addAccount.AccountName.GetValue(null));

                if(expectedPriority.HasValue)
                {
                    Assert.Equal(expectedPriority.Value, addAccount.PriorityOption.GetValue(null));
                }
            }
        }

        [Fact]
        public void ListAccountCommand()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();

                ICommandRoot root = BudgetCliCommands.CMD_LS_ACCOUNTS;
                ICommandUsage usage = root.Usages.First(x => !x.IsHelp);

                CommandInterpreter interpreter = new CommandInterpreter(SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object), BudgetCliCommands.BuildCommandLibrary());

                ICommandAction action;
                bool success = interpreter.TryParseCommand("ls accounts", out action);

                Assert.True(success);
                Assert.IsType<ListAccountCommand>(action);
            }
        }

        [Fact]
        public void DeleteAccountCommand()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();

                ICommandRoot root = BudgetCliCommands.CMD_REMOVE_ACCOUNTS;
                ICommandUsage usage = root.Usages.First(x => !x.IsHelp);

                CommandInterpreter interpreter = new CommandInterpreter(SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object), BudgetCliCommands.BuildCommandLibrary());

                ICommandAction action;
                bool success = interpreter.TryParseCommand("rm account test", out action);

                Assert.True(success);
                Assert.IsType<DeleteAccountCommand>(action);

                DeleteAccountCommand deleteAccount = (DeleteAccountCommand)action;
                Assert.Equal("test", deleteAccount.AccountName.GetValue(null));
            }
        }
    }
}