using System.Collections.Generic;
using BudgetCli.Core.Models.Commands;
using BudgetCli.Core.Models.Interfaces;

namespace BudgetCli.Core.Models.CommandResults
{
    public class DeleteCommandResult<TModel> : ICommandResult where TModel : IDetailable
    {
        public ICommandAction CommandAction { get; }

        public bool IsSuccessful { get; }

        public IEnumerable<TModel> DeletedItems { get; }

        public DeleteCommandResult(CommandActionBase command, bool isSuccessful, TModel deletedItem)
            : this(command, isSuccessful, new [] { deletedItem} )
        {
            //intentionally blank
        }

        public DeleteCommandResult(CommandActionBase command, bool isSuccessful, IEnumerable<TModel> deletedItems)
        {
            CommandAction = command;
            IsSuccessful = isSuccessful;
            DeletedItems = deletedItems;
        }
    }
}