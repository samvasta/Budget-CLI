using System.Collections.Generic;
using BudgetCli.Core.Models.CommandResults.InfoModels;
using BudgetCli.Core.Models.Commands;
using BudgetCli.Core.Models.Interfaces;

namespace BudgetCli.Core.Models.CommandResults
{
    public class ReadCommandResult<TModel> : ICommandResult where TModel : IListable
    {
        public ICommandAction Command { get; }

        public bool IsSuccessful { get; }

        public FilterCriteria Criteria { get; }
        public IEnumerable<TModel> FilteredItems { get; }

        public ReadCommandResult(CommandActionBase command, bool isSuccessful, IEnumerable<TModel> filteredItems, FilterCriteria criteria)
        {
            Command = command;
            IsSuccessful = isSuccessful;
            Criteria = criteria;
            FilteredItems = filteredItems;
        }
    }
}