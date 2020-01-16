using BudgetCli.Core.Interpreters;
using BudgetCli.Core.Interpreters.Visitors;
using BudgetCli.Data.Repositories;

namespace BudgetCli.Core.Utilities
{
    public static class VisitorBagUtil
    {
        public static VisitorBag GetRuntimeVisitorBag(RepositoryBag repositories)
        {
            return new VisitorBag.VisitorBagBuilder()
                .CommandVisitor(new BudgetCliCommandVisitor(repositories))
                .DateRangeVisitor(new BudgetCliDateRangeVisitor())
                .DateVisitor(new BudgetCliDateVisitor())
                .Build();
        }
    }
}