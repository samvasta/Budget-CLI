using System;
using BudgetCli.Util.Enums;
using Humanizer;

namespace BudgetCli.Util.Utilities
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

        public static DateTime GetRelativeDate(DateTime date, int delta, TimeUnit unit)
        {
            switch(unit)
            {
                case TimeUnit.Day:
                    return date.Subtract(new TimeSpan(1, 0, 0, 0));
                case TimeUnit.Week:
                    return date.Subtract(new TimeSpan(7, 0, 0, 0));
                case TimeUnit.Month:
                    int monthIndex0 = date.Month-1;
                    monthIndex0 -= delta;
                    int year = date.Year;

                    //Ensure within 0-11 range, and in/decrement year accordingly
                    while(monthIndex0 < 0)
                    {
                        --year;
                        monthIndex0 += 12;
                    }
                    while(monthIndex0 > 11)
                    {
                        ++year;
                        monthIndex0 -= 12;
                    }
                    //Add one to year to get back to range 1-12
                    return new DateTime(year, monthIndex0+1, date.Day);
                case TimeUnit.Year:
                    return new DateTime(date.Year-delta, date.Month, date.Day);
                default:
                    throw new Exception("Cannot get relative date for this type of time unit: " + unit.Humanize());
            }
        }
    }
}