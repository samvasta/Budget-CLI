using System.Globalization;
using System;
namespace BudgetCli.Util.Models
{
    public struct Money : IComparable<Money>
    {
        public static readonly Money None = (Money)0;

        private long _value;

        public long InternalValue { get { return _value; } }

        #region - Constructors & Conversions -
        public Money(byte value) : this((long)value, false) {}
        public Money(short value) : this((long)value, false) {}
        public Money(ushort value) : this((long)value, false) {}
        public Money(int value) : this((long)value, false) {}
        public Money(uint value) : this((long)value, false) {}
        public Money(ulong value) : this((long)value, false) {}
        public Money(long value) : this(value, false) {}

        public Money(float value) : this((double) value) {}
        public Money(double value) 
        {
            _value = ConvertToMoneyLong(value);
        }
        public Money(decimal value)
        {
            _value = ConvertToMoneyLong(value);
        }

        /// <param name="isInternalValue">False indicates that the moneyValue is in whole dollars</param>
        public Money(long moneyValue, bool isInternalValue)
        {
            if(isInternalValue)
            {
                //No need to convert. It's already a money value
                _value = moneyValue;
            }
            else
            {
                _value = ConvertToMoneyLong(moneyValue);
            }
        }

        public static implicit operator Money(byte value) { return new Money(value); }
        public static implicit operator Money(short value) { return new Money(value); }
        public static implicit operator Money(ushort value) { return new Money(value); }
        public static implicit operator Money(int value) { return new Money(value); }
        public static implicit operator Money(uint value) { return new Money(value); }
        public static implicit operator Money(ulong value) { return new Money(value); }
        public static implicit operator Money(long value) { return new Money(value); }
        public static implicit operator Money(float value) { return new Money(value); }
        public static implicit operator Money(double value)  { return new Money(value); }
        public static implicit operator Money(decimal value) { return new Money(value); }


        public static explicit operator decimal(Money money) { return money._value / 100m; }
        public static explicit operator double(Money money) { return money._value / 100d; }
        public static explicit operator float(Money money) { return money._value / 100f; }

        #endregion - Constructors & Conversions -

        #region - Private Helpers -

        private static long ConvertToMoneyLong(long value)
        {
            //Shift over 2 orders of magnitude (used for cents)
            return value * 100L;
        }
        private static long ConvertToMoneyLong(decimal value)
        {
            //Shift over 2 orders of magnitude (used for cents)
            return (long)Math.Round(value * 100m);
        }
        private static long ConvertToMoneyLong(double value)
        {
            //Shift over 2 orders of magnitude (used for cents)
            return (long)Math.Round(value * 100d);
        }

        #endregion - Private Helpers -

        #region - Arithmetic Operators +-*/% -

        //Money and Money
        public static Money operator +(Money left, Money right)
        {
            return new Money(left._value + right._value, true);
        }
        public static Money operator -(Money left, Money right)
        {
            return new Money(left._value - right._value, true);
        }
        public static Money operator *(Money left, decimal right)
        {
            decimal leftDecimal = (decimal)left;
            return new Money(leftDecimal * right);
        }
        public static Money operator *(decimal left, Money right)
        {
            decimal rightDecimal = (decimal)right;
            return new Money(left * rightDecimal);
        }
        public static Money operator /(Money left, decimal right)
        {
            decimal leftDecimal = (decimal)left;
            return new Money(leftDecimal / right);
        }
        public static Money operator /(decimal left, Money right)
        {
            decimal rightDecimal = (decimal)right;
            return new Money(left / rightDecimal);
        }
        public static Money operator %(Money left, Money right)
        {
            decimal leftDecimal = (decimal)left;
            decimal rightDecimal = (decimal)right;
            return new Money(leftDecimal % rightDecimal);
        }

        #endregion

        #region - Comparison Operators == != < > <= >= -

        public static bool operator ==(Money left, Money right)
        {
            return left._value == right._value;
        }
        public static bool operator !=(Money left, Money right)
        {
            return left._value != right._value;
        }
        public static bool operator <(Money left, Money right)
        {
            return left._value < right._value;
        }
        public static bool operator >(Money left, Money right)
        {
            return left._value > right._value;
        }
        public static bool operator <=(Money left, Money right)
        {
            return left._value <= right._value;
        }
        public static bool operator >=(Money left, Money right)
        {
            return left._value >= right._value;
        }

        #endregion - Comparison Operators -

        public long Dollars
        {
            get
            {
                return _value / 100L;
            }
        }

        public short Cents
        {
            get
            {
                return (short)(_value % 100L);
            }
        }

        public override string ToString()
        {
            decimal decimalValue = (decimal)this;
            return decimalValue.ToString("C2", CultureInfo.CurrentCulture);
        }

        public override bool Equals(object obj)
        {
            if(obj is Money other)
            {
                return other._value.Equals(this._value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool TryParse(string moneyStr, out Money value)
        {
            double doubleValue;
            if(double.TryParse(moneyStr, NumberStyles.Any, CultureInfo.CurrentCulture, out doubleValue))
            {
                value = doubleValue;
                return true;
            }
            value = None;
            return false;
        }

        public int CompareTo(Money other)
        {
            return this._value.CompareTo(other._value);
        }
    }
}