using System;
using System.Linq;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Tests.TestHarness;
using BudgetCli.Util.Logging;
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
    }
}