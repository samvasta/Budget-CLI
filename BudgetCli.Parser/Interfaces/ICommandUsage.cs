using BudgetCli.Parser.Models;

namespace BudgetCli.Parser.Interfaces
{
    public interface ICommandUsage
    {
        bool IsHelp { get; }
        string Description { get; }
        ICommandToken[] Tokens { get; }
        string[] Examples { get; }
    }
}