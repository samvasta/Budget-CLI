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
    public class DtoToModelTranslatorTests
    {
        [Fact]
        public void FromDto_Account()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountDto accountDto = new AccountDto()
                {
                    Id = 1,
                    Name = "Test Account",
                    AccountKind = Data.Enums.AccountKind.Source,
                    CategoryId = null,
                    Description = "",
                    Priority = 7
                };

                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object);
                
                //Act
                Account account = DtoToModelTranslator.FromDto(accountDto, repositories);
                
                //Assert
                Assert.Equal("Test Account", account.Name);
                Assert.Null(account.CategoryId);
                Assert.Equal(7, account.Priority);
                Assert.Equal(Data.Enums.AccountKind.Source, account.AccountKind);
            }
        }

        
        [Fact]
        public void Account_ToDto()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object);
                AccountDto accountDto = new AccountDto()
                {
                    Id = 1,
                    Name = "Test Account",
                    AccountKind = AccountKind.Source,
                    CategoryId = null,
                    Description = "",
                    Priority = 7
                };

                Account account = new Account(1, "Test Account", null, 7, AccountKind.Source, "", repositories);

                //Act
                AccountDto toDto = account.ToDto();

                //Assert
                Assert.Equal(accountDto, toDto);
            }
        }

        
        [Fact]
        public void FromDto_Transaction()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object);

                
                DateTime timestamp = DateTime.Now;
                long? sourceAccountId = 1;
                long? destAccountId = 3;
                Money transferAmount = 123.45;
                string memo = "memo";

                TransactionDto transactionDto = new TransactionDto()
                {
                    Id = 1,
                    Timestamp = timestamp,
                    SourceAccountId = sourceAccountId,
                    DestinationAccountId = destAccountId,
                    TransferAmount = transferAmount.InternalValue,
                    Memo = memo
                };
                
                //Act
                Transaction transaction = DtoToModelTranslator.FromDto(transactionDto, repositories);
                
                //Assert
                Assert.Equal(timestamp, transaction.Timestamp);
                Assert.Equal(sourceAccountId, transaction.SourceAccountId);
                Assert.Equal(destAccountId, transaction.DestinationAccountId);
                Assert.Equal(transferAmount, transaction.TransferAmount);
                Assert.Equal(memo, transaction.Memo);
            }
        }
        
        [Fact]
        public void Transaction_ToDto()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object);

                
                DateTime timestamp = DateTime.Now;
                long? sourceAccountId = 1;
                long? destAccountId = 3;
                Money transferAmount = 123.45;
                string memo = "memo";

                TransactionDto transactionDto = new TransactionDto()
                {
                    Id = 1,
                    Timestamp = timestamp,
                    SourceAccountId = sourceAccountId,
                    DestinationAccountId = destAccountId,
                    TransferAmount = transferAmount.InternalValue,
                    Memo = memo
                };
                
                Transaction transaction = new Transaction(1, timestamp, sourceAccountId, destAccountId, transferAmount, memo, repositories);
                
                //Act
                TransactionDto toDto = transaction.ToDto();

                //Assert
                Assert.Equal(transactionDto, toDto);
            }
        }
    }
}