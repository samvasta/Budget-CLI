using System;
using BudgetCli.Data.Attributes;
using BudgetCli.Data.Enums;

namespace BudgetCli.Data.Models
{
    public class CommandActionDto : IDbModel
    {
        [Persisted]
        public virtual long? Id { get; set; }

        [Persisted]
        public virtual CommandActionKind CommandActionKind { get; set; }
        
        [Persisted]
        public virtual string CommandText { get; set; }

        [Persisted]
        public virtual DateTime Timestamp { get; set; }

        [Persisted]
        public virtual bool IsExecuted { get; set; }

    }
}