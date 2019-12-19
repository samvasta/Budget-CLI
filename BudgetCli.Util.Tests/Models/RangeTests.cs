using System.Globalization;
using System;
using Xunit;
using BudgetCli.Util.Models;

namespace BudgetCli.Util.Tests.Models
{
    public class RangeTest
    {
        [Theory]
        [InlineData(0, 10, 5, true)]
        [InlineData(0, 10, 0, true)]
        [InlineData(0, 10, 10, true)]
        [InlineData(0, 10, 11, false)]
        [InlineData(0, 10, -1, false)]
        [InlineData(-10, 10, 0, true)]
        public void TestRangeInteger_InRange(int from, int to, int test, bool expectedInRange)
        {
            Range<int> intRange = new Range<int>(from, to);
            
            bool isInRange = intRange.IsInRange(test);

            Assert.Equal(expectedInRange, isInRange);
        }

        
        [Theory]
        [InlineData(10, 9)]
        [InlineData(int.MinValue+1, int.MinValue)]
        [InlineData(int.MaxValue, int.MaxValue-1)]
        [InlineData(0, -1)]
        public void TestRangeInteger_BadArgs(int from, int to)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                Range<int> intRange = new Range<int>(from, to);
            });
        }


        
        [Theory]
        [InlineData(0, 10, 5, true)]
        [InlineData(0, 10, 0, true)]
        [InlineData(0, 10, 10, true)]
        [InlineData(0, 10, 11, false)]
        [InlineData(0, 10, -1, false)]
        [InlineData(0.001, 0.002, 0.0015, true)]
        [InlineData(123456.789, 987654.321, 555555, true)]
        [InlineData(-123456.789, 123456.789, 0, true)]
        public void TestRangeDecimal_InRange(decimal from, decimal to, decimal test, bool expectedInRange)
        {
            Range<decimal> decimalRange = new Range<decimal>(from, to);
            
            bool isInRange = decimalRange.IsInRange(test);

            Assert.Equal(expectedInRange, isInRange);
        }

        
        [Theory]
        [InlineData(10, 9)]
        [InlineData(int.MinValue+1, int.MinValue)]
        [InlineData(int.MaxValue, int.MaxValue-1)]
        [InlineData(0, -1)]
        [InlineData(0.00000001, 0)]
        public void TestRangeDecimal_BadArgs(decimal from, decimal to)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                Range<decimal> decimalRange = new Range<decimal>(from, to);
            });
        }
        
        [Theory]
        [InlineData(2020,1,1,   2020,2,1,   2020,1,15, true)]
        [InlineData(2020,1,1,   2020,2,1,   2020,2,15, false)]
        [InlineData(2020,10,1,  2021,3,1,   2020,11,15, true)]
        [InlineData(2020,10,1,  2021,3,1,   2021,1,15, true)]
        public void TestRangeDate_InRange(int year1, int month1, int day1, int year2, int month2, int day2, int yearTest, int monthTest, int dayTest, bool expectedInRange)
        {
            DateTime from = new DateTime(year1, month1, day1);
            DateTime to = new DateTime(year2, month2, day2);
            DateTime test = new DateTime(yearTest, monthTest, dayTest);

            Range<DateTime> yearRange = new Range<DateTime>(from, to);
            
            bool isInRange = yearRange.IsInRange(test);

            Assert.Equal(expectedInRange, isInRange);
        }

        
        [Theory]
        [InlineData(2020,1,2,  2020,1,1)]
        [InlineData(2020,1,2,  2019,10,1)]
        public void TestRangeDate_BadArgs(int year1, int month1, int day1, int year2, int month2, int day2)
        {
            DateTime from = new DateTime(year1, month1, day1);
            DateTime to = new DateTime(year2, month2, day2);

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                Range<DateTime> yearRange = new Range<DateTime>(from, to);
            });
        }

        [Theory]
        [InlineData("[1,3]", 1, 3, true, true)]
        [InlineData("(1,3]", 1, 3, false, true)]
        [InlineData("[1,3)", 1, 3, true, false)]
        [InlineData("[-1,3]", -1, 3, true, true)]
        [InlineData("[-10,-3]", -10, -3, true, true)]
        [InlineData("20", 20, 20, true, true)]
        [InlineData("-20", -20, -20, true, true)]
        public void TestRange_ParseInt(string input, int from, int to, bool isFromInclusive, bool isToInclusive)
        {
            Range<int> range;
            bool successful = BudgetCli.Util.Models.Range<int>.TryParse(input, int.TryParse, out range);

            Assert.True(successful);
            Assert.Equal(from, range.From);
            Assert.Equal(to, range.To);
            Assert.Equal(isFromInclusive, range.IsFromInclusive);
            Assert.Equal(isToInclusive, range.IsToInclusive);
        }
        [Theory]
        [InlineData("[10,3]")]
        [InlineData("(10,3]")]
        [InlineData("[10,3)")]
        [InlineData("[$1,$3]")]
        [InlineData("[1,3.234]")]
        [InlineData("[-1,3.234]")]
        [InlineData("{1,3}")]
        [InlineData("[-1,3")]
        [InlineData("-1,3")]
        [InlineData("(-1,3(")]
        [InlineData("(,)")]
        [InlineData("[,]")]
        public void TestRange_ParseIntFails(string input)
        {
            Range<int> range;
            bool successful = BudgetCli.Util.Models.Range<int>.TryParse(input, int.TryParse, out range);

            Assert.False(successful);
        }        

        [Theory]
        [InlineData("[1.01,3.34]", 1.01, 3.34, true, true)]
        [InlineData("(1.66,3.34]", 1.66, 3.34, false, true)]
        [InlineData("[1.77,3.56)", 1.77, 3.56, true, false)]
        [InlineData("[-1.12,3.234]", -1.12, 3.234, true, true)]
        [InlineData("[-10.234,-3]", -10.234, -3, true, true)]
        [InlineData("20.66", 20.66, 20.66, true, true)]
        [InlineData("-20.3", -20.3, -20.3, true, true)]
        [InlineData(".3", 0.3, 0.3, true, true)]
        [InlineData("(.123, .456]", 0.123, 0.456, false, true)]
        public void TestRange_ParseDouble(string input, double from, double to, bool isFromInclusive, bool isToInclusive)
        {
            Range<double> range;
            bool successful = BudgetCli.Util.Models.Range<double>.TryParse(input, double.TryParse, out range);

            Assert.True(successful);
            Assert.Equal(from, range.From);
            Assert.Equal(to, range.To);
            Assert.Equal(isFromInclusive, range.IsFromInclusive);
            Assert.Equal(isToInclusive, range.IsToInclusive);
        }

        [Theory]
        [InlineData("[1, 3]", 1, 3, true, true)]
        [InlineData("(1, 3]", 1, 3, false, true)]
        [InlineData("[1, 3)", 1, 3, true, false)]
        [InlineData("[-1, 3]", -1, 3, true, true)]
        [InlineData("[-10, -3]", -10, -3, true, true)]
        [InlineData("20", 20, 20, true, true)]
        [InlineData("-20", -20, -20, true, true)]
        public void TestRange_ToStringInt(string expected, int from, int to, bool isFromInclusive, bool isToInclusive)
        {
            Range<int> range = new Range<int>(from, to, isFromInclusive, isToInclusive);

            string rangeStr = range.ToString();

            Assert.Equal(expected, rangeStr);
        }

        [Theory]
        [InlineData("[1.01, 3.34]", 1.01, 3.34, true, true)]
        [InlineData("(1.66, 3.34]", 1.66, 3.34, false, true)]
        [InlineData("[1.77, 3.56)", 1.77, 3.56, true, false)]
        [InlineData("[-1.12, 3.234]", -1.12, 3.234, true, true)]
        [InlineData("[-10.234, -3]", -10.234, -3, true, true)]
        [InlineData("20.66", 20.66, 20.66, true, true)]
        [InlineData("-20.3", -20.3, -20.3, true, true)]
        [InlineData("0.3", 0.3, 0.3, true, true)]
        [InlineData("(0.123, 0.456]", 0.123, 0.456, false, true)]
        public void TestRange_ToStringDouble(string expected, double from, double to, bool isFromInclusive, bool isToInclusive)
        {
            Range<double> range = new Range<double>(from, to, isFromInclusive, isToInclusive);

            string rangeStr = range.ToString();

            Assert.Equal(expected, rangeStr);
        }

        [Fact]
        public void TestRange_Equals()
        {
            Range<DateTime> range = new Range<DateTime>(new DateTime(2020, 4, 18), new DateTime(2021, 1, 11), false, false);
            Range<DateTime> range2 = new Range<DateTime>(new DateTime(2020, 4, 18), new DateTime(2021, 1, 11), false, false);

            Assert.Equal(range, range2);
        }

        [Fact]
        public void TestRange_HashCode()
        {
            Range<DateTime> range = new Range<DateTime>(new DateTime(2020, 4, 18), new DateTime(2021, 1, 11), false, false);
            Range<DateTime> range2 = new Range<DateTime>(new DateTime(2020, 4, 18), new DateTime(2021, 1, 11), false, false);

            Assert.Equal(range.GetHashCode(), range2.GetHashCode());
        }
    }
}