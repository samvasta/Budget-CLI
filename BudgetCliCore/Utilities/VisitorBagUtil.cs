using BudgetCliCore.Interpreters;
using BudgetCliCore.Interpreters.Visitors;

namespace BudgetCliCore.Utilities
{
    public static class VisitorBagUtil
    {
        public static VisitorBag GetRuntimeVisitorBag()
        {
            return new VisitorBag.VisitorBagBuilder()
                .CommandVisitor(new BudgetCliCommandVisitor())
                .DateRangeVisitor(new BudgetCliDateRangeVisitor())
                .DateVisitor(new BudgetCliDateVisitor())
                .Build();
        }
    }
}