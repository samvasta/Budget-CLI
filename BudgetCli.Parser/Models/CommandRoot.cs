using System;
using System.Linq;
using System.Collections.Generic;

using BudgetCli.Parser.Interfaces;

namespace BudgetCli.Parser.Models
{
    public class CommandRoot : ICommandRoot
    {
        public Name CommandName { get; }
        public string Description { get; }
        public ICommandUsage[] Usages { get; }

        private CommandRoot(Name name, string description, params ICommandUsage[] usages)
        {
            if(name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if(usages.Length == 0)
            {
                throw new ArgumentException("Must include at least 1 usage");
            }

            CommandName = name;
            Description = description;
            Usages = usages;
        }

        public class Builder
        {
            private Name _name;
            private string _description;
            private List<ICommandUsage> _usages;

            public Builder()
            {
                _usages = new List<ICommandUsage>();
            }

            public Builder Name(string preferred, params string[] alternatives)
            {
                _name = new Name(preferred, alternatives);
                return this;
            }

            public Builder Description(string description)
            {
                _description = description;
                return this;
            }

            public Builder WithUsage(ICommandUsage usage)
            {
                _usages.Add(usage);
                return this;
            }

            public CommandRoot Build()
            {
                return new CommandRoot(_name, _description, _usages.ToArray());
            }
        }
    }
}