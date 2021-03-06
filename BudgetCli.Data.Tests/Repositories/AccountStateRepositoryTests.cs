using System.IO.Compression;
using System;
using System.Collections.Generic;
using System.Linq;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Tests.TestHarness;
using BudgetCli.Util.Logging;
using BudgetCli.Util.Models;
using Moq;
using Xunit;

namespace BudgetCli.Data.Tests.Repositories
{
    public class AccountStateRepositoryTests
    {
        [Fact]
        public void TestGetLatestStateByAccountId()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountRepository accountRepo = new AccountRepository(testDbInfo.ConnectionString, log.Object);
                AccountStateRepository repo = new AccountStateRepository(testDbInfo.ConnectionString, log.Object);

                AccountDto account = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "account",
                    Priority = 5,
                    CategoryId = null
                };
                bool isInsertSuccessful = accountRepo.Upsert(account);

                DateTime state1Timestamp = DateTime.Now;
                AccountStateDto accountState = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(100)).InternalValue,
                    IsClosed = false,
                    Timestamp = state1Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState);

                
                DateTime state2Timestamp = state1Timestamp.AddDays(-1);
                AccountStateDto accountState2 = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(500)).InternalValue,
                    IsClosed = false,
                    Timestamp = state2Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState2);
                
                DateTime state3Timestamp = state1Timestamp.AddDays(-2);
                AccountStateDto accountState3 = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(800)).InternalValue,
                    IsClosed = false,
                    Timestamp = state3Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState3);
                
                //Act
                AccountStateDto latest = repo.GetLatestByAccountId(account.Id.Value);
                
                //Assert
                Assert.True(isInsertSuccessful);

                Assert.Equal(100, new Money(latest.Funds, true));
            }
        }
        
        [Fact]
        public void TestGetLatestStateByAccountId_WithDate()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountRepository accountRepo = new AccountRepository(testDbInfo.ConnectionString, log.Object);
                AccountStateRepository repo = new AccountStateRepository(testDbInfo.ConnectionString, log.Object);

                AccountDto account = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "account",
                    Priority = 5,
                    CategoryId = null
                };
                bool isInsertSuccessful = accountRepo.Upsert(account);

                DateTime state1Timestamp = DateTime.Today;
                AccountStateDto accountState = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(100)).InternalValue,
                    IsClosed = false,
                    Timestamp = state1Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState);

                
                DateTime state2Timestamp = state1Timestamp.AddDays(-10);
                AccountStateDto accountState2 = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(500)).InternalValue,
                    IsClosed = false,
                    Timestamp = state2Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState2);
                
                DateTime state3Timestamp = state1Timestamp.AddDays(-20);
                AccountStateDto accountState3 = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(800)).InternalValue,
                    IsClosed = false,
                    Timestamp = state3Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState3);
                
                //Act
                AccountStateDto a = repo.GetLatestByAccountId(account.Id.Value, state3Timestamp);
                AccountStateDto b = repo.GetLatestByAccountId(account.Id.Value, state3Timestamp.AddDays(5));    // half way between 3 and 2
                AccountStateDto c = repo.GetLatestByAccountId(account.Id.Value, state2Timestamp);
                AccountStateDto d = repo.GetLatestByAccountId(account.Id.Value, state2Timestamp.AddDays(5));    //Half way between 2 and 1
                AccountStateDto e = repo.GetLatestByAccountId(account.Id.Value, state1Timestamp);
                AccountStateDto f = repo.GetLatestByAccountId(account.Id.Value, state1Timestamp.AddDays(5));    //5 days in future
                
                //Assert
                Assert.True(isInsertSuccessful);

                Assert.Equal(800, new Money(a.Funds, true));
                Assert.Equal(800, new Money(b.Funds, true));
                Assert.Equal(500, new Money(c.Funds, true));
                Assert.Equal(500, new Money(d.Funds, true));
                Assert.Equal(100, new Money(e.Funds, true));
                Assert.Equal(100, new Money(f.Funds, true));
            }
        }
        
        [Fact]
        public void TestGetLatestStateByAccountId_MultipleAccounts()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountRepository accountRepo = new AccountRepository(testDbInfo.ConnectionString, log.Object);
                AccountStateRepository repo = new AccountStateRepository(testDbInfo.ConnectionString, log.Object);

                AccountDto account = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "account",
                    Priority = 5,
                    CategoryId = null
                };
                AccountDto account2 = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "account2",
                    Priority = 5,
                    CategoryId = null
                };
                bool isInsertSuccessful = accountRepo.Upsert(account);
                isInsertSuccessful &= accountRepo.Upsert(account2);

                DateTime state1Timestamp = DateTime.Now;
                AccountStateDto accountState = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(100)).InternalValue,
                    IsClosed = false,
                    Timestamp = state1Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState);

                
                DateTime state2Timestamp = state1Timestamp.AddDays(-1);
                AccountStateDto accountState2 = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(500)).InternalValue,
                    IsClosed = false,
                    Timestamp = state2Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState2);
                
                DateTime state3Timestamp = state1Timestamp.AddDays(-2);
                AccountStateDto accountState3 = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(800)).InternalValue,
                    IsClosed = false,
                    Timestamp = state3Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState3);
                
                //This should be the most recent state, but doesn't belong to the original account
                DateTime state4Timestamp = state1Timestamp.AddDays(5);
                AccountStateDto accountState4 = new AccountStateDto()
                {
                    AccountId = account2.Id.Value,
                    Funds = (new Money(800)).InternalValue,
                    IsClosed = false,
                    Timestamp = state3Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState4);
                
                //Act
                AccountStateDto latest = repo.GetLatestByAccountId(account.Id.Value);
                
                //Assert
                Assert.True(isInsertSuccessful);
                Assert.Equal(state1Timestamp, latest.Timestamp);
                Assert.Equal(100, new Money(latest.Funds, true));
            }
        }
        
        [Fact]
        public void TestGetAllStatesByAccountId()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountRepository accountRepo = new AccountRepository(testDbInfo.ConnectionString, log.Object);
                AccountStateRepository repo = new AccountStateRepository(testDbInfo.ConnectionString, log.Object);

                AccountDto account = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "account",
                    Priority = 5,
                    CategoryId = null
                };
                bool isInsertSuccessful = accountRepo.Upsert(account);

                DateTime state1Timestamp = DateTime.Now;
                AccountStateDto accountState = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(100)).InternalValue,
                    IsClosed = false,
                    Timestamp = state1Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState);

                
                DateTime state2Timestamp = state1Timestamp.AddDays(-1);
                AccountStateDto accountState2 = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(500)).InternalValue,
                    IsClosed = false,
                    Timestamp = state2Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState2);
                
                DateTime state3Timestamp = state1Timestamp.AddDays(-2);
                AccountStateDto accountState3 = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(800)).InternalValue,
                    IsClosed = false,
                    Timestamp = state3Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState3);
                
                //Act
                List<AccountStateDto> allStates = repo.GetAllByAccountId(account.Id.Value);
                
                //Assert
                Assert.True(isInsertSuccessful);
                Assert.Equal(3, allStates.Count);
                Assert.True(allStates.Any(x => x.Equals(accountState)));
                Assert.True(allStates.Any(x => x.Equals(accountState2)));
                Assert.True(allStates.Any(x => x.Equals(accountState3)));
            }
        }

        
        
        [Fact]
        public void TestRemoveAllStatesByAccountId()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountRepository accountRepo = new AccountRepository(testDbInfo.ConnectionString, log.Object);
                AccountStateRepository repo = new AccountStateRepository(testDbInfo.ConnectionString, log.Object);

                AccountDto account = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "account",
                    Priority = 5,
                    CategoryId = null
                };
                bool isInsertSuccessful = accountRepo.Upsert(account);

                DateTime state1Timestamp = DateTime.Now;
                AccountStateDto accountState = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(100)).InternalValue,
                    IsClosed = false,
                    Timestamp = state1Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState);

                
                DateTime state2Timestamp = state1Timestamp.AddDays(-1);
                AccountStateDto accountState2 = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(500)).InternalValue,
                    IsClosed = false,
                    Timestamp = state2Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState2);
                
                DateTime state3Timestamp = state1Timestamp.AddDays(-2);
                AccountStateDto accountState3 = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(800)).InternalValue,
                    IsClosed = false,
                    Timestamp = state3Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState3);
                
                //Act
                List<AccountStateDto> allStatesBeforeRemove = repo.GetAllByAccountId(account.Id.Value);
                bool isSuccessful = repo.RemoveAllByAccountId(account.Id.Value);
                List<AccountStateDto> allStatesAfterRemove = repo.GetAllByAccountId(account.Id.Value);
                
                //Assert
                Assert.True(isInsertSuccessful);
                Assert.True(isSuccessful);
                Assert.Equal(3, allStatesBeforeRemove.Count);
                Assert.Empty(allStatesAfterRemove);
            }
        }

        [Fact]
        public void TestRemoveAllStatesByAccountId_NoStates()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountRepository accountRepo = new AccountRepository(testDbInfo.ConnectionString, log.Object);
                AccountStateRepository repo = new AccountStateRepository(testDbInfo.ConnectionString, log.Object);

                AccountDto account = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "account",
                    Priority = 5,
                    CategoryId = null
                };
                bool isInsertSuccessful = accountRepo.Upsert(account);

                //Act
                List<AccountStateDto> allStatesBeforeRemove = repo.GetAllByAccountId(account.Id.Value);
                bool isSuccessful = repo.RemoveAllByAccountId(account.Id.Value);
                List<AccountStateDto> allStatesAfterRemove = repo.GetAllByAccountId(account.Id.Value);
                
                //Assert
                Assert.True(isInsertSuccessful);
                Assert.False(isSuccessful);
                Assert.Empty(allStatesBeforeRemove);
                Assert.Empty(allStatesAfterRemove);
            }
        }

        
        [Fact]
        public void TestRemoveAllStatesByAccountId_InvalidAccount()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountStateRepository repo = new AccountStateRepository(testDbInfo.ConnectionString, log.Object);

                //Act
                bool isSuccessful = repo.RemoveAllByAccountId(1);
                
                //Assert
                Assert.False(isSuccessful);
            }
        }

        
        
        [Fact]
        public void TestCloseAccount()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountRepository accountRepo = new AccountRepository(testDbInfo.ConnectionString, log.Object);
                AccountStateRepository repo = new AccountStateRepository(testDbInfo.ConnectionString, log.Object);

                AccountDto account = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "account",
                    Priority = 5,
                    CategoryId = null
                };
                bool isInsertSuccessful = accountRepo.Upsert(account);

                DateTime state1Timestamp = DateTime.Now;
                AccountStateDto accountState = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(100)).InternalValue,
                    IsClosed = false,
                    Timestamp = state1Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState);
                
                //Act
                bool successful = repo.CloseAccount(account.Id.Value);
                AccountStateDto latestState = repo.GetLatestByAccountId(account.Id.Value);
                
                //Assert
                Assert.True(isInsertSuccessful);
                Assert.True(successful);
                Assert.True(latestState.IsClosed);
            }
        }

        [Fact]
        public void TestCloseAccount_BadAccountId()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountStateRepository repo = new AccountStateRepository(testDbInfo.ConnectionString, log.Object);
                
                //Act
                bool successful = repo.CloseAccount(1);
                
                //Assert
                Assert.False(successful);
            }
        }
        
        [Fact]
        public void TestOpenAccount()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountRepository accountRepo = new AccountRepository(testDbInfo.ConnectionString, log.Object);
                AccountStateRepository repo = new AccountStateRepository(testDbInfo.ConnectionString, log.Object);

                AccountDto account = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "account",
                    Priority = 5,
                    CategoryId = null
                };
                bool isInsertSuccessful = accountRepo.Upsert(account);

                DateTime state1Timestamp = DateTime.Now;
                AccountStateDto accountState = new AccountStateDto()
                {
                    AccountId = account.Id.Value,
                    Funds = (new Money(100)).InternalValue,
                    IsClosed = true,
                    Timestamp = state1Timestamp
                };
                isInsertSuccessful &= repo.Upsert(accountState);
                
                //Act
                bool successful = repo.ReOpenAccount(account.Id.Value);
                AccountStateDto latestState = repo.GetLatestByAccountId(account.Id.Value);
                
                //Assert
                Assert.True(isInsertSuccessful);
                Assert.True(successful);
                Assert.False(latestState.IsClosed);
            }
        }
        
        [Fact]
        public void TestOpenAccount_BadAccountId()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountStateRepository repo = new AccountStateRepository(testDbInfo.ConnectionString, log.Object);
                
                //Act
                bool successful = repo.ReOpenAccount(1);
                
                //Assert
                Assert.False(successful);
            }
        }
    }
}