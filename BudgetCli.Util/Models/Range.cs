using System;
using System.Text;
using System.Text.RegularExpressions;

namespace BudgetCli.Util.Models
{
    /// <summary>
    /// Represents a range of values that can have upper and/or lower bounds
    /// </summary>
    public class Range<T> where T : System.IComparable<T>
    {
        public delegate bool ValueParser(string text, out T value); 

        /// <summary>
        /// Lower Bound
        /// </summary>
        public T From { get; }

        /// <summary>
        /// Upper Bound
        /// </summary>
        public T To { get; }

        public bool IsFromInclusive { get; }

        public bool IsToInclusive { get; }

        public Range(T from, T to, bool isFromInclusive = true, bool isToInclusive = true)
        {
            if(from.CompareTo(to) > 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(from)} must be greater or equal to {nameof(to)}");
            }
            
            From = from;
            To = to;
            IsFromInclusive = isFromInclusive;
            IsToInclusive = isToInclusive;
        }

        public bool IsInRange(T testValue)
        {
            int lowerComparison = testValue.CompareTo(From);
            int upperComparison = testValue.CompareTo(To);

            bool passesLowerBound = (IsFromInclusive && lowerComparison == 0) || lowerComparison > 0;
            bool passesUpperBound = (IsToInclusive && upperComparison == 0) || upperComparison < 0;

            return passesLowerBound && passesUpperBound;
        }

        public override string ToString()
        {
            int comparison = From.CompareTo(To);
            if(comparison == 0)
            {
                //From == To
                return From.ToString();
            }
            else if(comparison < 0)
            {
                //From < To
                StringBuilder sb = new StringBuilder();
                if(IsFromInclusive)
                {
                    sb.Append('[');
                }
                else
                {
                    sb.Append('(');
                }

                sb.Append(From.ToString()).Append(", ").Append(To.ToString());
                if(IsToInclusive)
                {
                    sb.Append(']');
                }
                else
                {
                    sb.Append(')');
                }
                return sb.ToString();
            }
            else
            {
                throw new ArgumentOutOfRangeException($"{nameof(From)} must be greater or equal to {nameof(To)}");
            }
        }

        public static bool TryParse(string text, ValueParser valueParser, out Range<T> range)
        {
            const string pattern = @"(\[|\()([^,]*)\s*,\s*([^\)\]]*)(\)|\])";

            var match = Regex.Match(text, pattern);
            if(match.Success)
            {
                string leftBracket = match.Groups[1].Value;
                string fromStr = match.Groups[2].Value;
                string toStr = match.Groups[3].Value;
                string rightBracket = match.Groups[4].Value;
                
                bool isFromInclusive = leftBracket.Equals("[");
                bool isToInclusive = rightBracket.Equals("]");
                T from;
                T to;
                if(valueParser(fromStr, out from) &&
                   valueParser(toStr, out to) &&
                   from.CompareTo(to) <= 0)
                {
                    range = new Range<T>(from, to, isFromInclusive, isToInclusive);
                    return true;
                }
            } 
            else
            {
                T value;
                bool isSuccessful = valueParser(text, out value);
                if(isSuccessful)
                {
                    range = new Range<T>(value, value);
                    return true;
                }
            }

            range = new Range<T>(default(T), default(T));
            return false;
        }
    }
}