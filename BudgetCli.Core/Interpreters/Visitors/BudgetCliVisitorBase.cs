using BudgetCli.Core.Grammar;

namespace BudgetCli.Core.Interpreters.Visitors
{
    public abstract class BudgetCliVisitorBase<T> : BudgetCliBaseVisitor<T>
    {
        public VisitorBag VisitorBag { get; internal set; }
    }
}