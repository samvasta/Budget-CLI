using System;
using System.Collections.Generic;
using System.Linq;
using BudgetCli.Core.Cli;
using BudgetCli.Core.Interpreters;
using BudgetCli.Core.Models.Commands;
using BudgetCli.Core.Models.Commands.Accounts;
using BudgetCli.Core.Utilities;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Tests.TestHarness;
using BudgetCli.Util.Logging;
using Moq;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Interpreters
{
    public class MainInterpreterTests
    {
        [Fact]
        public void TestMainInterpreter()
        {
            using(var dbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();
                VisitorBag visitors = VisitorBagUtil.GetRuntimeVisitorBag(new RepositoryBag()
                {
                    AccountRepository = new AccountRepository(dbInfo.ConnectionString, mockLog.Object),
                    AccountStateRepository = new AccountStateRepository(dbInfo.ConnectionString, mockLog.Object),
                    TransactionRepository = new TransactionRepository(dbInfo.ConnectionString, mockLog.Object),
                });
                MainInterpreter interpreter = new MainInterpreter(visitors);
                InterpreterResult<ICommandAction> result = interpreter.GetAction("new account Test");

                Assert.True(result.IsSuccessful);
                Assert.IsType(typeof(AddAccountCommand), result.ReturnValue);

                AddAccountCommand command = (AddAccountCommand)result.ReturnValue;

                Assert.Equal("Test", command.AccountName.GetValue(String.Empty));
                Assert.False(command.AccountTypeOption.IsDataValid);
                Assert.False(command.CategoryIdOption.IsDataValid);
                Assert.False(command.DescriptionOption.IsDataValid);
                Assert.False(command.FundsOption.IsDataValid);
                Assert.False(command.PriorityOption.IsDataValid);
            }
        }

        [Theory]
        [InlineData("new account Test --type sink", true, false, false, false, false)]
        [InlineData("new account Test -t source", true, false, false, false, false)]
        [InlineData("new account Test --description \"Description\"", false, false, true, false, false)]
        [InlineData("new account Test -f 123.45", false, false, false, true, false)]
        [InlineData("new account Test -p 1", false, false, false, false, true)]
        [InlineData("new account Test --priority 1", false, false, false, false, true)]
        public void TestMainInterpreter_AddAccount(string input, bool isAccountTypeValid, bool isCategoryValid, bool isDescriptionValid, bool isFundsValid, bool isPriorityValid)
        {
            using(var dbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                RepositoryBag repositories = new RepositoryBag()
                {
                    AccountRepository = new AccountRepository(dbInfo.ConnectionString, mockLog.Object),
                    AccountStateRepository = new AccountStateRepository(dbInfo.ConnectionString, mockLog.Object),
                    TransactionRepository = new TransactionRepository(dbInfo.ConnectionString, mockLog.Object),
                };
                VisitorBag visitors = VisitorBagUtil.GetRuntimeVisitorBag(repositories);
                MainInterpreter interpreter = new MainInterpreter(visitors);

                //Act
                InterpreterResult<ICommandAction> result = interpreter.GetAction(input);

                //Assert
                Assert.True(result.IsSuccessful);
                Assert.IsType(typeof(AddAccountCommand), result.ReturnValue);

                AddAccountCommand command = (AddAccountCommand)result.ReturnValue;

                Assert.Equal(isAccountTypeValid, command.AccountTypeOption.IsDataValid);
                Assert.Equal(isCategoryValid, command.CategoryIdOption.IsDataValid);
                Assert.Equal(isDescriptionValid, command.DescriptionOption.IsDataValid);
                Assert.Equal(isFundsValid, command.FundsOption.IsDataValid);
                Assert.Equal(isPriorityValid, command.PriorityOption.IsDataValid);
            }
        }
        

        [Fact]
        public void TestMainInterpreter_DeleteAccount()
        {
            using(var dbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                RepositoryBag repositories = new RepositoryBag()
                {
                    AccountRepository = new AccountRepository(dbInfo.ConnectionString, mockLog.Object),
                    AccountStateRepository = new AccountStateRepository(dbInfo.ConnectionString, mockLog.Object),
                    TransactionRepository = new TransactionRepository(dbInfo.ConnectionString, mockLog.Object),
                };
                VisitorBag visitors = VisitorBagUtil.GetRuntimeVisitorBag(repositories);
                MainInterpreter interpreter = new MainInterpreter(visitors);

                //Act
                InterpreterResult<ICommandAction> result = interpreter.GetAction("rm account Test");

                //Assert
                Assert.True(result.IsSuccessful);
                Assert.IsType(typeof(DeleteAccountCommand), result.ReturnValue);

                DeleteAccountCommand command = (DeleteAccountCommand)result.ReturnValue;

                Assert.Equal("Test", command.AccountName.GetValue(String.Empty));
                Assert.False(command.IsRecursiveOption.IsDataValid);
            }
        }
    }
}