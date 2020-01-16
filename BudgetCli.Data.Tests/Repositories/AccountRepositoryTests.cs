using System;
using System.Collections.Generic;
using System.Linq;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Repositories.Interfaces;
using BudgetCli.Data.Tests.TestHarness;
using BudgetCli.Util.Logging;
using BudgetCli.Util.Models;
using Moq;
using Xunit;

namespace BudgetCli.Data.Tests.Repositories
{
    public class AccountRepositoryTests
    {
        [Fact]
        public void TestGetIdByName()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, log.Object);

                AccountDto parent = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "parent",
                    Priority = 5,
                    CategoryId = null
                };

                bool isParentInsertSuccessful = repo.Upsert(parent);

                AccountDto child1 = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "child1",
                    Priority = 5,
                    CategoryId = parent.Id
                };

                bool isChild1InsertSuccessful = repo.Upsert(child1);

                AccountDto child2 = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "child2",
                    Priority = 5,
                    CategoryId = parent.Id
                };

                bool isChild2InsertSuccessful = repo.Upsert(child2);
                
                //Act
                long parentId = repo.GetIdByName("parent");
                long child1Id = repo.GetIdByName("child1");
                long child2Id = repo.GetIdByName("child2");
                
                //Assert
                Assert.True(isParentInsertSuccessful);
                Assert.True(isChild1InsertSuccessful);
                Assert.True(isChild2InsertSuccessful);

                Assert.Equal(parent.Id, parentId);
                Assert.Equal(child1.Id, child1Id);
                Assert.Equal(child2.Id, child2Id);
            }
        }

        
        [Fact]
        public void TestGetIdByName_NameDoesNotExist()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, log.Object);

                AccountDto account = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "account",
                    Priority = 5,
                    CategoryId = null
                };

                repo.Upsert(account);
                
                //Act
                long missingAccountId = repo.GetIdByName("doesNotExist");
                
                //Assert
                Assert.Equal(-1, missingAccountId);
            }
        }

        
        [Fact]
        public void TestGetChildAccountIds()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, log.Object);

                AccountDto parent = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "parent",
                    Priority = 5,
                    CategoryId = null
                };

                bool isParentInsertSuccessful = repo.Upsert(parent);

                AccountDto child1 = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "child1",
                    Priority = 5,
                    CategoryId = parent.Id
                };

                bool isChild1InsertSuccessful = repo.Upsert(child1);

                AccountDto unrelated = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "unrelated",
                    Priority = 5,
                    CategoryId = null
                };

                bool isUnrelatedInsertSuccessful = repo.Upsert(unrelated);

                AccountDto child2 = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "child2",
                    Priority = 5,
                    CategoryId = parent.Id
                };

                bool isChild2InsertSuccessful = repo.Upsert(child2);
                
                //Act
                var childIds = repo.GetChildAccountIds(parent.Id.Value);
                
                //Assert
                Assert.True(isParentInsertSuccessful);
                Assert.True(isChild1InsertSuccessful);
                Assert.True(isChild2InsertSuccessful);

                Assert.True(childIds.Contains(child1.Id.Value));
                Assert.True(childIds.Contains(child2.Id.Value));
                Assert.False(childIds.Contains(unrelated.Id.Value));
            }
        }

        
        [Fact]
        public void TestGetChildAccountIds_NoChildren()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, log.Object);

                AccountDto account = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "account",
                    Priority = 5,
                    CategoryId = null
                };

                bool isInsertSuccessful = repo.Upsert(account);

                AccountDto other1 = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "other1",
                    Priority = 5,
                    CategoryId = null
                };

                bool isOtherInsertSuccessful = repo.Upsert(other1);

                AccountDto unrelated = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "unrelated",
                    Priority = 5,
                    CategoryId = null
                };

                bool isUnrelatedInsertSuccessful = repo.Upsert(unrelated);

                AccountDto other2 = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "other2",
                    Priority = 5,
                    CategoryId = null
                };

                bool isOther2InsertSuccessful = repo.Upsert(other2);
                
                //Act
                var childIds = repo.GetChildAccountIds(account.Id.Value);
                
                //Assert
                Assert.True(isInsertSuccessful);
                Assert.True(isOtherInsertSuccessful);
                Assert.True(isOther2InsertSuccessful);

                //make sure it's empty
                Assert.NotNull(childIds);
                Assert.False(childIds.Any());
            }
        }

        
        [Fact]
        public void TestDoesNameExist_True()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, log.Object);

                AccountDto parent = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "parent",
                    Priority = 5,
                    CategoryId = null
                };

                bool isParentInsertSuccessful = repo.Upsert(parent);

                AccountDto child1 = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "child1",
                    Priority = 5,
                    CategoryId = parent.Id
                };

                bool isChild1InsertSuccessful = repo.Upsert(child1);

                AccountDto child2 = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "child2",
                    Priority = 5,
                    CategoryId = parent.Id
                };

                bool isChild2InsertSuccessful = repo.Upsert(child2);
                
                //Act
                bool parentExists = repo.DoesNameExist("parent");
                bool child1Exists = repo.DoesNameExist("child1");
                bool child2Exists = repo.DoesNameExist("child2");
                
                //Assert
                Assert.True(isParentInsertSuccessful);
                Assert.True(isChild1InsertSuccessful);
                Assert.True(isChild2InsertSuccessful);

                Assert.True(parentExists);
                Assert.True(child1Exists);
                Assert.True(child2Exists);
            }
        }

        
        [Fact]
        public void TestDoesNameExist_False()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, log.Object);

                AccountDto account = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "account",
                    Priority = 5,
                    CategoryId = null
                };

                repo.Upsert(account);
                
                //Act
                bool missingAccountExists = repo.DoesNameExist("doesNotExist");
                
                //Assert
                Assert.False(missingAccountExists);
            }
        }

        

        
        [Fact]
        public void TestGetAccounts()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, log.Object);
                AccountStateRepository stateRepo = new AccountStateRepository(testDbInfo.ConnectionString, log.Object);

                AccountDto parent = new AccountDto()
                {
                    AccountKind = Enums.AccountKind.Category,
                    Description = String.Empty,
                    Name = "parent",
                    Priority = 5,
                    CategoryId = null
                };

                bool isParentInsertSuccessful = repo.Upsert(parent);

                AccountDto testAccount = MakeAccount("Test Account");
                AccountDto accountWithDescription1 = MakeAccount("AccountWithDescription1", description: "this is a description");
                AccountDto accountWithDescription2 = MakeAccount("AccountWithDescription2", description: "descending");
                AccountDto accountPriority10 = MakeAccount("AccountWithPriority10", priority: 10);
                AccountDto accountPriority1 = MakeAccount("AccountWithPriority1", priority: 1);
                AccountDto childAccount1 = MakeAccount("child1", categoryId: parent.Id);
                AccountDto childAccount2WithDescription = MakeAccount("child2", categoryId: parent.Id, description: "description");
                AccountDto closedAccount = MakeAccount("closed", priority: 100);
                AccountDto fundedAccount = MakeAccount("funded", priority: 100);


                AddAccounts(repo,
                            testAccount,
                            accountWithDescription1,
                            accountWithDescription2,
                            accountPriority10,
                            accountPriority1,
                            childAccount1,
                            childAccount2WithDescription,
                            closedAccount,
                            fundedAccount);

                SetAccountState(stateRepo, parent, 0, false);
                SetAccountState(stateRepo, testAccount, 0, false);
                SetAccountState(stateRepo, accountWithDescription1, 0, false);
                SetAccountState(stateRepo, accountWithDescription2, 0, false);
                SetAccountState(stateRepo, accountPriority10, 0, false);
                SetAccountState(stateRepo, accountPriority1, 0, false);
                SetAccountState(stateRepo, childAccount1, 0, false);
                SetAccountState(stateRepo, childAccount2WithDescription, 0, false);
                SetAccountState(stateRepo, closedAccount, 0, true);
                SetAccountState(stateRepo, fundedAccount, 1000.00, false);
                
                //Act
                int numMatchingTestAccountName = repo.GetAccounts(nameContains: "Test Account").Count();
                int numMatchingChildName = repo.GetAccounts(nameContains: "child").Count();
                int numMatchingDescription = repo.GetAccounts(descriptionContains: "desc").Count();
                int numMatchingCategoryId = repo.GetAccounts(categoryId: parent.Id).Count();
                int numInPriorityRange1To5 = repo.GetAccounts(priorityRange: new Range<long>(1, 5)).Count();
                int numInPriorityRange6To15 = repo.GetAccounts(priorityRange: new Range<long>(6, 15)).Count();
                int numInPriorityRange0To4 = repo.GetAccounts(priorityRange: new Range<long>(0, 4)).Count();
                int numExcludingClosed = repo.GetAccounts(includeClosedAccounts: false).Count();
                int numIncludingClosed = repo.GetAccounts(includeClosedAccounts: true).Count();
                int numFunded = repo.GetAccounts(fundsRange: new Range<Money>(999.00, 1001.00)).Count();
                
                //Assert
                Assert.Equal(1, numMatchingTestAccountName);
                Assert.Equal(2, numMatchingChildName);
                Assert.Equal(3, numMatchingDescription);
                Assert.Equal(2, numMatchingCategoryId);
                Assert.Equal(7, numInPriorityRange1To5);
                Assert.Equal(1, numInPriorityRange6To15);
                Assert.Equal(1, numInPriorityRange0To4);
                Assert.Equal(9, numExcludingClosed);
                Assert.Equal(10, numIncludingClosed);
                Assert.Equal(1, numFunded);
            }
        }

        private AccountDto MakeAccount(string name, long? categoryId = null, string description = "", long priority = 5, AccountKind accountKind = AccountKind.Sink)
        {
            return new AccountDto()
            {
                Name = name,
                CategoryId = categoryId,
                Description = description,
                Priority = priority,
                AccountKind = accountKind
            };
        }

        private void AddAccounts(IAccountRepository repo, params AccountDto[] dtos)
        {
            foreach(var dto in dtos)
            {
                repo.Upsert(dto);            
            }
        }

        private void SetAccountState(IAccountStateRepository repo, AccountDto dto, Money funds, bool isClosed)
        {
            repo.Upsert(new AccountStateDto()
            {
                AccountId = dto.Id.Value,
                Funds = funds.InternalValue,
                IsClosed = isClosed,
                Timestamp = DateTime.Now
            });
        }
    }
}