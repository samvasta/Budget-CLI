using System;
using System.Linq;
using System.Collections.Generic;

using BudgetCli.Parser.Interfaces;

namespace BudgetCli.Parser.Models
{
    public class CommandUsage : ICommandUsage
    {
        public bool IsHelp { get; }
        public string Description { get; }
        public ICommandToken[] Tokens { get; }
        public string[] Examples { get; }

        private readonly Dictionary<ICommandToken, string> _descriptions;
        private readonly Dictionary<ICommandToken, object> _defaultValues;

        private CommandUsage(bool isHelp, string description, ICommandToken[] tokens, string[] examples, Dictionary<ICommandToken, string> descriptions, Dictionary<ICommandToken, object> defaultValues)
        {
            if(tokens.Length == 0)
            {
                throw new ArgumentException($"{nameof(tokens)} cannot be empty!");
            }
            IsHelp = isHelp;
            Description = description;
            Tokens = tokens;
            Examples = examples;
            _descriptions = descriptions;
            _defaultValues = defaultValues;
        }

        public string GetDescription(ICommandToken token)
        {
            if(_descriptions.ContainsKey(token))
            {
                return _descriptions[token];
            }
            return String.Empty;
        }

        public object GetDefaultValue(ICommandToken token)
        {
            if(_defaultValues.ContainsKey(token))
            {
                return _defaultValues[token];
            }
            return null;
        }

        public class Builder
        {
            private bool _isHelp;
            private string _description;
            private readonly List<ICommandToken> _tokens;
            private readonly List<string> _examples;
            private readonly Dictionary<ICommandToken, string> _descriptions;
            private readonly Dictionary<ICommandToken, object> _defaultValues;

            public Builder()
            {
                _isHelp = false;
                _tokens = new List<ICommandToken>();
                _examples = new List<string>();
                _descriptions = new Dictionary<ICommandToken, string>();
                _defaultValues = new Dictionary<ICommandToken, object>();
            }

            public Builder IsHelp()
            {
                _isHelp = true;
                return this;
            }

            public Builder Description(string description)
            {
                _description = description;
                return this;
            }

            public Builder WithToken(ICommandToken token, string description = "", object defaultValue = null)
            {
                _tokens.Add(token);
                _descriptions.Add(token, description);
                if(defaultValue != null)
                {
                    _defaultValues.Add(token, defaultValue);
                }
                return this;
            }

            public Builder WithExample(string example)
            {
                _examples.Add(example);
                return this;
            }

            public CommandUsage Build()
            {
                return new CommandUsage(_isHelp, _description, _tokens.ToArray(), _examples.ToArray(), _descriptions, _defaultValues);
            }
        }
    }
}