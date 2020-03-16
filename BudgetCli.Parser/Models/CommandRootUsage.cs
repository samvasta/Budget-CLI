using System.Linq;
using BudgetCli.Parser.Interfaces;

namespace BudgetCli.Parser.Models
{
    public class CommandRootUsage : ICommandUsage
    {
        public ICommandRoot CommandRoot { get; }
        public bool IsHelp { get { return false; } }

        public string Description { get { return CommandRoot.Description; } }

        public ICommandToken[] Tokens { get { return CommandRoot.CommonTokens; } }

        public string[] Examples { get { return Tokens.Select(x => x.DisplayName).ToArray(); } }

        public CommandRootUsage(ICommandRoot root)
        {
            CommandRoot = root;
        }

        public string GetDescription(ICommandToken token)
        {
            return "";
        }
    }
}