using System;
using BudgetCli.Core.Exceptions;
using BudgetCli.Core.Interpreters.Visitors;
using BudgetCli.Core.Models;
using BudgetCli.Core.Models.Commands;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Interpreters
{
    public class VisitorBag
    {
        public readonly BudgetCliVisitorBase<InterpreterResult<ICommandAction>> CommandVisitor;
        public readonly BudgetCliVisitorBase<Range<DateTime>> DateRangeVisitor;
        public readonly BudgetCliVisitorBase<DateTime> DateVisitor;

        private VisitorBag(BudgetCliVisitorBase<InterpreterResult<ICommandAction>> commandVisitor,
                           BudgetCliVisitorBase<Range<DateTime>> dateRangeVisitor,
                           BudgetCliVisitorBase<DateTime> dateVisitor
                           )
        {
            CommandVisitor = commandVisitor;
            DateRangeVisitor = dateRangeVisitor;
            DateVisitor = dateVisitor;

            CommandVisitor.VisitorBag = this;
            DateRangeVisitor.VisitorBag = this;
            DateVisitor.VisitorBag = this;
        }
        
        public class VisitorBagBuilder
        {
            private BudgetCliVisitorBase<InterpreterResult<ICommandAction>> _commandVisitor;
            private BudgetCliVisitorBase<Range<DateTime>> _dateRangeVisitor;
            private BudgetCliVisitorBase<DateTime> _dateVisitor;

            public VisitorBagBuilder CommandVisitor(BudgetCliVisitorBase<InterpreterResult<ICommandAction>> visitor)
            {
                _commandVisitor = visitor;
                return this;
            }
            public VisitorBagBuilder DateRangeVisitor(BudgetCliVisitorBase<Range<DateTime>> visitor)
            {
                _dateRangeVisitor = visitor;
                return this;
            }
            public VisitorBagBuilder DateVisitor(BudgetCliVisitorBase<DateTime> visitor)
            {
                _dateVisitor = visitor;
                return this;
            }

            public VisitorBag Build()
            {
                if(_commandVisitor == null ||
                   _dateRangeVisitor == null ||
                   _dateVisitor == null)
                {
                    throw new BuilderException(typeof(VisitorBagBuilder), typeof(VisitorBag), "One or more visitors is null");
                }
                return new VisitorBag(_commandVisitor,
                                      _dateRangeVisitor,
                                      _dateVisitor);
            }
        }
    }
}