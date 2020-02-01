using System;
using System.Linq;
using System.Collections.Generic;

using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Util.Models;

namespace BudgetCli.Parser.Models
{
    public class CommandRoot : ICommandRoot
    {
        public VerbToken[] CommonTokens { get; }
        public string Description { get; }
        public ICommandUsage[] Usages { get; }

        private CommandRoot(VerbToken[] commonTokens, string description, params ICommandUsage[] usages)
        {
            if(commonTokens.Length == 0)
            {
                throw new ArgumentException("Must include at least 1 common token");
            }

            CommonTokens = commonTokens;
            Description = description;
            Usages = usages;
        }

        public class Builder
        {
            private List<VerbToken> _commonTokens;
            private string _description;
            private List<ICommandUsage> _usages;

            public Builder()
            {
                _commonTokens = new List<VerbToken>();
                _usages = new List<ICommandUsage>();
            }

            public Builder WithToken(string preferred, params string[] alternatives)
            {
                _commonTokens.Add(new VerbToken(new Name(preferred, alternatives)));
                return this;
            }

            public Builder WithToken(Name verb)
            {
                _commonTokens.Add(new VerbToken(verb));
                return this;
            }

            public Builder WithToken(VerbToken verb)
            {
                _commonTokens.Add(verb);
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
                return new CommandRoot(_commonTokens.ToArray(), _description, _usages.ToArray());
            }
        }
    }
}