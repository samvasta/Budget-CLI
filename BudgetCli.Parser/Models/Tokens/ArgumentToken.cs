using System;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Util.Utilities;
using Humanizer;

namespace BudgetCli.Parser.Models.Tokens
{
    public abstract class ArgumentToken : ICommandToken
    {
        public TokenKind Kind { get { return TokenKind.Argument; } }

        public bool IsOptional { get; }

        public string ArgumentName { get; }

        public string Description { get; }

        protected ArgumentToken(string argumentName, bool isOptional)
        {
            if(argumentName.ContainsWhitespace())
            {
                throw new ArgumentException($"{nameof(argumentName)} cannot contain whitespace");
            }
            ArgumentName = argumentName;
            Description = $"<{argumentName.Kebaberize()}>";
            IsOptional = isOptional;
        }

        public abstract bool Matches(string[] inputTokens, int startIdx, out int matchLength);
    }

    public class ArgumentToken<T> : ArgumentToken, ICommandArgumentToken<T>
    {

        public ICommandArgumentToken<T>.ValueParser Parser { get; }

        private ArgumentToken(string argumentName, bool isOptional, ICommandArgumentToken<T>.ValueParser parser)
            : base(argumentName, isOptional)
        {
            Parser = parser;
        }

        public override bool Matches(string[] inputTokens, int startIdx, out int matchLength)
        {
            if(startIdx >= inputTokens.Length || startIdx < 0)
            {
                matchLength = 0;
                return false;
            }

            if(Parser(inputTokens[startIdx], out _))
            {
                matchLength = 1;
                return true;
            }

            matchLength = 0;
            return false;
        }

        public class Builder
        {
            private string _name;
            private bool _isOptional;

            private ICommandArgumentToken<T>.ValueParser _parser;

            public Builder Name(string name)
            {
                _name = name;
                return this;
            }

            public Builder IsOptional(bool isOptional)
            {
                _isOptional = isOptional;
                return this;
            }

            public Builder Parser(ICommandArgumentToken<T>.ValueParser parser)
            {
                _parser = parser;
                return this;
            }

            public ArgumentToken<T> Build()
            {
                return new ArgumentToken<T>(_name, _isOptional, _parser);
            }
        }
    }
}