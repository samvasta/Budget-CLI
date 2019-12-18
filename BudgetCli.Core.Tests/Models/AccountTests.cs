using System;
using BudgetCli.Core.Models;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Tests.TestHarness;
using BudgetCli.Util.Logging;
using BudgetCli.Util.Models;
using Moq;
using Xunit;

namespace BudgetCli.Core.Tests.Models
{
    public class AccountTests
    {

        [Fact]
        public void TestAccountAddNewConstructor()
        {
            RepositoryBag repositoryBag = new RepositoryBag();
            Account account = new Account("Test Account", (Money)123.45, repositoryBag);

            Assert.Equal("Test Account", account.Name);
            Assert.Null(account.Category);
            Assert.Equal(Account.DEFAULT_PRIORITY, account.Priority);
            Assert.Equal(Account.DEFAULT_ACCOUNT_KIND, account.AccountKind);
        }

        [Fact]
        public void TestAccountAddFromDtoConstructor()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);

                AccountDto categoryDto = new AccountDto()
                {
                    Name = "Test Category",
                    AccountKind = Data.Enums.AccountKind.Category,
                    CategoryId = null,
                    Description = "",
                    Priority = 3
                };
                repo.Upsert(categoryDto);

                AccountDto accountDto = new AccountDto()
                {
                    Name = "Test Account",
                    AccountKind = Data.Enums.AccountKind.Source,
                    CategoryId = categoryDto.Id,
                    Description = "",
                    Priority = 7
                };
                repo.Upsert(accountDto);

                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo);
                
                //Act
                Account account = new Account(accountDto.Id.Value, accountDto.Name, accountDto.CategoryId, accountDto.Priority, accountDto.AccountKind, accountDto.Description, repositories);

                Account category = account.Category;
                
                //Assert
                Assert.Equal("Test Account", account.Name);
                Assert.Equal(categoryDto.Id, account.CategoryId);
                Assert.Equal(7, account.Priority);
                Assert.Equal(Data.Enums.AccountKind.Source, account.AccountKind);


                Assert.Equal("Test Category", category.Name);
                Assert.Null(category.CategoryId);
                Assert.Equal(3, category.Priority);
                Assert.Equal(Data.Enums.AccountKind.Category, category.AccountKind);

            }
        }
        
    }
}