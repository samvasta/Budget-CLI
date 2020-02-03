using System;
using BudgetCli.Util.Enums;
using Humanizer;

namespace BudgetCli.Util.Utilities
{
    public static class DateUtil
    {
        /// <summary>
        /// Calls <see cref="DateUtil.GetRelativeDateDayOfWeek()"/> starting from <see cref="DateTime.Today"/>
        /// </summary>
        public static DateTime GetRelativeDateDayOfWeek(DayOfWeek target)
        {
            return GetRelativeDateDayOfWeek(DateTime.Today, target);
        }

        /// <summary>
        /// Weeks start on Monday! Goes back to the sunday before startDate and continues stepping back until the appropriate day of week is found
        /// </summary>
        /// <returns></returns>
        public static DateTime GetRelativeDateDayOfWeek(DateTime startDate, DayOfWeek target)
        {
            int targetInt = (int)target;
            
            int currentDowInt = (int)startDate.DayOfWeek;

            //Add 1 because MSFT says weeks start on Sunday but I like when they start on Monday because it's called a weekEND not a weekSTART
            int deltaToStartOfCurrentWeek = currentDowInt - (int)DayOfWeek.Monday + 1;

            //Add 7 to get to start of last week
            return startDate.Subtract(new TimeSpan(deltaToStartOfCurrentWeek + 7 - targetInt, 0, 0, 0));
        }
        
        /// <summary>
        /// Calls <see cref="DateUtil.GetRelativeDateDayOfMonth()"/> starting from <see cref="DateTime.Today"/>
        /// </summary>
        public static DateTime GetRelativeDateDayOfMonth(int targetMonth, int targetDay)
        {
            return GetRelativeDateDayOfMonth(DateTime.Today, targetMonth, targetDay);
        }
        
        public static DateTime GetRelativeDateDayOfMonth(DateTime startDate, int targetMonth, int targetDay)
        {
            int targetYear = startDate.Year-1;

            targetDay = Math.Clamp(targetDay, 1, DateTime.DaysInMonth(targetYear, targetMonth));

            return new DateTime(targetYear, targetMonth, targetDay);
        }

        /// <summary>
        /// Calls <see cref="DateUtil.GetRelativeDate()"/> starting from <see cref="DateTime.Today"/>
        /// </summary>
        public static DateTime GetRelativeDate(int yearsDiff, int monthsDiff, int daysDiff)
        {
            return GetRelativeDate(DateTime.Today, yearsDiff, monthsDiff, daysDiff);
        }

        public static DateTime GetRelativeDate(DateTime start, int yearsDiff, int monthsDiff, int daysDiff)
        {
            return start.AddYears(yearsDiff).AddMonths(monthsDiff).AddDays(daysDiff);
        }

        public static DateTime GetRelativeDate(DateTime date, int delta, TimeUnit unit)
        {
            switch(unit)
            {
                case TimeUnit.Day:
                    return date.Add(new TimeSpan(1*delta, 0, 0, 0));
                case TimeUnit.Week:
                    return date.Add(new TimeSpan(7*delta, 0, 0, 0));
                case TimeUnit.Month:
                    int monthIndex0 = date.Month-1;
                    monthIndex0 += delta;
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
                    return new DateTime(date.Year+delta, date.Month, date.Day);
                default:
                    throw new Exception("Cannot get relative date for this type of time unit: " + unit.Humanize());
            }
        }
    }
}