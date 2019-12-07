using System;
using BudgetCliData.Attributes;

namespace BudgetCliData.Models
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
    }
}