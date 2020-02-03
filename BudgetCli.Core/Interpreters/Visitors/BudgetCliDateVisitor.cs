using System;
using Antlr4.Runtime.Misc;
using BudgetCli.Core.Enums;
using BudgetCli.Core.Exceptions;
using BudgetCli.Core.Grammar;
using BudgetCli.Core.Utilities;
using BudgetCli.Util.Enums;
using BudgetCli.Util.Utilities;

namespace BudgetCli.Core.Interpreters.Visitors
{
    public class BudgetCliDateVisitor : BudgetCliVisitorBase<DateTime>
    {
        public override DateTime VisitExplicitDate([NotNull] BudgetCliParser.ExplicitDateContext context)
        {
            int day = int.Parse(context.DATE_DAY().GetText());
            int month = int.Parse(context.DATE_MONTH().GetText());
            int year = DateTime.Now.Year;
            
            if(context.DATE_YEAR() != null)
            {
                year = int.Parse(context.DATE_YEAR().GetText());
            }
            
            return new DateTime(year, month, day);
        }

        public override DateTime VisitRelativeDayOfWeekDate([NotNull] BudgetCliParser.RelativeDayOfWeekDateContext context)
        {
            DayOfWeek target = context.dayOfWeek().Day;
            
            return DateUtil.GetRelativeDateDayOfWeek(target);
        }

        public override DateTime VisitRelativeDayOfMonthDate([NotNull] BudgetCliParser.RelativeDayOfMonthDateContext context)
        {
            Month target = context.month().Month;
            int day = int.Parse(context.DATE_DAY().GetText());
            return DateUtil.GetRelativeDateDayOfMonth((int)target, day);
        }

        public override DateTime VisitRelativeDate([NotNull] BudgetCliParser.RelativeDateContext context)
        {
            TimeUnit timeUnit = context.timeUnit().TimeUnit;
            int number = int.Parse(context.DIGITS().GetText());

            int yearDiff = 0;
            int monthDiff = 0;
            int dayDiff = 0;

            if(timeUnit == TimeUnit.Day)
            {
                dayDiff = -number;
            }
            else if(timeUnit == TimeUnit.Week)
            {
                dayDiff = -7*number;    //7 days per week
            }
            else if(timeUnit == TimeUnit.Month)
            {
                monthDiff = -number;
            }
            else if(timeUnit == TimeUnit.Year)
            {
                yearDiff = -number;
            }
            else
            {
                throw new BudgetCliParseException($"No such time unit defined: {Enum.GetName(typeof(TimeUnit), timeUnit)}");   
            }


            return DateUtil.GetRelativeDate(yearDiff, monthDiff, dayDiff);
        }
    }
}