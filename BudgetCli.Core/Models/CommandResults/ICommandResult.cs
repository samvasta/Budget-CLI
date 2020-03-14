using BudgetCli.Core.Models.Commands;

namespace BudgetCli.Core.Models.CommandResults
{
    public interface ICommandResult
    {
         ICommandAction CommandAction { get; }
         bool IsSuccessful { get; }
    }
}