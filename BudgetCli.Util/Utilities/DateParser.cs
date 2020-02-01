using System.Text.RegularExpressions;
using System.Globalization;
using System;
using System.Collections.Generic;
using BudgetCli.Util.Models;
using BudgetCli.Util.Enums;

namespace BudgetCli.Util.Utilities
{
    public static class DateParser
    {
        private static readonly Dictionary<Name, DayOfWeek> _dayOfWeekNames;
        private static readonly Dictionary<Name, int> _monthNames;
        private static readonly Dictionary<Name, TimeUnit> _timeUnitNames;

        static DateParser()
        {
            _dayOfWeekNames = new Dictionary<Name, DayOfWeek>();
            _dayOfWeekNames.Add(new Name("monday", "mon"), DayOfWeek.Monday);
            _dayOfWeekNames.Add(new Name("tuesday", "tue"), DayOfWeek.Tuesday);
            _dayOfWeekNames.Add(new Name("wednesday", "wed"), DayOfWeek.Wednesday);
            _dayOfWeekNames.Add(new Name("thursday", "thu"), DayOfWeek.Thursday);
            _dayOfWeekNames.Add(new Name("friday", "fri"), DayOfWeek.Friday);
            _dayOfWeekNames.Add(new Name("saturday", "sat"), DayOfWeek.Saturday);
            _dayOfWeekNames.Add(new Name("sunday", "sun"), DayOfWeek.Sunday);

            _monthNames = new Dictionary<Name, int>();
            _monthNames.Add(new Name("january", "jan"), 1);
            _monthNames.Add(new Name("february", "feb"), 2);
            _monthNames.Add(new Name("march", "mar"), 3);
            _monthNames.Add(new Name("april", "apr"), 4);
            _monthNames.Add(new Name("may"), 5);
            _monthNames.Add(new Name("june", "jun"), 6);
            _monthNames.Add(new Name("july", "jul"), 7);
            _monthNames.Add(new Name("august", "aug"), 8);
            _monthNames.Add(new Name("september", "sep"), 9);
            _monthNames.Add(new Name("october", "oct"), 10);
            _monthNames.Add(new Name("november", "nov"), 11);
            _monthNames.Add(new Name("december", "dec"), 12);

            _timeUnitNames = new Dictionary<Name, TimeUnit>();
            _timeUnitNames.Add(new Name("days", "day", "d", "ds"), TimeUnit.Day);
            _timeUnitNames.Add(new Name("weeks", "week", "w", "wk", "wks"), TimeUnit.Week);
            _timeUnitNames.Add(new Name("months", "month", "m", "mos", "mo"), TimeUnit.Month);
            _timeUnitNames.Add(new Name("years", "year", "y", "yrs", "yr"), TimeUnit.Year);
        }

        public static bool TryParseDayOfWeek(string input, out DayOfWeek dayOfWeek)
        {
            foreach(KeyValuePair<Name, DayOfWeek> kvp in _dayOfWeekNames)
            {
                if(kvp.Key.Equals(input, StringComparison.CurrentCultureIgnoreCase))
                {
                    dayOfWeek = kvp.Value;
                    return true;
                }
            }

            dayOfWeek = DayOfWeek.Monday;
            return false;
        }

        public static bool TryParseMonth(string input, out int month)
        {
            foreach(KeyValuePair<Name, int> kvp in _monthNames)
            {
                if(kvp.Key.Equals(input, StringComparison.CurrentCultureIgnoreCase))
                {
                    month = kvp.Value;
                    return true;
                }
            }

            month = -1;
            return false;
        }

        public static bool TryParseTimeUnit(string input, out TimeUnit unit)
        {
            foreach(KeyValuePair<Name, TimeUnit> kvp in _timeUnitNames)
            {
                if(kvp.Key.Equals(input, StringComparison.CurrentCultureIgnoreCase))
                {
                    unit = kvp.Value;
                    return true;
                }
            }

            unit = TimeUnit.Day;
            return false;
        }

        public static bool TryParseDayOfMonth(string input, int year, int month, out int dayOfMonth)
        {
            int maxDays = DateTime.DaysInMonth(year, month);
            int day;
            if(int.TryParse(input, out day) && day <= maxDays && day > 0)
            {
                dayOfMonth = day;
                return true;
            }
            
            dayOfMonth = -1;
            return false;
        }
    }
}