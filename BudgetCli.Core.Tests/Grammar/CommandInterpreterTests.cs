using System.Diagnostics;
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
using BudgetCli.Data.Enums;
using BudgetCli.Core.Enums;
using BudgetCli.Core.Models.Commands.SystemCommands;

namespace BudgetCli.Core.Tests.Grammar
{
    public class CommandInterpreterTests
    {
        [Fact]
        public void UnrecognizedCommand()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();

                CommandInterpreter interpreter = new CommandInterpreter(SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object), BudgetCliCommands.BuildCommandLibrary());

                ICommandAction action;
                bool success = interpreter.TryParseCommand("NOT A REAL COMMAND", out action);

                Assert.False(success);
                Assert.Null(action);
            }
        }

        [Theory]
        [InlineData("exit", CommandKind.Exit)]
        [InlineData("help", CommandKind.Help)]
        [InlineData("clear", CommandKind.ClearConsole)]
        [InlineData("version", CommandKind.Version)]
        public void SystemCommand(string input, CommandKind expectedKind)
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();

                CommandInterpreter interpreter = new CommandInterpreter(SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object), BudgetCliCommands.BuildCommandLibrary());

                ICommandAction action;
                bool success = interpreter.TryParseCommand(input, out action);

                Assert.True(success);
                Assert.NotNull(action);
                Assert.IsType(typeof(SystemCommand), action);
                Assert.Equal(expectedKind, ((SystemCommand)action).CommandKind);
            } 
        }

        [Fact]
        public void AddAccountCommand()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();

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

                CommandInterpreter interpreter = new CommandInterpreter(SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object), BudgetCliCommands.BuildCommandLibrary());

                ICommandAction action;
                bool success = interpreter.TryParseCommand("ls accounts", out action);

                Assert.True(success);
                Assert.IsType<ListAccountCommand>(action);
            }
        }

        [Fact]
        public void ListAccountCommand_Options_()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();

                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);

                //Id will be 1
                repo.Upsert(new Data.Models.AccountDto(){
                    AccountKind = AccountKind.Source,
                    CategoryId = 2,
                    Description = "Description",
                    Name = "Name",
                    Priority = 5
                });
                //Id will be 2
                repo.Upsert(new Data.Models.AccountDto(){
                    AccountKind = AccountKind.Category,
                    CategoryId = null,
                    Description = "",
                    Name = "Category",
                    Priority = 5
                });

                CommandInterpreter interpreter = new CommandInterpreter(SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo), BudgetCliCommands.BuildCommandLibrary());

                ICommandAction action;
                bool success = interpreter.TryParseCommand("list accounts -n Name -c Category -d Description -y Source", out action);

                Assert.True(success);
                Assert.IsType<ListAccountCommand>(action);

                ListAccountCommand command = (ListAccountCommand)action;
                Assert.Equal("Name", command.NameOption.GetValue("N/A"));
                Assert.Equal("Description", command.DescriptionOption.GetValue("N/A"));
                Assert.Equal(2L, command.CategoryIdOption.GetValue("N/A"));
                Assert.Equal(AccountKind.Source, command.AccountTypeOption.GetValue(AccountKind.Sink));
            }
        }

        [Fact]
        public void DeleteAccountCommand()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();

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