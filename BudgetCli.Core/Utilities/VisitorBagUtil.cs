using BudgetCli.Core.Interpreters;
using BudgetCli.Core.Interpreters.Visitors;

namespace BudgetCli.Core.Utilities
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