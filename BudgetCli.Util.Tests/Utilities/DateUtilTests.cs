using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using BudgetCli.Util.Utilities;
using BudgetCli.Util.Enums;

namespace BudgetCli.Util.Tests.Utilities
{
    public class DateUtilTests
    {
        [Theory]
        [InlineData(DayOfWeek.Monday, "2019-12-23")]
        [InlineData(DayOfWeek.Tuesday, "2019-12-24")]
        [InlineData(DayOfWeek.Wednesday, "2019-12-25")]
        [InlineData(DayOfWeek.Thursday, "2019-12-26")]
        [InlineData(DayOfWeek.Friday, "2019-12-27")]
        [InlineData(DayOfWeek.Saturday, "2019-12-28")]
        [InlineData(DayOfWeek.Sunday, "2019-12-22")]
        public void GetRelativeDayOfWeek(DayOfWeek dayOfWeek, string expectedOutputStr)
        {
            DateTime start = new DateTime(2020, 1, 1);  //A wednesday

            DateTime output = DateUtil.GetRelativeDateDayOfWeek(start, dayOfWeek);
            DateTime expected = DateTime.Parse(expectedOutputStr);
            Assert.Equal(expected, output);
        }

        [Theory]
        [InlineData("2020-1-1", -1, 0, 0, "2019-1-1")]
        [InlineData("2020-1-1", 0, -1, 0, "2019-12-1")]
        [InlineData("2020-1-1", 0, 0, -1, "2019-12-31")]
        [InlineData("2020-2-29", -1, 0, 0, "2019-2-28")]    //2020 is Leap Year
        [InlineData("2020-2-28", -1, 0, 0, "2019-2-28")]    //2020 is Leap Year
        public void GetRelativeDate(string startDateStr, int yearDiff, int monthDiff, int dayDiff, string expectedDateStr)
        {
            DateTime start = DateTime.Parse(startDateStr);
            DateTime expected = DateTime.Parse(expectedDateStr);
            DateTime actual = DateUtil.GetRelativeDate(start, yearDiff, monthDiff, dayDiff);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2020-1-1", -1, "2019-1-1")]
        [InlineData("2020-1-1", 2, "2022-1-1")]
        [InlineData("2020-1-1", 0, "2020-1-1")]
        public void GetRelativeDateByTimeUnit_Years(string startDateStr, int numUnits, string expectedDateStr)
        {
            DateTime start = DateTime.Parse(startDateStr);
            DateTime expected = DateTime.Parse(expectedDateStr);
            DateTime actual = DateUtil.GetRelativeDate(start, numUnits, TimeUnit.Year);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2020-1-1", -1, "2019-12-1")]
        [InlineData("2020-1-1", 2, "2020-3-1")]
        [InlineData("2020-1-1", 0, "2020-1-1")]
        [InlineData("2020-1-1", 13, "2021-2-1")]
        [InlineData("2020-1-1", -24, "2018-1-1")]
        public void GetRelativeDateByTimeUnit_Months(string startDateStr, int numUnits, string expectedDateStr)
        {
            DateTime start = DateTime.Parse(startDateStr);
            DateTime expected = DateTime.Parse(expectedDateStr);
            DateTime actual = DateUtil.GetRelativeDate(start, numUnits, TimeUnit.Month);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2020-1-1", -1, "2019-12-25")]
        [InlineData("2020-1-1", 2, "2020-1-15")]
        [InlineData("2020-1-1", 0, "2020-1-1")]
        public void GetRelativeDateByTimeUnit_Weeks(string startDateStr, int numUnits, string expectedDateStr)
        {
            DateTime start = DateTime.Parse(startDateStr);
            DateTime expected = DateTime.Parse(expectedDateStr);
            DateTime actual = DateUtil.GetRelativeDate(start, numUnits, TimeUnit.Week);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2020-1-1", -1, "2019-12-31")]
        [InlineData("2020-1-1", 2, "2020-1-3")]
        [InlineData("2020-1-1", 0, "2020-1-1")]
        [InlineData("2020-1-1", 366, "2021-1-1")]   //2020 is leap year so it has 366 days
        public void GetRelativeDateByTimeUnit_Days(string startDateStr, int numUnits, string expectedDateStr)
        {
            DateTime start = DateTime.Parse(startDateStr);
            DateTime expected = DateTime.Parse(expectedDateStr);
            DateTime actual = DateUtil.GetRelativeDate(start, numUnits, TimeUnit.Day);

            Assert.Equal(expected, actual);
        }
    }
}