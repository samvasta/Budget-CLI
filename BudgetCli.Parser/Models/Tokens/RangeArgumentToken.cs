using System;
using BudgetCli.Util.Models;

namespace BudgetCli.Parser.Models.Tokens
{
    public class RangeArgumentToken<T> : ArgumentToken where T : IComparable<T>
    {
        public RangeArgumentToken(string argumentName, bool isOptional) : base(argumentName, isOptional)
        {
        }

        public override TokenMatchResult Matches(string[] inputTokens, int startIdx)
        {
            throw new NotImplementedException();
        }
    }
}