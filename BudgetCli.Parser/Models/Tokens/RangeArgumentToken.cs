using System;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Util;
using BudgetCli.Util.Models;

namespace BudgetCli.Parser.Models.Tokens
{
    public class RangeArgumentToken<T> : ArgumentToken where T : IComparable<T>
    {
        public Range<T>.TryParseValue ValueParser { get; }

        public RangeArgumentToken(string argumentName, bool isOptional, Range<T>.TryParseValue valueParser) : base(argumentName, isOptional)
        {
            ValueParser = valueParser;
        }

        public override TokenMatchResult Matches(string[] inputTokens, int startIdx)
        {
            for(int len = 1; len <= inputTokens.Length - startIdx; len++)
            {
                string combinedStr = TokenUtils.GetMatchText(inputTokens, startIdx, len);
                if(Range<T>.TryParse(combinedStr, ValueParser, out Range<T> range))
                {
                    var result = new TokenMatchResult(this, combinedStr, combinedStr, MatchOutcome.Full, combinedStr.Length, len);
                    result.SetArgValue(this, range);
                    return result;
                }
            }

            return TokenMatchResult.None;
        }
    }
}