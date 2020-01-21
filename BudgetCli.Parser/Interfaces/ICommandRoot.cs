using BudgetCli.Parser.Models;

namespace BudgetCli.Parser.Interfaces
{
    public interface ICommandRoot
    {         
        Name CommandName { get; }
        string Description { get; }
        ICommandUsage[] Usages { get; }
    }
}