using System;
using BudgetCli.Util.Models;
using Xunit;

namespace BudgetCli.Util.Tests.Models
{
    public class MoneyTests
    {
        [Fact]
        public void TestMoneyConstructors()
        {
            Money money = new Money((byte)100);
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = new Money((short)100);
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = new Money((ushort)100);
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = new Money((int)100);
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = new Money((uint)100);
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = new Money(100L);
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = new Money(100UL);
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = new Money(100f);
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = new Money(100d);
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = new Money(100m);
            Assert.Equal<decimal>(100.00m, (decimal)(money));
        }

        [Fact]
        public void TestMoneyConversions()
        {
            Money money = (byte)100;
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = (short)100;
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = (ushort)100;
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = (int)100;
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = (uint)100;
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = 100L;
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = 100UL;
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = 100f;
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = 100d;
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            
            money = 100m;
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            money = 99.99m;
            Assert.Equal<decimal>(99.99m, (decimal)(money));

            money = 99.999m;
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            money = 99.99d;
            Assert.Equal<decimal>(99.99m, (decimal)(money));

            money = 99.999d;
            Assert.Equal<decimal>(100.00m, (decimal)(money));

            money = 99.99f;
            Assert.Equal<decimal>(99.99m, (decimal)(money));

            money = 99.999f;
            Assert.Equal<decimal>(100.00m, (decimal)(money));

        }

        [Fact]
        public void TestMoneyEquals()
        {
            Money first = (Money)1.23;

            Assert.Equal(1.23, first);

            Money second = ((Money)1.00) + ((Money)0.23);
            Assert.Equal(first, second);


            Assert.False(first.Equals(null));
            Assert.False(first.Equals(DateTime.Now));
            Assert.False(first.Equals(new object()));
            Assert.False(first.Equals("1.23"));
        }

        [Fact]
        public void TestMoneyHashCode()
        {
            Money first = (Money)1.23;
            Money second = ((Money)1.00) + ((Money)0.23);

            Assert.Equal(first.GetHashCode(), second.GetHashCode());
        }
        
        [Theory]
        [InlineData("123.45", true, 123.45)]
        [InlineData("$123.45", true, 123.45)]
        [InlineData("$123,456.78", true, 123456.78)]
        [InlineData("123456.78", true, 123456.78)]
        [InlineData("123,456.78", true, 123456.78)]
        [InlineData("(123,456.78)", true, -123456.78)]
        [InlineData("123,456..78", false, 0)]
        [InlineData("abcde", false, 0)]
        [InlineData("12.34.56.778", false, 0)]
        public void TestMoneyTryParse(string input, bool expectedSuccess, double expectedValue)
        {
            Money money;
            bool success = Money.TryParse(input, out money);

            Assert.Equal(expectedSuccess, success);
            if(expectedSuccess)
            {
                Assert.Equal(expectedValue, (double)money);
            }
        }

        [Theory]
        [InlineData(1.00, 1.00, 2.00)]
        [InlineData(1.00, -1.00, 0.00)]
        [InlineData(-1.00, 1.00, 0.00)]
        [InlineData(1.23, 4.56, 5.79)]
        [InlineData(1_000_000_000.00, -1_000_000.00, 999_000_000.00)]
        public void TestMoney_Add(double left, double right, double expected)
        {
            Money leftMoney = new Money(left);
            Money rightMoney = new Money(right);

            Money result = leftMoney + rightMoney;

            Assert.Equal<double>(expected, (double)(result));
        }

        [Fact]
        public void TestMoney_Add_Conversions()
        {
            Money left = 123.45;
            double right = 123.45;
            double expected = 246.90;

            Assert.Equal<double>(expected, (double)(left + right));
            Assert.Equal<double>(expected, (double)(right + left));
        }

        [Theory]
        [InlineData(1.00, 1.00, 0.00)]
        [InlineData(100.23, 90.23, 10.00)]
        [InlineData(1.00, 0.10, 0.90)]
        public void TestMoney_Subtract(double left, double right, double expected)
        {
            Money leftMoney = new Money(left);
            Money rightMoney = new Money(right);

            Money result = leftMoney - rightMoney;

            Assert.Equal<double>(expected, (double)result);
        }

