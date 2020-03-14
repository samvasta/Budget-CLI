using BudgetCli.Core.Models.Commands;
using BudgetCli.Core.Models.Interfaces;

namespace BudgetCli.Core.Models.CommandResults
{
    public class CreateCommandResult<TModel> : ICommandResult where TModel : IDetailable
    {
        public ICommandAction CommandAction { get; }

        public bool IsSuccessful { get; }

        public TModel CreatedItem { get; }

        public CreateCommandResult(CommandActionBase command, bool isSuccessful, TModel createdItem)
        {
            CommandAction = command;
            IsSuccessful = isSuccessful;
            CreatedItem = createdItem;
        }
    }
}