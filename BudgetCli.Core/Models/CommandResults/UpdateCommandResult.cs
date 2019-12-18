using System.Collections.Generic;
using BudgetCli.Core.Models.CommandResults.InfoModels;
using BudgetCli.Core.Models.Commands;

namespace BudgetCli.Core.Models.CommandResults
{
    public class UpdateCommandResult<TModel> : ICommandResult
    {
        public CommandActionBase Command { get; }

        public bool IsSuccessful { get; }
        public TModel UpdatedItem { get; }
        public IEnumerable<UpdateInfo> UpdateInfos { get; }

        public UpdateCommandResult(CommandActionBase command, bool isSuccessful, TModel updatedItem, IEnumerable<UpdateInfo> updateInfos)
        {
            Command = command;
            IsSuccessful = isSuccessful;
            UpdatedItem = updatedItem;
            UpdateInfos = updateInfos;
        }
    }
}