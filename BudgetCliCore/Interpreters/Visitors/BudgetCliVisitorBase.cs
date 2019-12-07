using BudgetCliCore.Grammar;

namespace BudgetCliCore.Interpreters.Visitors
{
    public abstract class BudgetCliVisitorBase<T> : BudgetCliBaseVisitor<T>
    {
        public VisitorBag VisitorBag { get; internal set; }
    }
}