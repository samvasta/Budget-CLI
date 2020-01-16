using System.Collections.Generic;

namespace BudgetCli.Core.Models.InfoItems
{
    public class HelpInfoItem
    {
        public string Header { get; set; }
        public string Description { get; set; }

        public List<HelpUsageToken> UsageTokens { get; set; }

        public List<HelpOptionInfoItem> Options { get; set; }

        public List<string> Examples { get; set; }
    }
}