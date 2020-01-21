using System.Text.RegularExpressions;
using System;
using System.Linq;
using BudgetCli.Util.Utilities;
using BudgetCli.Util.Utilities;

namespace BudgetCli.Parser.Models
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