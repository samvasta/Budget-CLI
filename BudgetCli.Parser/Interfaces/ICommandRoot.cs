using BudgetCli.Parser.Models;
using BudgetCli.Parser.Models.Tokens;

namespace BudgetCli.Parser.Interfaces
{
    public interface ICommandRoot
    {
        int CommandId { get; }
        VerbToken[] CommonTokens { get; }
        string Description { get; }
        ICommandUsage[] Usages { get; }
    }
}