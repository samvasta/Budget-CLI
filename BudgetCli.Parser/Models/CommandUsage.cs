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

        private CommandUsage(bool isHelp, string description, ICommandToken[] tokens, string[] examples)
        {
            if(tokens.Length == 0)
            {
                throw new ArgumentException($"{nameof(tokens)} cannot be empty!");
            }
            IsHelp = isHelp;
            Description = description;
            Tokens = tokens;
            Examples = examples;
        }

        public class Builder
        {
            private bool _isHelp;
            private string _description;
            private readonly List<ICommandToken> _tokens;
            private readonly List<string> _examples;

            public Builder()
            {
                _isHelp = false;
                _tokens = new List<ICommandToken>();
                _examples = new List<string>();
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

            public Builder WithToken(ICommandToken token)
            {
                _tokens.Add(token);
                return this;
            }

            public Builder WithExample(string example)
            {
                _examples.Add(example);
                return this;
            }

            public CommandUsage Build()
            {
                return new CommandUsage(_isHelp, _description, _tokens.ToArray(), _examples.ToArray());
            }
        }
    }
}