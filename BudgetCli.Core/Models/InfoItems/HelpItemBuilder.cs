using System.Collections.Generic;

namespace BudgetCli.Core.Models.InfoItems
{
    public class HelpItemBuilder
    {
        protected string _header;
        protected string _description;
        protected List<HelpUsageToken> _usageTokens;
        protected List<HelpOptionInfoItem> _options;
        protected List<string> _examples;

        public HelpItemBuilder()
        {
            _usageTokens = new List<HelpUsageToken>();
            _options = new List<HelpOptionInfoItem>();
            _examples = new List<string>();
        }

        public HelpItemBuilder Header(string header)
        {
            _header = header;
            return this;
        }

        public HelpItemBuilder Description(string description)
        {
            _description = description;
            return this;
        }

        public HelpItemBuilder WithUsageTokens(params HelpUsageToken[] tokens)
        {
            _usageTokens.AddRange(tokens);
            return this;
        }

        public HelpItemBuilder WithOption(HelpOptionInfoItem optionInfoItem)
        {
            _options.Add(optionInfoItem);
            return this;
        }

        public HelpItemBuilder WithExamples(params string[] examples)
        {
            _examples.AddRange(examples);
            return this;
        }

        public HelpInfoItem Build()
        {
            return new HelpInfoItem()
            {
                Header = _header,
                Description = _description,
                UsageTokens = _usageTokens,
                Options = _options,
                Examples = _examples
            };
        }

    }
}