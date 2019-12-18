using BudgetCli.Core.Models.Commands;

namespace BudgetCli.Core.Models.CommandResults
{
    public class DeleteCommandResult<TModel> : ICommandResult
    {
        public CommandActionBase Command { get; }

        public bool IsSuccessful { get; }

        public TModel DeletedItem { get; }

        public DeleteCommandResult(CommandActionBase command, bool isSuccessful, TModel deletedItem)
        {
            Command = command;
            IsSuccessful = isSuccessful;
            DeletedItem = deletedItem;
        }
    }
}