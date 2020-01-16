using BudgetCli.Core.Models.Commands;

namespace BudgetCli.Core.Models.CommandResults
{
    public interface ICommandResult
    {
         ICommandAction Command { get; }
         bool IsSuccessful { get; }
    }
}