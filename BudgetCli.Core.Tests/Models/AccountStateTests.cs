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
            AccountState accountState1 = new AccountState(1, 2, 123.45, timestamp, false);
            AccountState accountState1_copy = new AccountState(1, 2, 123.45, timestamp, false);
            AccountState accountState3 = new AccountState(2, 4, 246.9, timestamp, true);

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
            AccountState accountState1 = new AccountState(1, 2, 123.45, timestamp, false);
            AccountState accountState1_copy = new AccountState(1, 2, 123.45, timestamp, false);
            AccountState accountState3 = new AccountState(2, 4, 246.9, timestamp, true);

            Assert.Equal(accountState1.GetHashCode(), accountState1_copy.GetHashCode());
            Assert.NotEqual(accountState1.GetHashCode(), accountState3.GetHashCode());
        }
        
    }
}