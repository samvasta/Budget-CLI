using System.ComponentModel.DataAnnotations;
using System.Xml.Xsl;
using System.Linq;
using System;
using BudgetCli.Core.Models;
using BudgetCli.Data.Enums;
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
        public void TestAccountEquals()
        {
            RepositoryBag repositoryBag = new RepositoryBag();
            Account account1 = new Account(1, "Test Account", 4, 10, AccountKind.Category, "Description", DateTime.Today, repositoryBag);
            Account account1_copy = new Account(1, "Test Account", 4, 10, AccountKind.Category, "Description", DateTime.Today, repositoryBag);
            Account account3 = new Account(2, "Test Account2", 2, 2, AccountKind.Sink, "Description2", DateTime.Today, repositoryBag);

            Assert.True(account1 == account1_copy);
            Assert.True(account1.Equals(account1_copy));
            Assert.True(account1.Equals((object)account1_copy));
            Assert.False(account1.Equals("not equal"));
            Assert.False(account1.Equals((object)null));
            Assert.False(account1.Equals(account3));
            Assert.False(account1.Equals(null));

            Assert.True((Account)null == (Account)null);
            Assert.False((Account)null == (Account)account1);
            Assert.False((Account)account1 == (Account)null);
            Assert.False(account1 == account3);
            
            Assert.False((Account)null != (Account)null);
            Assert.True((Account)null != (Account)account1);
            Assert.True((Account)account1 != (Account)null);
            Assert.True(account1 != account3);
        }
        
        [Fact]
        public void TestAccountGetHashCode()
        {
            RepositoryBag repositoryBag = new RepositoryBag();
            Account account1 = new Account(1, "Test Account", 4, 10, AccountKind.Category, "Description", DateTime.Today, repositoryBag);
            Account account1_copy = new Account(1, "Test Account", 4, 10, AccountKind.Category, "Description", DateTime.Today, repositoryBag);
            Account account3 = new Account(2, "Test Account2", 2, 2, AccountKind.Sink, "Description2", DateTime.Today, repositoryBag);

            Assert.Equal(account1.GetHashCode(), account1_copy.GetHashCode());
            Assert.NotEqual(account1.GetHashCode(), account3.GetHashCode());
        }

        [Fact]
        public void TestAccountAddNewConstructor_NoRepositoryBag()
        {
            Assert.Throws<ArgumentNullException>(() => 
            {
                Account account = new Account("Test Account", (Money)123.45, DateTime.Today, null);
            });
            Assert.Throws<ArgumentNullException>(() => 
            {
                Account account = new Account(2, "name", 3, 4, AccountKind.Sink, "", DateTime.Today, null);
            });
        }

        [Fact]
        public void TestAccountAddNewConstructor()
        {
            RepositoryBag repositoryBag = new RepositoryBag();
            Account account = new Account("Test Account", (Money)123.45, DateTime.Today, repositoryBag);

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
                Account account = new Account(accountDto.Id.Value, accountDto.Name, accountDto.CategoryId, accountDto.Priority, accountDto.AccountKind, accountDto.Description, DateTime.Today, repositories);

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

        [Fact]
        public void TestAccountProperties()
        {
                long id = 1;
                string name = "Test Account";
                long priority = 7;
                long? categoryId = null;
                string description = "description";
                AccountKind accountKind = AccountKind.Source;

                RepositoryBag repositories = new RepositoryBag();
                
                //Act
                Account account = new Account(id, name, categoryId, priority, accountKind, description, DateTime.Today, repositories);

            var properties = account.GetProperties().ToList();

            Assert.Equal(7, properties.Count);

            Assert.Contains(Account.PROP_ID, properties);
            Assert.Contains(Account.PROP_ACCOUNT_KIND, properties);
            Assert.Contains(Account.PROP_CATEGORY, properties);
            Assert.Contains(Account.PROP_CURRENT_STATE, properties);
            Assert.Contains(Account.PROP_DESCRIPTION, properties);
            Assert.Contains(Account.PROP_NAME, properties);
            Assert.Contains(Account.PROP_PRIORITY, properties);
        }

        [Fact]
        public void TestAccountPropertyValues()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository stateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);

                long id;
                string name = "Test Account";
                long priority = 7;
                long? categoryId = null;
                string description = "description";
                AccountKind accountKind = AccountKind.Source;

                Money initialFunds = 123.45;
                DateTime timestamp = DateTime.Now;

                AccountDto accountDto = new AccountDto()
                {
                    Name = name,
                    Priority = priority,
                    Description = description,
                    CategoryId = categoryId,
                    AccountKind = accountKind,
                };
                repo.Upsert(accountDto);
                id = accountDto.Id.Value;

                AccountStateDto stateDto = new AccountStateDto()
                {
                    AccountId = id,
                    Funds = initialFunds.InternalValue,
                    IsClosed = false,
                    Timestamp = timestamp,
                };
                stateRepo.Upsert(stateDto);


                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, stateRepo);
                
                //Act
                Account account = new Account(accountDto.Id.Value, accountDto.Name, accountDto.CategoryId, accountDto.Priority, accountDto.AccountKind, accountDto.Description, timestamp, repositories);

                var propertyValues = account.GetPropertyValues().ToList();

                AccountState state = DtoToModelTranslator.FromDto(stateDto, repositories);

                //Assert
                Assert.Equal(7, propertyValues.Count);

                Assert.Equal(id, propertyValues.First(x => x.Property.Equals(Account.PROP_ID)).Value);
                Assert.Equal(name, propertyValues.First(x => x.Property.Equals(Account.PROP_NAME)).Value);
                Assert.Equal(description, propertyValues.First(x => x.Property.Equals(Account.PROP_DESCRIPTION)).Value);
                Assert.Equal(accountKind, propertyValues.First(x => x.Property.Equals(Account.PROP_ACCOUNT_KIND)).Value);
                Assert.Equal(null, propertyValues.First(x => x.Property.Equals(Account.PROP_CATEGORY)).Value);
                Assert.Equal(priority, propertyValues.First(x => x.Property.Equals(Account.PROP_PRIORITY)).Value);
                Assert.Equal(state, propertyValues.First(x => x.Property.Equals(Account.PROP_CURRENT_STATE)).Value);
            }
        }
        
    }
}