using System.Text.RegularExpressions;
using System;
using System.Linq;
using BudgetCli.Util.Utilities;
using System.Collections.Generic;

namespace BudgetCli.Util.Models
{
    public class Name : IEquatable<string>, IEquatable<Name>
    {
        public string Preferred { get; }
        public string[] Alternates { get; }
        private IOrderedEnumerable<string> AlternatesByLengthDesc { get; }

        public Name(string preferred, params string[] alternates)
        {
            if(string.IsNullOrWhiteSpace(preferred))
            {
                throw new ArgumentException($"{nameof(preferred)} cannot be empty or null.");
            }
            if(alternates.Any(x => string.IsNullOrWhiteSpace(x)))
            {
                throw new ArgumentException($"{nameof(alternates)} cannot have empty or null members.");
            }
            if(preferred.ContainsWhitespace())
            {
                throw new ArgumentException($"{nameof(preferred)} cannot contain any whitespace");
            }
            if(alternates.Any(x => x.ContainsWhitespace()))
            {
                throw new ArgumentException($"None of {nameof(alternates)} can contain whitespace.");
            }
            
            Preferred = preferred;
            Alternates = alternates;
            AlternatesByLengthDesc = Alternates.OrderByDescending(x => x.Length);
        }

        public bool IsIn(string text, out int index, out int length)
        {
            index = text.IndexOf(Preferred);
            if(index >= 0)
            {
                length = Preferred.Length;
                return true;
            }
            
            foreach(var alternate in AlternatesByLengthDesc)
            {
                index = text.IndexOf(alternate);
                if(index >= 0)
                {
                    length = alternate.Length;
                    return true;
                }
            }

            length = 0;
            return false;
        }

        public bool IsStartOf(string text, out int length)
        {
            if(text.StartsWith(Preferred))
            {
                length = Preferred.Length;
                return true;
            }
            
            foreach(var alternate in AlternatesByLengthDesc)
            {
                if(text.StartsWith(alternate))
                {
                    length = alternate.Length;
                    return true;
                }
            }

            length = 0;
            return false;
        }

        public string GetLongestMatch(string input, out int matchLength)
        {
            if(String.IsNullOrEmpty(input))
            {
                matchLength = 0;
                return String.Empty;
            }

            Dictionary<string, CharEnumerator> enumerators = new Dictionary<string, CharEnumerator>();
            enumerators.Add(Preferred, Preferred.GetEnumerator());
            foreach(string alt in Alternates)
            {
                enumerators.Add(alt, alt.GetEnumerator());
            }

            int charIdx = 0;

            CharEnumerator inputEnumerator = input.GetEnumerator();

            List<string> toRemove = new List<string>();
            while(enumerators.Any())
            {
                if(!inputEnumerator.MoveNext())
                {
                    //End of input string
                    break;
                }
                charIdx++;

                foreach(KeyValuePair<string, CharEnumerator> kvp in enumerators)
                {
                    if(kvp.Value.MoveNext())
                    {
                        if(kvp.Value.Current != inputEnumerator.Current)
                        {
                            //Character doesn't match input
                            toRemove.Add(kvp.Key);
                        }
                    }
                    else
                    {
                        //String is over. Remove from list
                        toRemove.Add(kvp.Key);
                    }
                }

                //Make sure to not remove the remaining keys 
                if(toRemove.Count > 0 && toRemove.Count < enumerators.Count)
                {
                    foreach(string keyToRemove in toRemove)
                    {
                        enumerators.Remove(keyToRemove);
                    }
                    toRemove.Clear();
                }
                else if(toRemove.Count == enumerators.Count)
                {
                    //All remaining keys should be removed. No reason to keep going
                    //Decrement charIdx because the latest idx didn't match anything
                    charIdx--;
                    break;
                }
            }

            matchLength = charIdx;
            return enumerators.Keys.OrderByDescending(x => x.Length).First();
        }

        public override bool Equals(object obj)
        {
            if(obj is Name otherName)
            {
                return Equals(otherName);
            }
            if(obj is string otherString)
            {
                return Equals(otherString);
            }
            return false;
        }

        public bool Equals(string other)
        {
            if(Preferred.Equals(other))
            {
                return true;
            }
            foreach(var alt in Alternates)
            {
                if(alt.Equals(other))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Equals(string other, StringComparison stringComparison)
        {
            if(Preferred.Equals(other, stringComparison))
            {
                return true;
            }
            foreach(var alt in Alternates)
            {
                if(alt.Equals(other, stringComparison))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Equals(Name other)
        {
            if(!this.Preferred.Equals(other.Preferred))
            {
                return false;
            }

            if(this.Alternates.Length != other.Alternates.Length)
            {
                return false;
            }
            
            var intersection = this.Alternates.Intersect(other.Alternates);

            return intersection.Count() == Alternates.Length;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Preferred, Alternates.GetOrderIndependentHashCode());
        }
    }
}