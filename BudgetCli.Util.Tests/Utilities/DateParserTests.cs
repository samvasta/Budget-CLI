using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using BudgetCli.Util.Utilities;
using System.Text;
using BudgetCli.Util.Enums;

namespace BudgetCli.Util.Tests.Utilities
{
    public class DateParserTests
    {
        /// <summary>
        /// Randomly changes case of each letter in string.
        /// </summary>
        private string ScrambleCase(string input)
        {
            //Probably not a great way to do this but it's just a test
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            foreach(char c in input)
            {
                if(rand.NextDouble() < 0.5)
                {
                    sb.Append(char.ToUpper(c));
                }
                else
                {
                    sb.Append(char.ToLower(c));
                }
            } 
            return sb.ToString();
        }

        [Fact]
        public void TestScrambleCase()
        {
            string x = "asdlkjfwelrkj ;lkj; oiawejt kajitoauhlk jwhru8()*)*&0)(&(*";
            string scrambledCase = ScrambleCase(x);
            Assert.NotEqual(x, scrambledCase);
            Assert.Equal(x, scrambledCase.ToLower());
        }

        [Theory]
        [InlineData("monday", DayOfWeek.Monday)]
        [InlineData("mon", DayOfWeek.Monday)]
        [InlineData("tuesday", DayOfWeek.Tuesday)]
        [InlineData("tue", DayOfWeek.Tuesday)]
        [InlineData("wednesday", DayOfWeek.Wednesday)]
        [InlineData("wed", DayOfWeek.Wednesday)]
        [InlineData("thursday", DayOfWeek.Thursday)]
        [InlineData("thu", DayOfWeek.Thursday)]
        [InlineData("friday", DayOfWeek.Friday)]
        [InlineData("fri", DayOfWeek.Friday)]
        [InlineData("saturday", DayOfWeek.Saturday)]
        [InlineData("sat", DayOfWeek.Saturday)]
        [InlineData("sunday", DayOfWeek.Sunday)]
        [InlineData("sun", DayOfWeek.Sunday)]
        public void IsDayOfWeek(string input, DayOfWeek expected)
        {
            input = ScrambleCase(input);
            DayOfWeek dayOfWeek;
            bool success = DateParser.TryParseDayOfWeek(input, out dayOfWeek);

            Assert.True(success, input);
            Assert.Equal(expected, dayOfWeek);
        }
        

        [Theory]
        [InlineData("day", TimeUnit.Day)]
        [InlineData("days", TimeUnit.Day)]
        [InlineData("d", TimeUnit.Day)]
        [InlineData("ds", TimeUnit.Day)]
        [InlineData("weeks", TimeUnit.Week)]
        [InlineData("week", TimeUnit.Week)]
        [InlineData("wks", TimeUnit.Week)]
        [InlineData("wk", TimeUnit.Week)]
        [InlineData("w", TimeUnit.Week)]
        [InlineData("months", TimeUnit.Month)]
        [InlineData("month", TimeUnit.Month)]
        [InlineData("mo", TimeUnit.Month)]
        [InlineData("mos", TimeUnit.Month)]
        [InlineData("m", TimeUnit.Month)]
        [InlineData("years", TimeUnit.Year)]
        [InlineData("year", TimeUnit.Year)]
        [InlineData("yr", TimeUnit.Year)]
        [InlineData("yrs", TimeUnit.Year)]
        [InlineData("y", TimeUnit.Year)]
        public void IsTimeUnit(string input, TimeUnit expected)
        {
            input = ScrambleCase(input);
            TimeUnit unit;
            bool success = DateParser.TryParseTimeUnit(input, out unit);

            Assert.True(success, input);
            Assert.Equal(expected, unit);
        }

        
        [Theory]
        [InlineData("january", 1)]
        [InlineData("february", 2)]
        [InlineData("march", 3)]
        [InlineData("april", 4)]
        [InlineData("may", 5)]
        [InlineData("june", 6)]
        [InlineData("july", 7)]
        [InlineData("august", 8)]
        [InlineData("september", 9)]
        [InlineData("october", 10)]
        [InlineData("november", 11)]
        [InlineData("december", 12)]
        [InlineData("jan", 1)]
        [InlineData("feb", 2)]
        [InlineData("mar", 3)]
        [InlineData("apr", 4)]
        [InlineData("jun", 6)]
        [InlineData("jul", 7)]
        [InlineData("aug", 8)]
        [InlineData("sep", 9)]
        [InlineData("oct", 10)]
        [InlineData("nov", 11)]
        [InlineData("dec", 12)]
        public void IsMonth(string input, int expected)
        {
            input = ScrambleCase(input);
            int month;
            bool success = DateParser.TryParseMonth(input, out month);

            Assert.True(success, input);
            Assert.Equal(expected, month);
        }
    }
}