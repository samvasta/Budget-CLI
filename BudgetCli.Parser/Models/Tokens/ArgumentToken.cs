using System;
using System.Collections.Generic;
using System.Linq;
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

        public string DisplayName { get; }

        public virtual string[] PossibleValues { get; }

        protected ArgumentToken(string argumentName, bool isOptional, IEnumerable<string> possibleValues)
        {
            ArgumentName = argumentName;
            DisplayName = $"<{argumentName.Kebaberize()}>";
            IsOptional = isOptional;
            PossibleValues = possibleValues.ToArray();
        }

        public abstract TokenMatchResult Matches(string[] inputTokens, int startIdx);
        public virtual object Parse(string text) { return null; }

        public override string ToString()
        {
            return ArgumentName;
        }
    }

    public class ArgumentToken<T> : ArgumentToken, ICommandArgumentToken<T>
    {

        public ICommandArgumentToken<T>.ValueParser Parser { get; }

        protected ArgumentToken(string argumentName, bool isOptional, ICommandArgumentToken<T>.ValueParser parser, IEnumerable<string> possibleValues)
            : base(argumentName, isOptional, possibleValues)
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

            if(Parser(inputTokens[startIdx], out T value))
            {
                string text = TokenUtils.GetMatchText(inputTokens, startIdx, 1);
                var result = new TokenMatchResult(this, text, text, MatchOutcome.Full, inputTokens[startIdx].Length, 1);
                result.SetArgValue(this, value);
                return result;
            }

            return TokenMatchResult.None;
        }

        public override object Parse(string text)
        {
            return ParseAs(text);
        }

        public T ParseAs(string text)
        {
            T value;
            Parser(text, out value);
            return value;
        }

        public class Builder
        {
            protected string _name;
            protected bool _isOptional;
            protected List<object> _possibleValues = new List<object>();

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

            public Builder PossibleValues(IEnumerable<object> values)
            {
                _possibleValues.AddRange(values);
                return this;
            }

            public Builder PossibleValues(params object[] values)
            {
                _possibleValues.AddRange(values);
                return this;
            }

            public Builder Parser(ICommandArgumentToken<T>.ValueParser parser)
            {
                _parser = parser;
                return this;
            }

            public ArgumentToken<T> Build()
            {
                return new ArgumentToken<T>(_name, _isOptional, _parser, _possibleValues.Select(x => x.ToString()));
            }
        }
    }
}