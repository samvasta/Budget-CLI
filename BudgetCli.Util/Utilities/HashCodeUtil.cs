using System.Collections.Generic;

namespace BudgetCli.Util.Utilities
{
    public static class HashCodeUtil
    {
        /// <summary>
        /// Gets a hashcode for the entire list where order does not matter.
        /// Based on SO answer: https://stackoverflow.com/a/670068
        /// </summary>
        /// <returns>Combined hashcode of all elements in the list regardless of their order</returns>
        public static int GetOrderIndependentHashCode<T>(this IEnumerable<T> source)
        {
            int hash = 0;
            int curHash;
            int bitOffset = 0;
            // Stores number of occurences so far of each value.
            Dictionary<T, int> valueCounts = new Dictionary<T, int>();

            foreach (T element in source)
            {
                curHash = EqualityComparer<T>.Default.GetHashCode(element);
                if (valueCounts.TryGetValue(element, out bitOffset))
                {
                    valueCounts[element] = bitOffset + 1;
                }
                else
                {
                    valueCounts.Add(element, bitOffset);
                }

                // The current hash code is shifted (with wrapping) one bit
                // further left on each successive recurrence of a certain
                // value to widen the distribution.
                // 37 is an arbitrary low prime number that helps the
                // algorithm to smooth out the distribution.
                hash = unchecked(hash + ((curHash << bitOffset) | (curHash >> (32 - bitOffset))) * 37);
            }

            return hash;
        }
    }
}