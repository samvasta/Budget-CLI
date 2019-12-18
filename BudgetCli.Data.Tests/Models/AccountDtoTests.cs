using System;
using BudgetCli.Data.Models;
using Xunit;

namespace BudgetCli.Data.Tests.Models
{
    public class AccountDtoTests
    {
        [Fact]
        public void TestAccountDtoEquals()
        {
            AccountDto account1 = new AccountDto()
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
                Name = "account",
                Priority = 5,
                CategoryId = null
            };

            Assert.Equal(account1, account1);
            Assert.Equal(account1, account2);
        }

        
        [Fact]
        public void TestAccountDtoEquals_Fails()
        {
            AccountDto account1 = new AccountDto()
            {
                Id = 1,
                AccountKind = Enums.AccountKind.Category,
                Description = String.Empty,
                Name = "account",
                Priority = 5,
                CategoryId = null
            };
            AccountDto differentId = new AccountDto()
            {
                Id = 2,
                AccountKind = Enums.AccountKind.Category,
                Description = String.Empty,
                Name = "account",
                Priority = 5,
                CategoryId = null
            };
            
            AccountDto differentAccountKind = new AccountDto()
            {
                Id = 1,
                AccountKind = Enums.AccountKind.Sink,
                Description = String.Empty,
                Name = "account",
                Priority = 5,
                CategoryId = null
            };
            AccountDto differentDescription = new AccountDto()
            {
                Id = 1,
                AccountKind = Enums.AccountKind.Category,
                Description = "test",
                Name = "account",
                Priority = 5,
                CategoryId = null
            };
            AccountDto differentName = new AccountDto()
            {
                Id = 1,
                AccountKind = Enums.AccountKind.Category,
                Description = String.Empty,
                Name = "account2",
                Priority = 5,
                CategoryId = null
            };
            AccountDto differentPriority = new AccountDto()
            {
                Id = 1,
                AccountKind = Enums.AccountKind.Category,
                Description = String.Empty,
                Name = "account",
                Priority = 6,
                CategoryId = null
            };
            AccountDto differentCategoryId = new AccountDto()
            {
                Id = 1,
                AccountKind = Enums.AccountKind.Category,
                Description = String.Empty,
                Name = "account",
                Priority = 5,
                CategoryId = 1
            };

            Assert.NotEqual(account1, differentId);
            Assert.NotEqual(account1, differentAccountKind);
            Assert.NotEqual(account1, differentDescription);
            Assert.NotEqual(account1, differentName);
            Assert.NotEqual(account1, differentPriority);
            Assert.NotEqual(account1, differentCategoryId);
        }

        
        [Fact]
        public void TestAccountDtoGetHashCode()
        {
            AccountDto account1 = new AccountDto()
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
                Name = "account",
                Priority = 5,
                CategoryId = null
            };

            Assert.Equal(account1.GetHashCode(), account1.GetHashCode());
            Assert.Equal(account1.GetHashCode(), account2.GetHashCode());
        }

        
        [Fact]
        public void TestAccountDtoGetHashCode_Fails()
        {
            AccountDto account1 = new AccountDto()
            {
                Id = 1,
                AccountKind = Enums.AccountKind.Category,
                Description = String.Empty,
                Name = "account",
                Priority = 5,
                CategoryId = null
            };
            AccountDto differentId = new AccountDto()
            {
                Id = 2,
                AccountKind = Enums.AccountKind.Category,
                Description = String.Empty,
                Name = "account",
                Priority = 5,
                CategoryId = null
            };
            
            AccountDto differentAccountKind = new AccountDto()
            {
                Id = 1,
                AccountKind = Enums.AccountKind.Sink,
                Description = String.Empty,
                Name = "account",
                Priority = 5,
                CategoryId = null
            };
            AccountDto differentDescription = new AccountDto()
            {
                Id = 1,
                AccountKind = Enums.AccountKind.Category,
                Description = "test",
                Name = "account",
                Priority = 5,
                CategoryId = null
            };
            AccountDto differentName = new AccountDto()
            {
                Id = 1,
                AccountKind = Enums.AccountKind.Category,
                Description = String.Empty,
                Name = "account2",
                Priority = 5,
                CategoryId = null
            };
            AccountDto differentPriority = new AccountDto()
            {
                Id = 1,
                AccountKind = Enums.AccountKind.Category,
                Description = String.Empty,
                Name = "account",
                Priority = 6,
                CategoryId = null
            };
            AccountDto differentCategoryId = new AccountDto()
            {
                Id = 1,
                AccountKind = Enums.AccountKind.Category,
                Description = String.Empty,
                Name = "account",
                Priority = 5,
                CategoryId = 1
            };

            Assert.NotEqual(account1.GetHashCode(), differentId.GetHashCode());
            Assert.NotEqual(account1.GetHashCode(), differentAccountKind.GetHashCode());
            Assert.NotEqual(account1.GetHashCode(), differentDescription.GetHashCode());
            Assert.NotEqual(account1.GetHashCode(), differentName.GetHashCode());
            Assert.NotEqual(account1.GetHashCode(), differentPriority.GetHashCode());
            Assert.NotEqual(account1.GetHashCode(), differentCategoryId.GetHashCode());
        }
    }
}