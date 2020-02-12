using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Util.Utilities;

namespace BudgetCli.Parser.Models.Tokens
{
    public struct TokenMatchResult : IComparable<TokenMatchResult>, IEquatable<TokenMatchResult>
    {
        public static readonly TokenMatchResult None = new TokenMatchResult(null, String.Empty, String.Empty, MatchOutcome.None, 0, 0);

        public ICommandToken Token { get; }
        public MatchOutcome MatchOutcome { get; }

        /// <summary>
        /// Number of characters matched (excluding whitespace between tokens)
        /// </summary>
        public int CharsMatched { get; }

        /// <summary>
        /// Number of tokens matched fully or partially
        /// </summary>
        public int TokensMatched { get; }

        /// <summary>
        /// Full text of all tokens matched fully or partially
        /// </summary>
        public string MatchedTokensText { get; }

        /// <summary>
        /// Text expected to classify as a "full" match
        /// </summary>
        public string FullMatchText { get; }

        public TokenMatchResult(ICommandToken token, string matchedTokensText, string fullMatchText, MatchOutcome matchOutcome, int charsMatched, int tokensMatched)
        {
            if(matchedTokensText == null)
            {
                throw new ArgumentNullException(nameof(matchedTokensText));
            }
            if(charsMatched < 0 || charsMatched > matchedTokensText.Length)
            {
                throw new ArgumentOutOfRangeException($"{nameof(charsMatched)} should be <= {nameof(matchedTokensText)}.Length");
            }
            
            Token = token;
            MatchedTokensText = matchedTokensText;
            FullMatchText = fullMatchText;
            MatchOutcome = matchOutcome;
            CharsMatched = charsMatched;
            TokensMatched = tokensMatched;
        }

        /// <summary>
        /// Compares this match to another. Prioritizes <see cref="MatchOutcome"/>, then <see cref="TokensMatched"/> then <see cref="CharsMatched"/>
        /// </summary>
        public bool IsBetterMatchThan(TokenMatchResult right)
        {
            return this.CompareTo(right) > 0;
        }

        public int CompareTo([NotNull] TokenMatchResult other)
        {
            //Prioritize match length
            int matchLengthCompare = this.MatchOutcome.CompareTo(other.MatchOutcome);
            if(matchLengthCompare != 0)
            {
                return matchLengthCompare;
            }

            //Then prioritize tokens matched
            int tokensCompare = this.TokensMatched.CompareTo(other.TokensMatched);
            if(tokensCompare != 0)
            {
                return tokensCompare;
            }

            //Then prioritize characters matched
            return this.CharsMatched.CompareTo(other.CharsMatched);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Token, MatchedTokensText, MatchOutcome, CharsMatched, TokensMatched);
        }

        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }
            if(obj is TokenMatchResult other)
            {
                return Equals(other);
            }
            return false;
        }

        public bool Equals(TokenMatchResult other)
        {
            return this.CompareTo(other) == 0;
        }

        public static bool operator >(TokenMatchResult left, TokenMatchResult right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <(TokenMatchResult left, TokenMatchResult right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >=(TokenMatchResult left, TokenMatchResult right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <=(TokenMatchResult left, TokenMatchResult right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator ==(TokenMatchResult left, TokenMatchResult right)
        {
            return left.CompareTo(right) == 0;
        }

        public static bool operator !=(TokenMatchResult left, TokenMatchResult right)
        {
            return left.CompareTo(right) != 0;
        }
    }
}