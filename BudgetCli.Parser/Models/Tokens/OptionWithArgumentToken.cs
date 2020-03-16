using System.Runtime.Versioning;
using System.Linq;
using System;
using System.Collections.Generic;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Util;
using BudgetCli.Util.Models;

namespace BudgetCli.Parser.Models.Tokens
{
    public class OptionWithArgumentToken : ICommandToken
    {
        public TokenKind Kind { get { return TokenKind.OptionWithArgument; } }

        public bool IsOptional { get { return true; } }

        public Name Name { get; }

        public ArgumentToken[] Arguments { get; }

        public string DisplayName { get { return $"{Name.ToDisplayName()} {String.Join(' ', Arguments.Select(x => x.DisplayName))}"; } }

        /// <summary>
        /// Number of tokens that are expected to match in the case of completely successful parsing
        /// </summary>
        public int FullMatchLength { get { return 1 + Arguments.Length; } }

        public string[] PossibleValues { get; }

        private OptionWithArgumentToken(Name name, ArgumentToken[] arguments)
        {
            Name = name;
            Arguments = arguments;
            PossibleValues = new string[] {};
        }

        public TokenMatchResult Matches(string[] inputTokens, int startIdx)
        {            
            //Not in bounds of input tokens array
            if(startIdx >= inputTokens.Length || startIdx < 0)
            {
                return TokenMatchResult.None;
            }

            int partialMatchLength;
            string match = Name.GetLongestMatch(inputTokens[startIdx], out partialMatchLength);
            if(match.Length != partialMatchLength)
            {
                //Only partial match on the option name
                return new TokenMatchResult(this, inputTokens[startIdx], match, MatchOutcome.Partial, partialMatchLength, 0);
            }

            int charsMatched = inputTokens[startIdx].Length;

            List<TokenMatchResult> argMatchResults = new List<TokenMatchResult>();

            int argIdx = 0;
            //start at startIdx+1 because we already checked the option name at startIdx.
            //So start looking for arguments at startIdx+1
            for(int i = startIdx+1; i < inputTokens.Length && argIdx < Arguments.Length; i++)
            {
                TokenMatchResult matchResult = Arguments[argIdx].Matches(inputTokens, i);
                argMatchResults.Add(matchResult);
                if(matchResult.MatchOutcome != MatchOutcome.Full)
                {
                    string partialMatchText = TokenUtils.GetMatchText(inputTokens, startIdx, i);
                    var partialResult = new TokenMatchResult(this, partialMatchText, partialMatchText, MatchOutcome.Partial, charsMatched + matchResult.CharsMatched, 1 + argIdx);
                    PopulateArgValues(partialResult, argMatchResults);
                    return partialResult;
                    
                }

                i += matchResult.TokensMatched-1;
                argIdx++;
            }

            //Add one for the option name
            int numTokens = 1 + argIdx;
            string matchText = TokenUtils.GetMatchText(inputTokens, startIdx, numTokens);
            TokenMatchResult result;
            if(numTokens == FullMatchLength)
            {
                //Full match
                result = new TokenMatchResult(this, matchText, matchText, MatchOutcome.Full, charsMatched, numTokens);
            }
            else
            {
                //Partial match
                result = new TokenMatchResult(this, matchText, matchText, MatchOutcome.Partial, charsMatched, numTokens);
            }
            PopulateArgValues(result, argMatchResults);
            return result;
        }

        private void PopulateArgValues(TokenMatchResult target, IEnumerable<TokenMatchResult> sources)
        {
            foreach(var source in sources)
            {
                target.ArgumentValues.AddAll(source.ArgumentValues);
            }
        }

        public class Builder
        {
            private Name _name;
            private readonly List<ArgumentToken> _arguments;
            
            public Builder()
            {
                _arguments = new List<ArgumentToken>();
            }

            public Builder Name(string preferred, params string[] alternates)
            {
                _name = new Name(preferred, alternates);
                return this;
            }

            public Builder WithArgument(ArgumentToken argument)
            {
                if(argument.IsOptional)
                {
                    throw new ArgumentException("Argument cannot be optional");
                }
                _arguments.Add(argument);
                return this;
            }

            public OptionWithArgumentToken Build()
            {
                return new OptionWithArgumentToken(_name, _arguments.ToArray());
            }
        }
    }
}