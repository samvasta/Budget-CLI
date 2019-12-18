using BudgetCli.Core.Models.Commands;

namespace BudgetCli.Core.Models.CommandResults
{
    public interface ICommandResult
    {
         CommandActionBase Command { get; }
         bool IsSuccessful { get; }
    }
}