        [Theory]
        [InlineData(1.00, 1.00, 1.00)]
        [InlineData(0.01, 100.00, 1.00)]
        [InlineData(10000.00, 0.0001, 1)]
        public void TestMoney_Multiply(double left, double right, double expected)
        {
            Money leftMoney = new Money(left);

            Money result = leftMoney * (decimal)right;

            Assert.Equal<double>(expected, (double)result);
        }

        [Theory]
        [InlineData(1.00, 1.00, 1.00)]
        [InlineData(10000.00, 100000.00, 0.10)]
        [InlineData(1.00, 0.0001, 10000.00)]
        public void TestMoney_Divide_LeftMoney(double left, double right, double expected)
        {
            Money leftMoney = new Money(left);

            Money result = leftMoney / (decimal)right;

            Assert.Equal<double>(expected, (double)result);
        }

        [Theory]
        [InlineData(1.00, 1.00, 1.00)]
        [InlineData(10000.00, 100000.00, 0.10)]
        [InlineData(100.00, 100000.00, 0.00)]
        public void TestMoney_Divide_RightMoney(double left, double right, double expected)
        {
            Money rightMoney = new Money(right);

            Money result = (decimal)left / rightMoney;

            Assert.Equal<double>(expected, (double)result);
        }

        [Theory]
        [InlineData(1.00, 1.00, true)]
        [InlineData(1.00, 0.999, true)]
        [InlineData(1.00, 0.99, false)]
        public void TestMoney_Equals(double left, double right, bool expected)
        {
            Money leftMoney = new Money(left);
            Money rightMoney = new Money(right);

            bool result = leftMoney == rightMoney;

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1.00, 1.00, false)]
        [InlineData(1.00, 0.999, false)]
        [InlineData(1.00, 0.99, true)]
        public void TestMoney_NotEquals(double left, double right, bool expected)
        {
            Money leftMoney = new Money(left);
            Money rightMoney = new Money(right);

            bool result = leftMoney != rightMoney;

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1.00, 1.00, false)]
        [InlineData(1.00, 0.999, false)]
        [InlineData(0.99, 1.00, true)]
        [InlineData(1.00, 1.001, false)]
        public void TestMoney_LessThan(double left, double right, bool expected)
        {
            Money leftMoney = new Money(left);
            Money rightMoney = new Money(right);

            bool result = leftMoney < rightMoney;

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1.00, 1.00, false)]
        [InlineData(1.00, 1.001, false)]
        [InlineData(1.00, 0.99, true)]
        [InlineData(1.00, 0.999, false)]
        public void TestMoney_GreaterThan(double left, double right, bool expected)
        {
            Money leftMoney = new Money(left);
            Money rightMoney = new Money(right);

            bool result = leftMoney > rightMoney;

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1.00, 1.00, true)]
        [InlineData(1.00, 0.999, true)]
        [InlineData(1.00, 0.99, false)]
        [InlineData(1.00, 1.001, true)]
        public void TestMoney_LessOrEqual(double left, double right, bool expected)
        {
            Money leftMoney = new Money(left);
            Money rightMoney = new Money(right);

            bool result = leftMoney <= rightMoney;

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1.00, 1.00, true)]
        [InlineData(1.00, 0.999, true)]
        [InlineData(1.00, 0.99, true)]
        [InlineData(1.00, 1.001, true)]
        public void TestMoney_GreaterOrEqual(double left, double right, bool expected)
        {
            Money leftMoney = new Money(left);
            Money rightMoney = new Money(right);

            bool result = leftMoney >= rightMoney;

            Assert.Equal(expected, result);
        }

        

        [Theory]
        [InlineData(1.00, 1)]
        [InlineData(123.45, 123)]
        [InlineData(-123.45, -123)]
        [InlineData(0, 0)]
        [InlineData(0.34567, 0)]
        [InlineData(0.99, 0)]
        [InlineData(1.99, 1)]
        public void TestMoney_Dollars(double value, long expected)
        {
            Money money = (Money)value;

            long result = money.Dollars;

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1.00, 0)]
        [InlineData(123.45, 45)]
        [InlineData(-123.45, -45)]
        [InlineData(0, 0)]
        [InlineData(0.34567, 35)]
        [InlineData(0.99, 99)]
        [InlineData(1.99, 99)]
        public void TestMoney_Cents(double value, short expected)
        {
            Money money = (Money)value;

            short result = money.Cents;

            Assert.Equal(expected, result);
        }
    }
}