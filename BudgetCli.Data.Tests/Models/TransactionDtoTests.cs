using System;
using BudgetCli.Data.Models;
using Xunit;

namespace BudgetCli.Data.Tests.Models
{
    public class TransactionDtoTests
    {
        [Fact]
        public void TestTransactionDtoEquals()
        {
            DateTime timestamp = DateTime.Now;
            TransactionDto transaction1 = new TransactionDto()
            {
                Id = 1,
                SourceAccountId = 2,
                DestinationAccountId = 3,
                Timestamp = timestamp,
                TransferAmount = 12345,
                Memo = "memo"
            };
            TransactionDto transaction2 = new TransactionDto()
            {
                Id = 1,
                SourceAccountId = 2,
                DestinationAccountId = 3,
                Timestamp = timestamp,
                TransferAmount = 12345,
                Memo = "memo"
            };

            Assert.Equal(transaction1, transaction1);
            Assert.Equal(transaction1, transaction2);
        }

        
        [Fact]
        public void TestTransactionDtoEquals_Fails()
        {
            DateTime timestamp = DateTime.Now;
            TransactionDto transaction1 = new TransactionDto()
            {
                Id = 1,
                SourceAccountId = 2,
                DestinationAccountId = 3,
                Timestamp = timestamp,
                TransferAmount = 12345,
                Memo = "memo"
            };
            
            TransactionDto differentId = new TransactionDto()
            {
                Id = 2,
                SourceAccountId = 2,
                DestinationAccountId = 3,
                Timestamp = timestamp,
                TransferAmount = 12345,
                Memo = "memo"
            };
            
            TransactionDto differentSource = new TransactionDto()
            {
                Id = 1,
                SourceAccountId = 1,
                DestinationAccountId = 3,
                Timestamp = timestamp,
                TransferAmount = 12345,
                Memo = "memo"
            };
            
            TransactionDto differentDestination = new TransactionDto()
            {
                Id = 1,
                SourceAccountId = 2,
                DestinationAccountId = 1,
                Timestamp = timestamp,
                TransferAmount = 12345,
                Memo = "memo"
            };
            
            TransactionDto differentTimestamp = new TransactionDto()
            {
                Id = 1,
                SourceAccountId = 2,
                DestinationAccountId = 3,
                Timestamp = timestamp.AddMilliseconds(1),
                TransferAmount = 12345,
                Memo = "memo"
            };
            
            TransactionDto differentTransferAmount = new TransactionDto()
            {
                Id = 1,
                SourceAccountId = 2,
                DestinationAccountId = 3,
                Timestamp = timestamp,
                TransferAmount = 12346,
                Memo = "memo"
            };
            
            TransactionDto differentMemo = new TransactionDto()
            {
                Id = 1,
                SourceAccountId = 2,
                DestinationAccountId = 3,
                Timestamp = timestamp,
                TransferAmount = 12345,
                Memo = "memo2"
            };

            Assert.NotEqual(transaction1, differentId);
            Assert.NotEqual(transaction1, differentSource);
            Assert.NotEqual(transaction1, differentDestination);
            Assert.NotEqual(transaction1, differentTimestamp);
            Assert.NotEqual(transaction1, differentTransferAmount);
            Assert.NotEqual(transaction1, differentMemo);
        }

        
        [Fact]
        public void TestTransactionDtoGetHashCode()
        {
            DateTime timestamp = DateTime.Now;
            TransactionDto transaction1 = new TransactionDto()
            {
                Id = 1,
                SourceAccountId = 2,
                DestinationAccountId = 3,
                Timestamp = timestamp,
                TransferAmount = 12345,
                Memo = "memo"
            };
            TransactionDto transaction2 = new TransactionDto()
            {
                Id = 1,
                SourceAccountId = 2,
                DestinationAccountId = 3,
                Timestamp = timestamp,
                TransferAmount = 12345,
                Memo = "memo"
            };

            Assert.Equal(transaction1.GetHashCode(), transaction1.GetHashCode());
            Assert.Equal(transaction1.GetHashCode(), transaction2.GetHashCode());
        }

        
        [Fact]
        public void TestTransactionGetHashCode_Fails()
        {
            DateTime timestamp = DateTime.Now;
            TransactionDto transaction1 = new TransactionDto()
            {
                Id = 1,
                SourceAccountId = 2,
                DestinationAccountId = 3,
                Timestamp = timestamp,
                TransferAmount = 12345,
                Memo = "memo"
            };
            
            TransactionDto differentId = new TransactionDto()
            {
                Id = 2,
                SourceAccountId = 2,
                DestinationAccountId = 3,
                Timestamp = timestamp,
                TransferAmount = 12345,
                Memo = "memo"
            };
            
            TransactionDto differentSource = new TransactionDto()
            {
                Id = 1,
                SourceAccountId = 1,
                DestinationAccountId = 3,
                Timestamp = timestamp,
                TransferAmount = 12345,
                Memo = "memo"
            };
            
            TransactionDto differentDestination = new TransactionDto()
            {
                Id = 1,
                SourceAccountId = 2,
                DestinationAccountId = 1,
                Timestamp = timestamp,
                TransferAmount = 12345,
                Memo = "memo"
            };
            
            TransactionDto differentTimestamp = new TransactionDto()
            {
                Id = 1,
                SourceAccountId = 2,
                DestinationAccountId = 3,
                Timestamp = timestamp.AddMilliseconds(1),
                TransferAmount = 12345,
                Memo = "memo"
            };
            
            TransactionDto differentTransferAmount = new TransactionDto()
            {
                Id = 1,
                SourceAccountId = 2,
                DestinationAccountId = 3,
                Timestamp = timestamp,
                TransferAmount = 12346,
                Memo = "memo"
            };
            
            TransactionDto differentMemo = new TransactionDto()
            {
                Id = 1,
                SourceAccountId = 2,
                DestinationAccountId = 3,
                Timestamp = timestamp,
                TransferAmount = 12345,
                Memo = "memo2"
            };

            Assert.NotEqual(transaction1.GetHashCode(), differentId.GetHashCode());
            Assert.NotEqual(transaction1.GetHashCode(), differentSource.GetHashCode());
            Assert.NotEqual(transaction1.GetHashCode(), differentDestination.GetHashCode());
            Assert.NotEqual(transaction1.GetHashCode(), differentTimestamp.GetHashCode());
            Assert.NotEqual(transaction1.GetHashCode(), differentTransferAmount.GetHashCode());
            Assert.NotEqual(transaction1.GetHashCode(), differentMemo.GetHashCode());
        }
        
    }
}