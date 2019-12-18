using System;
using BudgetCli.Data.Models;
using Xunit;

namespace BudgetCli.Data.Tests.Models
{
    public class AccountStateDtoTests
    {
        [Fact]
        public void TestAccountStateDtoEquals()
        {
            DateTime timestamp = DateTime.Now;
            AccountStateDto state1 = new AccountStateDto()
            {
                Id = 1,
                AccountId = 1,
                Funds = 12345,
                IsClosed = false,
                Timestamp = timestamp
            };

            AccountStateDto state2 = new AccountStateDto()
            {
                Id = 1,
                AccountId = 1,
                Funds = 12345,
                IsClosed = false,
                Timestamp = timestamp
            };

            Assert.Equal(state1, state1);
            Assert.Equal(state1, state2);
        }

        
        [Fact]
        public void TestAccountStateDtoEquals_Fails()
        {
            DateTime timestamp = DateTime.Now;
            AccountStateDto state1 = new AccountStateDto()
            {
                Id = 1,
                AccountId = 1,
                Funds = 12345,
                IsClosed = false,
                Timestamp = timestamp
            };

            AccountStateDto differentId = new AccountStateDto()
            {
                Id = 2,
                AccountId = 1,
                Funds = 12345,
                IsClosed = false,
                Timestamp = timestamp
            };

            AccountStateDto differentAccountId = new AccountStateDto()
            {
                Id = 1,
                AccountId = 2,
                Funds = 12345,
                IsClosed = false,
                Timestamp = timestamp
            };

            AccountStateDto differentFunds = new AccountStateDto()
            {
                Id = 1,
                AccountId = 1,
                Funds = 12346,
                IsClosed = false,
                Timestamp = timestamp
            };

            AccountStateDto differentIsClosed = new AccountStateDto()
            {
                Id = 1,
                AccountId = 1,
                Funds = 12345,
                IsClosed = true,
                Timestamp = timestamp
            };

            AccountStateDto differentTimestamp = new AccountStateDto()
            {
                Id = 1,
                AccountId = 1,
                Funds = 12345,
                IsClosed = false,
                Timestamp = timestamp.AddMilliseconds(1)
            };

            Assert.NotEqual(state1, differentId);
            Assert.NotEqual(state1, differentAccountId);
            Assert.NotEqual(state1, differentFunds);
            Assert.NotEqual(state1, differentIsClosed);
            Assert.NotEqual(state1, differentTimestamp);
        }

        
        [Fact]
        public void TestAccountStateDtoGetHashCode()
        {
            DateTime timestamp = DateTime.Now;
            AccountStateDto state1 = new AccountStateDto()
            {
                Id = 1,
                AccountId = 1,
                Funds = 12345,
                IsClosed = false,
                Timestamp = timestamp
            };

            AccountStateDto state2 = new AccountStateDto()
            {
                Id = 1,
                AccountId = 1,
                Funds = 12345,
                IsClosed = false,
                Timestamp = timestamp
            };

            Assert.Equal(state1.GetHashCode(), state1.GetHashCode());
            Assert.Equal(state1.GetHashCode(), state2.GetHashCode());
        }

        
        [Fact]
        public void TestAccountStateDtoGetHashCode_Fails()
        {
            DateTime timestamp = DateTime.Now;
            AccountStateDto state1 = new AccountStateDto()
            {
                Id = 1,
                AccountId = 1,
                Funds = 12345,
                IsClosed = false,
                Timestamp = timestamp
            };

            AccountStateDto differentId = new AccountStateDto()
            {
                Id = 2,
                AccountId = 1,
                Funds = 12345,
                IsClosed = false,
                Timestamp = timestamp
            };

            AccountStateDto differentAccountId = new AccountStateDto()
            {
                Id = 1,
                AccountId = 2,
                Funds = 12345,
                IsClosed = false,
                Timestamp = timestamp
            };

            AccountStateDto differentFunds = new AccountStateDto()
            {
                Id = 1,
                AccountId = 1,
                Funds = 12346,
                IsClosed = false,
                Timestamp = timestamp
            };

            AccountStateDto differentIsClosed = new AccountStateDto()
            {
                Id = 1,
                AccountId = 1,
                Funds = 12345,
                IsClosed = true,
                Timestamp = timestamp
            };

            AccountStateDto differentTimestamp = new AccountStateDto()
            {
                Id = 1,
                AccountId = 1,
                Funds = 12345,
                IsClosed = false,
                Timestamp = timestamp.AddMilliseconds(1)
            };

            Assert.NotEqual(state1.GetHashCode(), differentId.GetHashCode());
            Assert.NotEqual(state1.GetHashCode(), differentAccountId.GetHashCode());
            Assert.NotEqual(state1.GetHashCode(), differentFunds.GetHashCode());
            Assert.NotEqual(state1.GetHashCode(), differentIsClosed.GetHashCode());
            Assert.NotEqual(state1.GetHashCode(), differentTimestamp.GetHashCode());
        }
        
    }
}