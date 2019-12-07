using System;
using BudgetCliData.Attributes;
using BudgetCliData.Enums;

namespace BudgetCliData.Models
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