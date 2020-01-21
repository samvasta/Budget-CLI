using System;
using System.Collections.Generic;
using BudgetCli.Parser.Interfaces;

namespace BudgetCli.Parser.Models
{
    public class TokenMatchCollection
    {
        public ICommandUsage Usage { get; }
        private List<TokenMatch> _matches;
        public IEnumerable<TokenMatch> Matches { get { return _matches; } }

        public TokenMatchCollection(ICommandUsage usage)
        {
            if(usage == null)
            {
                throw new ArgumentNullException(nameof(usage) + " cannot be null");
            }
            _matches = new List<TokenMatch>();
            
            Usage = usage;
        }

        public TokenMatchCollection With(TokenMatch match)
        {
            _matches.Add(match);

            return this;
        }
    }
}