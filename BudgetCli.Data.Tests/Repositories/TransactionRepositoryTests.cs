using System;
using System.Collections.Generic;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Tests.TestHarness;
using BudgetCli.Util.Logging;
using Moq;
using Xunit;
using System.Linq;

namespace BudgetCli.Data.Tests.Repositories
{
    public class TransactionRepositoryTests
    {
        private Dictionary<string, long> SetupAccounts(AccountRepository repo, params string[] accountNames)
        {
            Dictionary<string, long> nameToIdDict = new Dictionary<string, long>();
            foreach(string name in accountNames)
            {
                long id = CreateAccount(repo, name);
                nameToIdDict.Add(name, id);
            }
            return nameToIdDict;
        }

        private long CreateAccount(AccountRepository repo, string accountName)
        {
            AccountDto dto = new AccountDto();
            dto.Name = accountName;
            repo.Upsert(dto);
            return dto.Id.Value;
        }

        [Fact]
        public void TestTransactionRepositoryInsertNew_NullSourceNullDest()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                
                Mock<TransactionDto> mockDto = new Mock<TransactionDto>();
                mockDto.SetupAllProperties();
                mockDto.SetupGet(t => t.SourceAccountId).Returns((long?)null);
                mockDto.SetupGet(t => t.DestinationAccountId).Returns((long?)null);
                //Skip set timestamp property

                var repo = new TransactionRepository(testDbInfo.ConnectionString, log.Object);

                //Act
                bool success = repo.Upsert(mockDto.Object);

                //Assert

                //Ensure update failed
                Assert.False(success);

                //Ensure failure was logged
                log.Verify(l => l.WriteLine(It.IsAny<string>(), LogLevel.Error));

                //Ensure ID was not set
                Assert.Null(mockDto.Object.Id);
            }
        }

        [Fact]
        public void TestTransactionRepositoryUpdate_NullSourceNullDest()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                AccountRepository accountRepo = new AccountRepository(testDbInfo.ConnectionString, log.Object);
                Dictionary<string, long> accountIds = SetupAccounts(accountRepo, "A1", "A2");
                
                Mock<TransactionDto> mockDto = new Mock<TransactionDto>();
                mockDto.SetupAllProperties();
                mockDto.SetupGet(t => t.TransferAmount).Returns(123_45);
                mockDto.SetupGet(t => t.Timestamp).Returns(DateTime.Today);

                mockDto.Object.SourceAccountId = accountIds["A1"];
                mockDto.Object.DestinationAccountId = accountIds["A2"];
                
                var repo = new TransactionRepository(testDbInfo.ConnectionString, log.Object);

                //First upsert will be an insert
                repo.Upsert(mockDto.Object);

                mockDto.Object.SourceAccountId = null;
                mockDto.Object.DestinationAccountId = null;

                //Act
                bool successful = repo.Upsert(mockDto.Object);

                //Assert update failed
                Assert.False(successful);

                //Ensure failure was logged
                log.Verify(l => l.WriteLine(It.IsAny<string>(), LogLevel.Error));
            }
        }
    }
}