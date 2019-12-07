using System;

namespace BudgetCliCore.Models
{
    /// <summary>
    /// Represents a range of values that can have upper and/or lower bounds
    /// </summary>
    public class Range<T> where T : System.IComparable<T>
    {
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
    }
}