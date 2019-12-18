using BudgetCli.Core.Models.Commands;

namespace BudgetCli.Core.Models.CommandResults
{
    public class CreateCommandResult<TModel> : ICommandResult
    {
        public CommandActionBase Command { get; }

        public bool IsSuccessful { get; }

        public TModel CreatedItem { get; }

        public CreateCommandResult(CommandActionBase command, bool isSuccessful, TModel createdItem)
        {
            Command = command;
            IsSuccessful = isSuccessful;
            CreatedItem = createdItem;
        }
    }
}