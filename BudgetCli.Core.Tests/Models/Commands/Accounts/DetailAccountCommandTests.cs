using System;
using BudgetCli.Core.Models;
using BudgetCli.Core.Models.CommandResults;
using BudgetCli.Core.Models.Commands.Accounts;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Tests.TestHarness;
using BudgetCli.Util.Logging;
using BudgetCli.Util.Models;
using Moq;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Commands.Accounts
{
    public class DetailAccountCommandTests
    {
        private void UpsertAccount(AccountDto dto, RepositoryBag repositories, double initialFunds = 0.0)
        {
            repositories.AccountRepository.Upsert(dto);
            repositories.AccountStateRepository.Upsert(new AccountStateDto(){
                AccountId = dto.Id.Value,
                IsClosed = false,
                Funds = ((Money)initialFunds).InternalValue,
                Timestamp = DateTime.Today
            });
        }
        
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

                AccountDto account1 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account1",
                    Priority = 123,
                    Description = "account1 description",
                    CategoryId = null
                };
                AccountDto account2 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account2",
                    Priority = 5,
                    Description = "account2 description",
                    CategoryId = null
                };

                UpsertAccount(account1, repositories);
                UpsertAccount(account2, repositories);

                DetailAccountCommand action = new DetailAccountCommand("detail account account1", repositories);
                action.NameOption.SetData("account1");
                
                ReadDetailsCommandResult<Account> result = null;
                Mock<ICommandActionListener> mockListener = new Mock<ICommandActionListener>();
                mockListener.Setup(x => x.OnCommand(It.IsAny<ReadDetailsCommandResult<Account>>())).Callback<ReadDetailsCommandResult<Account>>((y) => { result = y;});

                //Act
                bool successful = action.TryExecute(mockLog.Object, new []{mockListener.Object});

                Account account = DtoToModelTranslator.FromDto(repo.GetById(1), DateTime.Today, repositories);
                
                //Assert
                Assert.True(successful);
                Assert.NotNull(result);
                Assert.Equal(account1.Id, result.Item.Id);
            }
        }
        
        [Fact]
        public void TestAddAccountActionExecute_NameNotExist()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);
                
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, accountStateRepo);

                DetailAccountCommand action = new DetailAccountCommand("detail account missing", repositories);
                action.NameOption.SetData("missing");
                
                ReadDetailsCommandResult<Account> result = null;
                Mock<ICommandActionListener> mockListener = new Mock<ICommandActionListener>();
                mockListener.Setup(x => x.OnCommand(It.IsAny<ReadDetailsCommandResult<Account>>())).Callback<ReadDetailsCommandResult<Account>>((y) => { result = y;});

                //Act
                bool successful = action.TryExecute(mockLog.Object, new []{mockListener.Object});
                
                //Assert
                Assert.False(successful);
                Assert.False(result.IsSuccessful);
                Assert.Null(result.Item);
            }
        }
        
        [Fact]
        public void TestAddAccountActionExecute_PastState()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);
                
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, accountStateRepo);

                AccountDto account1 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account1",
                    Priority = 123,
                    Description = "account1 description",
                    CategoryId = null
                };

                UpsertAccount(account1, repositories);

                DateTime state1Timestamp = DateTime.Today.AddDays(-100);
                AccountStateDto accountState = new AccountStateDto()
                {
                    AccountId = account1.Id.Value,
                    Funds = (new Money(100)).InternalValue,
                    IsClosed = false,
                    Timestamp = state1Timestamp
                };
                accountStateRepo.Upsert(accountState);

                DateTime state2Timestamp = DateTime.Today.AddDays(-500);
                AccountStateDto accountState2 = new AccountStateDto()
                {
                    AccountId = account1.Id.Value,
                    Funds = (new Money(200)).InternalValue,
                    IsClosed = false,
                    Timestamp = state2Timestamp
                };
                accountStateRepo.Upsert(accountState2);

                DetailAccountCommand action = new DetailAccountCommand("detail account account1", repositories);
                action.NameOption.SetData("account1");
                action.DateOption.SetData(state1Timestamp.AddDays(-1)); //Latest state should be at timestamp2
                
                ReadDetailsCommandResult<Account> result = null;
                Mock<ICommandActionListener> mockListener = new Mock<ICommandActionListener>();
                mockListener.Setup(x => x.OnCommand(It.IsAny<ReadDetailsCommandResult<Account>>())).Callback<ReadDetailsCommandResult<Account>>((y) => { result = y;});

                //Act
                bool successful = action.TryExecute(mockLog.Object, new []{mockListener.Object});
                
                //Assert
                Assert.True(successful);
                Assert.NotNull(result);
                Assert.Equal(account1.Id, result.Item.Id);
                Assert.Equal(200, result.Item.CurrentState.Funds);
            }
        }
    }
}