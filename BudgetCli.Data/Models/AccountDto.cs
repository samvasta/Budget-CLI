using System;
using System.Net;
using System.Collections.Generic;
using BudgetCli.Data.Attributes;
using BudgetCli.Data.Enums;

namespace BudgetCli.Data.Models {

    public class AccountDto : IDbModel
    {
        [Persisted]
        public virtual long? Id { get; set; }

        [Persisted]
        public virtual string Name { get; set; }

        [Persisted]
        public virtual long? CategoryId { get; set; }

        [Persisted]
        public virtual long Priority { get; set; }

        [Persisted]
        public virtual AccountKind AccountKind { get; set; }
        
        [Persisted]
        public virtual string Description { get; set; }


        public override bool Equals(object obj)
        {
            if(obj is AccountDto other)
            {
                return this.Id.Equals(other.Id) &&
                       this.Name.Equals(other.Name) &&
                       this.CategoryId.Equals(other.CategoryId) &&
                       this.Priority.Equals(other.Priority) &&
                       this.AccountKind.Equals(other.AccountKind) &&
                       this.Description.Equals(other.Description);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, CategoryId, Priority, AccountKind, Description);
        }
    }

}