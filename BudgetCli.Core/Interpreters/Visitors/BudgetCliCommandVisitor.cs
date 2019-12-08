using System;
using Antlr4.Runtime.Misc;
using BudgetCli.Core.Grammar;
using BudgetCli.Core.Models.Commands;

namespace BudgetCli.Core.Interpreters.Visitors
{
    public class BudgetCliCommandVisitor : BudgetCliVisitorBase<ICommandAction>
    {

        #region - Accounts -

        public virtual ICommandAction VisitCatAccounts([NotNull] BudgetCliParser.CatAccountsContext context)
        {
            throw new NotImplementedException();
        }

        public virtual ICommandAction VisitListAccounts([NotNull] BudgetCliParser.ListAccountsContext context)
        {
            throw new NotImplementedException();
        }

        public virtual ICommandAction VisitMoveAccount([NotNull] BudgetCliParser.MoveAccountContext context)
        {
            throw new NotImplementedException();
        }

        public virtual ICommandAction VisitNewAccount([NotNull] BudgetCliParser.NewAccountContext context)
        {
            throw new NotImplementedException();
        }

        public virtual ICommandAction VisitRemoveAccount([NotNull] BudgetCliParser.RemoveAccountContext context)
        {
            throw new NotImplementedException();
        }

        public virtual ICommandAction VisitSetAccount([NotNull] BudgetCliParser.SetAccountContext context)
        {
            throw new NotImplementedException();
        }

        #endregion - Accounts -

        #region - Transactions -

        public virtual ICommandAction VisitCatTransaction([NotNull] BudgetCliParser.CatTransactionContext context)
        {
            throw new NotImplementedException();
        }

        public virtual ICommandAction VisitListTransactions([NotNull] BudgetCliParser.ListTransactionsContext context)
        {
            throw new NotImplementedException();
        }

        public virtual ICommandAction VisitNewTransaction([NotNull] BudgetCliParser.NewTransactionContext context)
        {
            throw new NotImplementedException();
        }

        #endregion - Transactions -

        #region - Reporting -

        public virtual ICommandAction VisitListHistory([NotNull] BudgetCliParser.ListHistoryContext context)
        {
            throw new NotImplementedException();
        }

        #endregion - Reporting -
    }
}