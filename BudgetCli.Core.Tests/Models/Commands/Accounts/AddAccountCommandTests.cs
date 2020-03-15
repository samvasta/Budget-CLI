using System;
using System.Collections.Generic;
using System.Linq;
using BudgetCli.Core.Models;
using BudgetCli.Core.Models.CommandResults;
using BudgetCli.Core.Models.Commands.Accounts;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Tests.TestHarness;
using BudgetCli.Util.Logging;
using BudgetCli.Util.Models;
using Moq;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Commands.Accounts
{
    public class AddAccountCommandTests
    {
        [Fact]
        public void TestAddAccountActionExecute()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, accountStateRepo);

                AddAccountCommand action = new AddAccountCommand("new account \"Test Account\"", repositories, "Test Account");
                action.FundsOption.SetData(123.45);
                action.DescriptionOption.SetData("Test Description");
                
                //Act
                bool successful = action.TryExecute(mockLog.Object);

                Account account = DtoToModelTranslator.FromDto(repo.GetById(1), DateTime.Today, repositories);
                AccountState state = DtoToModelTranslator.FromDto(accountStateRepo.GetLatestByAccountId(1), repositories);
                
                //Assert
                Assert.True(successful);

                Assert.Equal("Test Account", account.Name);
                Assert.Null(account.CategoryId);
                Assert.Equal(Account.DEFAULT_PRIORITY, account.Priority);
                Assert.Equal(Data.Enums.AccountKind.Sink, account.AccountKind);
                Assert.Equal("Test Description", account.Description);

                Assert.NotNull(state);
                Assert.Equal(123.45, state.Funds);
                Assert.False(state.IsClosed);
            }
        }
        
        [Fact]
        public void TestAddAccountActionExecute_DuplicateName()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, accountStateRepo);

                AccountDto dto = new AccountDto()
                {
                    Name = "Test Account"
                };

                repo.Upsert(dto);

                AddAccountCommand action = new AddAccountCommand("new account \"Test Account\"", repositories, "Test Account");
                action.FundsOption.SetData(123.45);
                action.DescriptionOption.SetData("Test Description");
                
                //Act
                bool successful = action.TryExecute(mockLog.Object);

                //Assert
                Assert.False(successful);
            }
        }

        [Fact]
        public void TestAddAccountActionExecute_WithListeners()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, accountStateRepo);

                AddAccountCommand action = new AddAccountCommand("new account \"Test Account\"", repositories, "Test Account");
                action.FundsOption.SetData(123.45);
                action.DescriptionOption.SetData("Test Description");

                Mock<ICommandActionListener> mockListener = new Mock<ICommandActionListener>();
                mockListener.Setup(x => x.OnCommand(It.Is<CreateCommandResult<Account>>(a => a.IsSuccessful && a.CreatedItem.Name.Equals("Test Account")))).Verifiable();

                List<ICommandActionListener> listeners = new List<ICommandActionListener>();
                listeners.Add(mockListener.Object);
                
                //Act
                bool successful = action.TryExecute(mockLog.Object, listeners);

                Account account = DtoToModelTranslator.FromDto(repo.GetById(1), DateTime.Today, repositories);
                AccountState state = DtoToModelTranslator.FromDto(accountStateRepo.GetLatestByAccountId(1), repositories);
                
                //Assert
                Assert.True(successful);

                Assert.Equal("Test Account", account.Name);
                Assert.Null(account.CategoryId);
                Assert.Equal(Account.DEFAULT_PRIORITY, account.Priority);
                Assert.Equal(Data.Enums.AccountKind.Sink, account.AccountKind);
                Assert.Equal("Test Description", account.Description);

                Assert.NotNull(state);
                Assert.Equal(123.45, state.Funds);
                Assert.False(state.IsClosed);

                mockListener.VerifyAll();
            }
        }

        [Fact]
        public void TestAddAccountActionExecute_DuplicateName_WithListeners()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, accountStateRepo);
                
                AccountDto dto = new AccountDto()
                {
                    Name = "Test Account"
                };

                repo.Upsert(dto);

                AddAccountCommand action = new AddAccountCommand("new account \"Test Account\"", repositories, "Test Account");
                action.FundsOption.SetData(123.45);
                action.DescriptionOption.SetData("Test Description");

                Mock<ICommandActionListener> mockListener = new Mock<ICommandActionListener>();
                mockListener.Setup(x => x.OnCommand(It.Is<CreateCommandResult<Account>>(a => !a.IsSuccessful && a.CreatedItem==null))).Verifiable();

                List<ICommandActionListener> listeners = new List<ICommandActionListener>();
                listeners.Add(mockListener.Object);
                
                //Act
                bool successful = action.TryExecute(mockLog.Object, listeners);

                //Assert
                Assert.False(successful);
                mockListener.VerifyAll();
            }
        }
    }
}