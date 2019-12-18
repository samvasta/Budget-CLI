using System;
using BudgetCli.Data.Attributes;

namespace BudgetCli.Data.Models
{
    public class TransactionDto : IDbModel
    {
        [Persisted]
        public virtual long? Id { get; set; }

        [Persisted]
        public virtual DateTime Timestamp { get; set; }

        [Persisted]
        public virtual long? SourceAccountId { get; set; }

        [Persisted]
        public virtual long? DestinationAccountId { get; set; }

        [Persisted]
        public virtual long TransferAmount { get; set; }

        [Persisted]
        public virtual string Memo { get; set; }


        public override bool Equals(object obj)
        {
            if(obj is TransactionDto other)
            {
                return this.Id.Equals(other.Id) &&
                       this.Timestamp.Equals(other.Timestamp) &&
                       this.SourceAccountId.Equals(other.SourceAccountId) &&
                       this.DestinationAccountId.Equals(other.DestinationAccountId) &&
                       this.TransferAmount.Equals(other.TransferAmount) &&
                       this.Memo.Equals(other.Memo);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Timestamp, SourceAccountId, DestinationAccountId, TransferAmount, Memo);
        }
    }
}