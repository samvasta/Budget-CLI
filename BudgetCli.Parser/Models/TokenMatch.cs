using System;
using System.Collections.Generic;
using BudgetCli.Parser.Interfaces;

namespace BudgetCli.Parser.Models
{
    public struct TokenMatch
    {
        public ICommandToken Token { get; }
        public int TokenIdx { get; }
        public string MatchText { get; }

        public TokenMatch(ICommandToken token, int tokenIdx, string matchText)
        {
            if(token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }
            if(tokenIdx < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tokenIdx));
            }
            
            Token = token;
            TokenIdx = tokenIdx;
            MatchText = matchText;
        }
    }
}