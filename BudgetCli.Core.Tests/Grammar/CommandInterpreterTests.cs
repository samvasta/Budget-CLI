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
using BudgetCli.Util.Models;

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

        [Fact]
        public void AddAccountCommand_NoName()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();

                CommandInterpreter interpreter = new CommandInterpreter(SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object), BudgetCliCommands.BuildCommandLibrary());

                ICommandAction action;
                bool success = interpreter.TryParseCommand("new account", out action);

                Assert.False(success);
                Assert.Null(action);
            }
        }

        [Fact]
        public void AddAccountCommand_WithOptions()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();

                CommandInterpreter interpreter = new CommandInterpreter(SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object), BudgetCliCommands.BuildCommandLibrary());

                ICommandAction action;
                bool success = interpreter.TryParseCommand("new account name -p 4 -c category -d description -f 10 -y Sink", out action);

                Assert.True(success);
                Assert.IsType<AddAccountCommand>(action);

                AddAccountCommand addAccount = (AddAccountCommand)action;
                Assert.Equal("name", addAccount.AccountName.GetValue(null));

                Assert.Equal(4L, addAccount.PriorityOption.GetValue(null));
                Assert.Equal("category", addAccount.CategoryNameOption.GetValue(null));
                Assert.Equal("description", addAccount.DescriptionOption.GetValue(null));
                Assert.Equal(new Money(10), addAccount.FundsOption.GetValue(null));
                Assert.Equal(AccountKind.Sink, addAccount.AccountTypeOption.GetValue(null));
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
        public void ListAccountCommand_Options()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();

                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository stateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);

                //Id will be 1
                repo.Upsert(new Data.Models.AccountDto(){
                    AccountKind = AccountKind.Source,
                    CategoryId = 2,
                    Description = "Description",
                    Name = "Name",
                    Priority = 10
                });
                stateRepo.Upsert(new Data.Models.AccountStateDto(){
                    AccountId = 1,
                    Funds = 10,
                    IsClosed = false,
                    Timestamp = DateTime.Today
                });

                //Id will be 2
                repo.Upsert(new Data.Models.AccountDto(){
                    AccountKind = AccountKind.Category,
                    CategoryId = null,
                    Description = "",
                    Name = "Category",
                    Priority = 5
                });
                stateRepo.Upsert(new Data.Models.AccountStateDto(){
                    AccountId = 2,
                    Funds = 100,
                    IsClosed = false,
                    Timestamp = DateTime.Today
                });

                //Id will be 3
                repo.Upsert(new Data.Models.AccountDto(){
                    AccountKind = AccountKind.Category,
                    CategoryId = null,
                    Description = "",
                    Name = "Category2",
                    Priority = 5
                });
                stateRepo.Upsert(new Data.Models.AccountStateDto(){
                    AccountId = 3,
                    Funds = 50,
                    IsClosed = false,
                    Timestamp = DateTime.Today
                });

                CommandInterpreter interpreter = new CommandInterpreter(SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, stateRepo), BudgetCliCommands.BuildCommandLibrary());

                ICommandAction action;
                bool success = interpreter.TryParseCommand("list accounts -n Name -c Category -d Description -y Source -p (4,6) -f (90,110)", out action);

                Assert.True(success);
                Assert.IsType<ListAccountCommand>(action);

                ListAccountCommand command = (ListAccountCommand)action;
                Assert.Equal("Name", command.NameOption.GetValue("N/A"));
                Assert.Equal("Description", command.DescriptionOption.GetValue("N/A"));
                Assert.Equal(2L, command.CategoryIdOption.GetValue("N/A"));
                Assert.Equal(AccountKind.Source, command.AccountTypeOption.GetValue(AccountKind.Sink));

                Range<long> expectedPriorityRange = new Range<long>(4, 6, false, false);
                Range<Money> expectedFundsRange = new Range<Money>(90, 110, false, false);
                Assert.Equal(expectedFundsRange, command.FundsOption.GetValue(null));
                Assert.Equal(expectedPriorityRange, command.PriorityOption.GetValue(null));
            }
        }

        [Fact]
        public void DetailAccountCommand()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();

                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository stateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);

                //Id will be 1
                repo.Upsert(new Data.Models.AccountDto(){
                    AccountKind = AccountKind.Source,
                    CategoryId = null,
                    Description = "Description",
                    Name = "Name",
                    Priority = 5
                });
                stateRepo.Upsert(new Data.Models.AccountStateDto(){
                    AccountId = 1,
                    Funds = 10,
                    IsClosed = false,
                    Timestamp = DateTime.Today
                });

                CommandInterpreter interpreter = new CommandInterpreter(SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, stateRepo), BudgetCliCommands.BuildCommandLibrary());

                ICommandAction action;
                bool success = interpreter.TryParseCommand("detail account \"Name\"", out action);

                Assert.True(success);
                Assert.IsType<DetailAccountCommand>(action);

                DetailAccountCommand command = (DetailAccountCommand)action;
                Assert.Equal("Name", command.NameOption.GetValue("N/A"));
                Assert.Null(command.DateOption.GetValue(null));
            }
        }

        [Fact]
        public void DetailAccountCommand_WithDate()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();

                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository stateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);

                //Id will be 1
                repo.Upsert(new Data.Models.AccountDto(){
                    AccountKind = AccountKind.Source,
                    CategoryId = null,
                    Description = "Description",
                    Name = "Name",
                    Priority = 5
                });
                stateRepo.Upsert(new Data.Models.AccountStateDto(){
                    AccountId = 1,
                    Funds = 10,
                    IsClosed = false,
                    Timestamp = DateTime.Today.AddDays(-10)
                });
                stateRepo.Upsert(new Data.Models.AccountStateDto(){
                    AccountId = 1,
                    Funds = 100,
                    IsClosed = false,
                    Timestamp = DateTime.Today
                });

                CommandInterpreter interpreter = new CommandInterpreter(SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, stateRepo), BudgetCliCommands.BuildCommandLibrary());

                ICommandAction action;
                bool success = interpreter.TryParseCommand("detail account \"Name\" -d yesterday", out action);

                Assert.True(success);
                Assert.IsType<DetailAccountCommand>(action);

                DetailAccountCommand command = (DetailAccountCommand)action;
                Assert.Equal("Name", command.NameOption.GetValue("N/A"));
                Assert.Equal(DateTime.Today.AddDays(-1), command.DateOption.GetValue(null));
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

        [Fact]
        public void DeleteAccountCommand_Recursive()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();

                CommandInterpreter interpreter = new CommandInterpreter(SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object), BudgetCliCommands.BuildCommandLibrary());

                ICommandAction action;
                bool success = interpreter.TryParseCommand("rm account test -r", out action);

                Assert.True(success);
                Assert.IsType<DeleteAccountCommand>(action);

                DeleteAccountCommand deleteAccount = (DeleteAccountCommand)action;
                Assert.Equal("test", deleteAccount.AccountName.GetValue(null));
                Assert.True(deleteAccount.IsRecursiveOption.GetValue(false));
            }
        }
    }
}