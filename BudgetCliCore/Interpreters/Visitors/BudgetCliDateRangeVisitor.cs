using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using BudgetCliCore.Grammar;
using BudgetCliCore.Models;

namespace BudgetCliCore.Interpreters.Visitors
{
    public class BudgetCliDateRangeVisitor: BudgetCliVisitorBase<Range<DateTime>>
    {
        public override Range<DateTime> VisitOptDateExpr([NotNull] BudgetCliParser.OptDateExprContext context)
        {
            return VisitDateExpr(context.dateExpr());
        }

        public override Range<DateTime> VisitDateExpr([NotNull] BudgetCliParser.DateExprContext context)
        {
            DateTime from = VisitorBag.DateVisitor.Visit(context.from).Date;
            DateTime to = VisitorBag.DateVisitor.Visit(context.to).Date;
            Range<DateTime> range = new Range<DateTime>(from, to);

            return range;
        }
    }
}