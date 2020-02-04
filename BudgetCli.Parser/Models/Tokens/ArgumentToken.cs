using System;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Util;
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

        public virtual string[] PossibleValues { get; }

        protected ArgumentToken(string argumentName, bool isOptional)
        {
            ArgumentName = argumentName;
            Description = $"<{argumentName.Kebaberize()}>";
            IsOptional = isOptional;
            PossibleValues = new string[0];
        }

        public abstract TokenMatchResult Matches(string[] inputTokens, int startIdx);
    }

    public class ArgumentToken<T> : ArgumentToken, ICommandArgumentToken<T>
    {

        public ICommandArgumentToken<T>.ValueParser Parser { get; }

        protected ArgumentToken(string argumentName, bool isOptional, ICommandArgumentToken<T>.ValueParser parser)
            : base(argumentName, isOptional)
        {
            if(parser == null)
            {
                throw new ArgumentNullException(nameof(parser));
            }
            Parser = parser;
        }

        public override TokenMatchResult Matches(string[] inputTokens, int startIdx)
        {
            if(startIdx >= inputTokens.Length || startIdx < 0)
            {
                return TokenMatchResult.None;
            }

            if(Parser(inputTokens[startIdx], out _))
            {
                return new TokenMatchResult(this, TokenUtils.GetMatchText(inputTokens, startIdx, 1),  MatchOutcome.Full, inputTokens[startIdx].Length, 1);
            }

            return TokenMatchResult.None;
        }

        public class Builder
        {
            protected string _name;
            protected bool _isOptional;

            protected ICommandArgumentToken<T>.ValueParser _parser;

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