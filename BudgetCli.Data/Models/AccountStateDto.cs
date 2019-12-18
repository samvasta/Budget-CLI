using System;
using BudgetCli.Data.Attributes;

namespace BudgetCli.Data.Models
{
    public class AccountStateDto : IDbModel
    {
        [Persisted]
        public virtual long? Id { get; set; }

        [Persisted]
        public virtual DateTime Timestamp { get; set; }

        [Persisted]
        public virtual long AccountId { get; set; }

        [Persisted]
        public virtual long Funds { get; set; }

        [Persisted]
        public virtual bool IsClosed { get; set; }


        public override bool Equals(object obj)
        {
            if(obj is AccountStateDto other)
            {
                return this.Id.Equals(other.Id) &&
                       this.Timestamp.Equals(other.Timestamp) &&
                       this.AccountId.Equals(other.AccountId) &&
                       this.Funds.Equals(other.Funds) &&
                       this.IsClosed.Equals(other.IsClosed);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Timestamp, AccountId, Funds, IsClosed);
        }
    }
}