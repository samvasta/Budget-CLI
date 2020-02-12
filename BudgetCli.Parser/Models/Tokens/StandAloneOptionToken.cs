using System;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Util.Models;

namespace BudgetCli.Parser.Models.Tokens
{
    public class StandAloneOptionToken : ICommandToken
    {
        public TokenKind Kind { get { return TokenKind.StandAloneOption; } }

        public bool IsOptional { get { return true; } }

        public Name Name { get; }

        public string Description { get { return Name.Preferred; } }
        
        public string[] PossibleValues { get; }

        public StandAloneOptionToken(Name name)
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
                return new TokenMatchResult(this, inputTokens[startIdx], match, MatchOutcome.Full, matchLength, 1);
            }
            else if(matchLength > 0)
            {
                return new TokenMatchResult(this, inputTokens[startIdx], match, MatchOutcome.Partial, matchLength, 0);
            }
            
            return TokenMatchResult.None;
        }
    }
}