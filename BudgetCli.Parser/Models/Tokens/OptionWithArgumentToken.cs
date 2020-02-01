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

        public string Description { get { return $"{Name.Preferred} {String.Join(' ', Arguments.Select(x => x.Description))}"; } }

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
                return new TokenMatchResult(this, inputTokens[startIdx], MatchOutcome.Partial, partialMatchLength, 0);
            }

            int charsMatched = inputTokens[startIdx].Length;

            int argIdx = 0;
            //start at startIdx+1 because we already checked the option name at startIdx.
            //So start looking for arguments at startIdx+1
            for(int i = startIdx+1; i < inputTokens.Length && argIdx < Arguments.Length; i++)
            {
                TokenMatchResult matchResult = Arguments[argIdx].Matches(inputTokens, i);
                if(matchResult.MatchOutcome != MatchOutcome.Full)
                {
                    return new TokenMatchResult(this, TokenUtils.GetMatchText(inputTokens, startIdx, i), MatchOutcome.Partial, charsMatched + matchResult.CharsMatched, 1 + argIdx);
                }

                i += matchResult.TokensMatched-1;
                argIdx++;
            }

            //Add one for the option name
            int numTokens = 1 + argIdx;
            if(numTokens == FullMatchLength)
            {
                //Full match
                return new TokenMatchResult(this, TokenUtils.GetMatchText(inputTokens, startIdx, numTokens), MatchOutcome.Full, charsMatched, numTokens);
            }
            else
            {
                //Partial match
                return new TokenMatchResult(this, TokenUtils.GetMatchText(inputTokens, startIdx, numTokens),MatchOutcome.Partial, charsMatched, numTokens);
            }
        }

        public class Builder
        {
            private Name _name;
            private List<ArgumentToken> _arguments;
            
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