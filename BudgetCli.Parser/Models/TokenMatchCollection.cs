using System;
using System.Collections.Generic;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models.Tokens;

namespace BudgetCli.Parser.Models
{
    public class TokenMatchCollection
    {
        public string TextToMatch { get; }
        public ICommandToken[] MatchableTokens { get; }
        private List<ParserTokenMatch> _matches;
        public IEnumerable<ParserTokenMatch> Matches { get { return _matches; } }

        private readonly ValueBag<ArgumentToken> _argumentValues;

        /// <summary>
        /// Heuristic for quality of match. Bigger is better. Not guaranteed to be in any range.
        /// </summary>
        public float MatchQuality
        {
            get
            {
                float percentOfTokensMatched = 0;
                int totalCharsMatched = 0;
                
                foreach(var match in _matches)
                {
                    percentOfTokensMatched += (float)match.CharsMatched / match.MatchedTokensText.Length;
                    totalCharsMatched += match.CharsMatched;
                }

                return totalCharsMatched * percentOfTokensMatched;
            }
        }

        public TokenMatchCollection(string textToMatch, ICommandToken[] matchableTokens)
        {
            if(matchableTokens == null)
            {
                throw new ArgumentNullException(nameof(matchableTokens) + " cannot be null");
            }
            TextToMatch = textToMatch;
            _matches = new List<ParserTokenMatch>();
            _argumentValues = new ValueBag<ArgumentToken>();
            
            MatchableTokens = matchableTokens;
        }

        public TokenMatchCollection With(ParserTokenMatch match)
        {
            _matches.Add(match);

            return this;
        }

        public void SetArgValue(ArgumentToken argument, object value)
        {
            _argumentValues.SetValue(argument, value);
        }

        public bool TryGetArgValue<T>(ArgumentToken argument, out T value)
        {
            return _argumentValues.TryGetValue<T>(argument, out value);
        }
    }
}