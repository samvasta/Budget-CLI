using System.Collections.Generic;
using BudgetCli.Core.Models.InfoItems;
using BudgetCli.Data.Enums;

namespace BudgetCli.Core.Utilities
{
    public class HelpLookup
    {
        private Dictionary<CommandActionKind, IEnumerable<HelpInfoItem>> HelpCommandLookup { get; }

        public HelpLookup()
        {
            HelpCommandLookup = new Dictionary<CommandActionKind, IEnumerable<HelpInfoItem>>();
        }

        public bool AddHelp(CommandActionKind actionKind, params HelpInfoItem[] helpInfos)
        {
            if(HelpCommandLookup.ContainsKey(actionKind))
            {
                return false;
            }
            HelpCommandLookup.Add(actionKind, helpInfos);
            return true;
        }

        public IEnumerable<HelpInfoItem> GetHelpItemsFor(CommandActionKind actionKind)
        {
            if(HelpCommandLookup.ContainsKey(actionKind))
            {
                return HelpCommandLookup[actionKind];
            }
            return new List<HelpInfoItem>();
        }
    }
}