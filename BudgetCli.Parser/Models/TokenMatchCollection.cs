using System;
using System.Collections.Generic;
using BudgetCli.Parser.Interfaces;

namespace BudgetCli.Parser.Models
{
    public class TokenMatchCollection
    {
        public ICommandToken[] MatchableTokens { get; }
        private List<ParserTokenMatch> _matches;
        public IEnumerable<ParserTokenMatch> Matches { get { return _matches; } }

        public TokenMatchCollection(ICommandToken[] matchableTokens)
        {
            if(matchableTokens == null)
            {
                throw new ArgumentNullException(nameof(matchableTokens) + " cannot be null");
            }
            _matches = new List<ParserTokenMatch>();
            
            MatchableTokens = matchableTokens;
        }

        public TokenMatchCollection With(ParserTokenMatch match)
        {
            _matches.Add(match);

            return this;
        }
    }
}