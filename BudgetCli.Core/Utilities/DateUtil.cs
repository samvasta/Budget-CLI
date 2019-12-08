using System;

namespace BudgetCli.Core.Utilities
{
    public static class DateUtil
    {
        public static DateTime GetRelativeDateDayOfWeek(DayOfWeek target)
        {
            int targetInt = (int)target;
            
            int currentDowInt = (int)DateTime.Today.DayOfWeek;

            if(currentDowInt > targetInt)
            {
                int numDays = targetInt - currentDowInt;
                return DateTime.Today.AddDays(-numDays);
            }
            else if(currentDowInt < targetInt)
            {
                int numDays = currentDowInt + ((int)DayOfWeek.Saturday - targetInt);
                return DateTime.Today.AddDays(-numDays);
            }
            else
            {
                return DateTime.Today;
            }
        }
        
        public static DateTime GetRelativeDateDayOfMonth(int targetMonth)
        {
            DateTime now = DateTime.Today;

            int targetYear = DateTime.Today.Year-1;

            int targetDay = Math.Min(DateTime.DaysInMonth(targetYear, targetMonth), targetMonth);

            return new DateTime(targetYear, targetMonth, targetDay);
        }

        public static DateTime GetRelativeDate(int yearsDiff, int monthsDiff, int daysDiff)
        {
            return DateTime.Today.AddYears(yearsDiff).AddMonths(monthsDiff).AddDays(daysDiff);
        }
    }
}