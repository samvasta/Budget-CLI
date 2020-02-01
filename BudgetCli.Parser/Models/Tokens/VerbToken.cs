using System;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Util.Models;

namespace BudgetCli.Parser.Models.Tokens
{
    public class VerbToken : ICommandToken
    {
        public TokenKind Kind { get { return TokenKind.Verb; } }

        public bool IsOptional { get { return false; } }

        public Name Name { get; }

        public string Description { get { return Name.Preferred; } }

        public string[] PossibleValues { get; }

        public VerbToken(Name name)
        {
            Name = name;
            PossibleValues = new string[] {};
        }

        public TokenMatchResult Matches(string[] inputTokens, int startIdx)
        {
            if(startIdx >= inputTokens.Length || startIdx < 0)
            {
                return TokenMatchResult.None;
            }

            int matchLength;
            string match = Name.GetLongestMatch(inputTokens[startIdx], out matchLength);
            
            if(match.Length == matchLength)
            {
                return new TokenMatchResult(this, inputTokens[startIdx], MatchOutcome.Full, matchLength, 1);
            }
            else if(matchLength > 0)
            {
                return new TokenMatchResult(this, inputTokens[startIdx], MatchOutcome.Partial, matchLength, 0);
            }
            return new TokenMatchResult(this, String.Empty, MatchOutcome.None, 0, 0);
        }
    }
}