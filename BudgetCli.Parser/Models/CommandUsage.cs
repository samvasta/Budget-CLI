using System;
using System.Linq;
using System.Collections.Generic;

using BudgetCli.Parser.Interfaces;

namespace BudgetCli.Parser.Models
{
    public class CommandUsage : ICommandUsage
    {
        public string Description { get; }
        public ICommandToken[] Tokens { get; }
        public string[] Examples { get; }

        private CommandUsage(string description, ICommandToken[] tokens, string[] examples)
        {
            if(tokens.Length == 0)
            {
                throw new ArgumentException($"{nameof(tokens)} cannot be empty!");
            }
            Description = description;
            Tokens = tokens;
            Examples = examples;
        }

        public class Builder
        {
            private string _description;
            private List<ICommandToken> _tokens;
            private List<string> _examples;

            public Builder()
            {
                _tokens = new List<ICommandToken>();
                _examples = new List<string>();
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
                return new CommandUsage(_description, _tokens.ToArray(), _examples.ToArray());
            }
        }
    }
}