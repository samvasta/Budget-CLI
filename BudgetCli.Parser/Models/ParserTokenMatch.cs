using System;
using System.Collections.Generic;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models.Tokens;

namespace BudgetCli.Parser.Models
{
    public struct ParserTokenMatch
    {
        public static readonly ParserTokenMatch None = new ParserTokenMatch(-1, TokenMatchResult.None);
        
        public int TokenIdx { get; }
        public TokenMatchResult TokenMatchResult { get; }


        #region - Pass-Thru properties of TokenMatchResult -
        public ICommandToken Token { get { return TokenMatchResult.Token; } }
        public MatchOutcome MatchOutcome { get { return TokenMatchResult.MatchOutcome; } }

        /// <summary>
        /// Number of characters matched (excluding whitespace between tokens)
        /// </summary>
        public int CharsMatched { get { return TokenMatchResult.CharsMatched; } }

        /// <summary>
        /// Number of tokens matched fully or partially
        /// </summary>
        public int TokensMatched { get { return TokenMatchResult.TokensMatched; } }

        /// <summary>
        /// Full text of all tokens matched fully or partially
        /// </summary>
        public string MatchedTokensText { get { return TokenMatchResult.MatchedTokensText; } }
        
        public bool IsFullMatch { get { return TokenMatchResult.MatchOutcome == Enums.MatchOutcome.Full; } }
        #endregion - Pass-Thru properties of TokenMatchResult -

        public ParserTokenMatch(int tokenIdx, TokenMatchResult tokenMatchResult)
        {
            TokenIdx = tokenIdx;
            TokenMatchResult = tokenMatchResult;
        }
    }
}