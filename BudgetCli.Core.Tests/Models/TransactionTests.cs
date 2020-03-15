using System.Linq;
using System;
using BudgetCli.Core.Models;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Tests.TestHarness;
using BudgetCli.Util.Logging;
using BudgetCli.Util.Models;
using Moq;
using Xunit;
using BudgetCli.Data.Enums;

namespace BudgetCli.Core.Tests.Models
{
    public class TransactionTests
    {
        [Fact]
        public void TestTransactionEquals()
        {
            RepositoryBag repositoryBag = new RepositoryBag();
            DateTime timestamp = DateTime.Now;
            Transaction transaction1 = new Transaction(1, timestamp, 1, 2, 123.45, "memo", repositoryBag);
            Transaction transaction1_copy = new Transaction(1, timestamp, 1, 2, 123.45, "memo", repositoryBag);
            Transaction transaction3 = new Transaction(2, timestamp, 4, 8, 246.9, "memo2", repositoryBag);

            Assert.True(transaction1 == transaction1_copy);
            Assert.True(transaction1.Equals(transaction1_copy));
            Assert.True(transaction1.Equals((object)transaction1_copy));
            Assert.False(transaction1.Equals("not equal"));
            Assert.False(transaction1.Equals((object)null));
            Assert.False(transaction1.Equals(transaction3));
            Assert.False(transaction1.Equals(null));

            Assert.True((Transaction)null == (Transaction)null);
            Assert.False((Transaction)null == (Transaction)transaction1);
            Assert.False((Transaction)transaction1 == (Transaction)null);
            Assert.False(transaction1 == transaction3);
            
            Assert.False((Transaction)null != (Transaction)null);
            Assert.True((Transaction)null != (Transaction)transaction1);
            Assert.True((Transaction)transaction1 != (Transaction)null);
            Assert.True(transaction1 != transaction3);
        }
        
        [Fact]
        public void TestTransactionGetHashCode()
        {
            RepositoryBag repositoryBag = new RepositoryBag();
            DateTime timestamp = DateTime.Now;
            Transaction transaction1 = new Transaction(1, timestamp, 1, 2, 123.45, "memo", repositoryBag);
            Transaction transaction1_copy = new Transaction(1, timestamp, 1, 2, 123.45, "memo", repositoryBag);
            Transaction transaction3 = new Transaction(2, timestamp, 4, 8, 246.9, "memo2", repositoryBag);

            Assert.Equal(transaction1.GetHashCode(), transaction1_copy.GetHashCode());
            Assert.NotEqual(transaction1.GetHashCode(), transaction3.GetHashCode());
        }

        [Fact]
        public void NewTransaction_NoRepositoryBag()
        {
            DateTime timestamp = DateTime.Now;
            long? sourceAccountId = 1;
            long? destAccountId = 3;
            Money transferAmount = 123.45;
            string memo = "memo";

            Assert.Throws<ArgumentNullException>(() =>
            {
                Transaction transaction = new Transaction(timestamp, sourceAccountId, destAccountId, transferAmount.InternalValue, memo, null);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                Transaction transaction = new Transaction(3, timestamp, sourceAccountId, destAccountId, transferAmount, memo, null);
            });
        }

        [Fact]
        public void NewTransaction()
        {
            DateTime timestamp = DateTime.Now;
            long? sourceAccountId = 1;
            long? destAccountId = 3;
            Money transferAmount = 123.45;
            string memo = "memo";

            RepositoryBag repositoryBag = new RepositoryBag();
            Transaction transaction = new Transaction(timestamp, sourceAccountId, destAccountId, transferAmount.InternalValue, memo, repositoryBag);

            Assert.Equal(timestamp, transaction.Timestamp);
            Assert.Equal(sourceAccountId, transaction.SourceAccountId);
            Assert.Equal(destAccountId, transaction.DestinationAccountId);
            Assert.Equal(transferAmount, transaction.TransferAmount);
            Assert.Equal(memo, transaction.Memo);
        }

        [Fact]
        public void TestTransactionProperties()
        {
            DateTime timestamp = DateTime.Now;
            long? sourceAccountId = 1;
            long? destAccountId = 3;
            Money transferAmount = 123.45;
            string memo = "memo";

            RepositoryBag repositoryBag = new RepositoryBag();
            Transaction transaction = new Transaction(timestamp, sourceAccountId, destAccountId, transferAmount.InternalValue, memo, repositoryBag);

            var properties = transaction.GetProperties().ToList();

            Assert.Contains(Transaction.PROP_ID, properties);
            Assert.Contains(Transaction.PROP_SOURCE, properties);
            Assert.Contains(Transaction.PROP_DEST, properties);
            Assert.Contains(Transaction.PROP_MEMO, properties);
            Assert.Contains(Transaction.PROP_TIMESTAMP, properties);
            Assert.Contains(Transaction.PROP_AMOUNT, properties);

            Assert.Equal(6, properties.Count);
        }

        [Fact]
        public void TestTransactionPropertyValues()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository accountRepo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);

                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, accountRepo, accountStateRepo);

                var accountDto1 = new AccountDto(){
                    Name = "source",
                    Priority = 5,
                    AccountKind = AccountKind.Sink
                };
                accountRepo.Upsert(accountDto1);
                var accountDto2 = new AccountDto(){
                    Name = "dest",
                    Priority = 5,
                    AccountKind = AccountKind.Sink
                };
                accountRepo.Upsert(accountDto2);
                accountStateRepo.Upsert(new AccountStateDto(){
                    AccountId = 1,
                    IsClosed = false,
                    Funds = 0,
                    Timestamp = DateTime.Now
                });
                accountStateRepo.Upsert(new AccountStateDto(){
                    AccountId = 2,
                    IsClosed = false,
                    Funds = 0,
                    Timestamp = DateTime.Now
                });

                Account account1 = DtoToModelTranslator.FromDto(accountDto1, DateTime.Today, repositories);
                Account account2 = DtoToModelTranslator.FromDto(accountDto2, DateTime.Today, repositories);

                long id = 1;
                DateTime timestamp = DateTime.Now;
                long? sourceAccountId = accountDto1.Id.Value;
                long? destAccountId = accountDto2.Id.Value;
                Money transferAmount = 123.45;
                string memo = "memo";

                //Act
                Transaction transaction = new Transaction(id, timestamp, sourceAccountId, destAccountId, transferAmount, memo, repositories);
                var propertyValues = transaction.GetPropertyValues().ToList();
                
                //Assert
                Assert.Equal(6, propertyValues.Count);

                Assert.Equal(id, propertyValues.First(x => x.Property.Equals(Transaction.PROP_ID)).Value);
                Assert.Equal(timestamp, propertyValues.First(x => x.Property.Equals(Transaction.PROP_TIMESTAMP)).Value);
                Assert.Equal(account1, propertyValues.First(x => x.Property.Equals(Transaction.PROP_SOURCE)).Value);
                Assert.Equal(account2, propertyValues.First(x => x.Property.Equals(Transaction.PROP_DEST)).Value);
                Assert.Equal(memo, propertyValues.First(x => x.Property.Equals(Transaction.PROP_MEMO)).Value);
                Assert.Equal(transferAmount, propertyValues.First(x => x.Property.Equals(Transaction.PROP_AMOUNT)).Value);
            }
        }
    }
}