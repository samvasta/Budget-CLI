using System;

namespace BudgetCli.Data.Enums
{
    [Flags]
    public enum AccountKind
    {
        
        /// <summary>
        /// An account that contains sub-accounts.
        /// Category accounts cannot contain funds,
        /// but can provide metrics regarding their
        /// sub-accounts.
        /// </summary>
        Category = 1,
        
        /// <summary>
        /// An account which is typically a source of income.
        /// Money typically flows into this account from an
        /// outside source like salary.
        /// </summary>
        Source = 2,
        
        /// <summary>
        /// An account which is typically a sink of income.
        /// Money typically flows into this account to pay
        /// for debts or expenses 
        /// </summary>
        Sink = 4,
    }
}