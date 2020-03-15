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
    public class AccountStateTests
    {
        [Fact]
        public void TestAccountStateEquals()
        {
            RepositoryBag repositoryBag = new RepositoryBag();
            DateTime timestamp = DateTime.Now;
            AccountState accountState1 = new AccountState(1, 2, 123.45, timestamp, false, repositoryBag);
            AccountState accountState1_copy = new AccountState(1, 2, 123.45, timestamp, false, repositoryBag);
            AccountState accountState3 = new AccountState(2, 4, 246.9, timestamp, true, repositoryBag);

            Assert.True(accountState1 == accountState1_copy);
            Assert.True(accountState1.Equals(accountState1_copy));
            Assert.True(accountState1.Equals((object)accountState1_copy));
            Assert.False(accountState1.Equals("not equal"));
            Assert.False(accountState1.Equals((object)null));
            Assert.False(accountState1.Equals(accountState3));
            Assert.False(accountState1.Equals(null));

            Assert.True((AccountState)null == (AccountState)null);
            Assert.False((AccountState)null == (AccountState)accountState1);
            Assert.False((AccountState)accountState1 == (AccountState)null);
            Assert.False(accountState1 == accountState3);
            
            Assert.False((AccountState)null != (AccountState)null);
            Assert.True((AccountState)null != (AccountState)accountState1);
            Assert.True((AccountState)accountState1 != (AccountState)null);
            Assert.True(accountState1 != accountState3);
        }
        
        [Fact]
        public void TestAccountStateGetHashCode()
        {
            RepositoryBag repositoryBag = new RepositoryBag();
            DateTime timestamp = DateTime.Now;
            AccountState accountState1 = new AccountState(1, 2, 123.45, timestamp, false, repositoryBag);
            AccountState accountState1_copy = new AccountState(1, 2, 123.45, timestamp, false, repositoryBag);
            AccountState accountState3 = new AccountState(2, 4, 246.9, timestamp, true, repositoryBag);

            Assert.Equal(accountState1.GetHashCode(), accountState1_copy.GetHashCode());
            Assert.NotEqual(accountState1.GetHashCode(), accountState3.GetHashCode());
        }

        [Fact]
        public void NewAccountState_NoRepositoryBag()
        {
            long id = 3;
            long accountId = 8;
            Money funds = 123.45;
            DateTime timestamp = DateTime.Now;
            bool isClosed = false;

            Assert.Throws<ArgumentNullException>(() =>
            {
                AccountState state = new AccountState(id, accountId, funds, timestamp, isClosed, null);
            });
        }
        
        [Fact]
        public void NewAccountState()
        {
            RepositoryBag repositoryBag = new RepositoryBag();
            long id = 3;
            long accountId = 8;
            Money funds = 123.45;
            DateTime timestamp = DateTime.Now;
            bool isClosed = false;

            AccountState state = new AccountState(id, accountId, funds, timestamp, isClosed, repositoryBag);

            Assert.Equal(id, state.Id);
            Assert.Equal(accountId, state.AccountId);
            Assert.Equal(funds, state.Funds);
            Assert.Equal(timestamp, state.Timestamp);
            Assert.Equal(isClosed, state.IsClosed);
        }
        
        [Fact]
        public void AccountStateToDto()
        {
            RepositoryBag repositoryBag = new RepositoryBag();
            long id = 3;
            long accountId = 8;
            Money funds = 123.45;
            DateTime timestamp = DateTime.Now;
            bool isClosed = false;

            AccountState state = new AccountState(id, accountId, funds, timestamp, isClosed, repositoryBag);

            AccountStateDto dto = state.ToDto();

            Assert.Equal(id, dto.Id);
            Assert.Equal(accountId, dto.AccountId);
            Assert.Equal(funds.InternalValue, dto.Funds);
            Assert.Equal(timestamp, dto.Timestamp);
            Assert.Equal(isClosed, dto.IsClosed);
        }

        [Fact]
        public void TestAccountStateProperties()
        {
            RepositoryBag repositoryBag = new RepositoryBag();
            long id = 3;
            long accountId = 8;
            Money funds = 123.45;
            DateTime timestamp = DateTime.Now;
            bool isClosed = false;

            AccountState state = new AccountState(id, accountId, funds, timestamp, isClosed, repositoryBag);
            var properties = state.GetProperties().ToList();

            Assert.Equal(5, properties.Count);

            Assert.Contains(AccountState.PROP_ACCOUNT, properties);
            Assert.Contains(AccountState.PROP_FUNDS, properties);
            Assert.Contains(AccountState.PROP_ID, properties);
            Assert.Contains(AccountState.PROP_STATUS, properties);
            Assert.Contains(AccountState.PROP_TIMESTAMP, properties);
        }

        [Fact]
        public void TestAccountStatePropertyValues()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository accountRepo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);

                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, accountRepo, accountStateRepo);

                var accountDto = new AccountDto(){
                    Name = "account",
                    Priority = 5,
                    AccountKind = AccountKind.Sink
                };
                accountRepo.Upsert(accountDto);

                long accountId = accountDto.Id.Value;
                Money funds = 123.45;
                DateTime timestamp = DateTime.Now;
                bool isClosed = false;
                
                AccountStateDto accountStateDto = new AccountStateDto(){
                    AccountId = accountId,
                    IsClosed = isClosed,
                    Funds = funds.InternalValue,
                    Timestamp = timestamp
                };
                accountStateRepo.Upsert(accountStateDto);

                long id = accountStateDto.Id.Value;


                //Act
                Account account = DtoToModelTranslator.FromDto(accountDto, DateTime.Today, repositories);
                AccountState state = DtoToModelTranslator.FromDto(accountStateDto, repositories);
                var propertyValues = state.GetPropertyValues().ToList();
                
                //Assert
                Assert.Equal(5, propertyValues.Count);

                Assert.Equal(id, propertyValues.First(x => x.Property.Equals(AccountState.PROP_ID)).Value);
                Assert.Equal(account, propertyValues.First(x => x.Property.Equals(AccountState.PROP_ACCOUNT)).Value);
                Assert.Equal(funds, propertyValues.First(x => x.Property.Equals(AccountState.PROP_FUNDS)).Value);
                Assert.Equal(isClosed, propertyValues.First(x => x.Property.Equals(AccountState.PROP_STATUS)).Value);
                Assert.Equal(timestamp, propertyValues.First(x => x.Property.Equals(AccountState.PROP_TIMESTAMP)).Value);
            }
        }
    }
}