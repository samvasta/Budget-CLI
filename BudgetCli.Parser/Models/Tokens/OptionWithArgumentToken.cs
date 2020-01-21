using System.Linq;
using System;
using System.Collections.Generic;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Interfaces;

namespace BudgetCli.Parser.Models.Tokens
{
    public class OptionWithArgumentToken : ICommandToken
    {
        public TokenKind Kind { get { return TokenKind.OptionWithArgument; } }

        public bool IsOptional { get { return true; } }

        public Name Name { get; }

        public ArgumentToken[] Arguments { get; }

        public string Description { get { return $"{Name.Preferred} {String.Join(' ', Arguments.Select(x => x.Description))}"; } }

        private OptionWithArgumentToken(Name name, ArgumentToken[] arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public bool Matches(string[] inputTokens, int startIdx, out int matchLength)
        {
            if(startIdx >= inputTokens.Length || startIdx < 0)
            {
                matchLength = 0;
                return false;
            }

            if((inputTokens.Length - startIdx) < (1 + Arguments.Length))
            {
                matchLength = 0;
                return false;
            }

            if(!Name.Equals(inputTokens[startIdx]))
            {
                matchLength = 0;
                return false;
            }

            int argIdx = 0;
            for(int i = startIdx+1; i < inputTokens.Length && argIdx < Arguments.Length; i++)
            {
                int argMatchLength;
                if(!Arguments[argIdx].Matches(inputTokens, i, out argMatchLength))
                {
                    matchLength = i - startIdx - 1;
                    return false;
                }
                i += argMatchLength-1;
                argIdx++;
            }

            //Add one for the option name
            matchLength = 1 + argIdx;
            return true;
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