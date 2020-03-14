using System.Collections.Generic;
using BudgetCli.Core.Models.CommandResults.InfoModels;
using BudgetCli.Core.Models.Commands;
using BudgetCli.Core.Models.Interfaces;

namespace BudgetCli.Core.Models.CommandResults
{
    public class ReadDetailsCommandResult<TModel> : ICommandResult where TModel : IDetailable
    {
        public ICommandAction CommandAction { get; }

        public bool IsSuccessful { get; }

        public FilterCriteria Criteria { get; }
        public TModel Item { get; }

        public ReadDetailsCommandResult(CommandActionBase command, bool isSuccessful, TModel item, FilterCriteria criteria)
        {
            CommandAction = command;
            IsSuccessful = isSuccessful;
            Criteria = criteria;
            Item = item;
        }
    }
}