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
        public virtual long InitialFunds { get; set; }

        [Persisted]
        public virtual long Priority { get; set; }

        [Persisted]
        public virtual AccountKind AccountKind { get; set; }
        
        [Persisted]
        public virtual string Description { get; set; }

        public virtual List<long> StateIds { get; set; }
    }

}