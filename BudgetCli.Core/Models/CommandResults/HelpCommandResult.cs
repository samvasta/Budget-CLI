using BudgetCli.Core.Enums;
using BudgetCli.Core.Models.Commands;
using BudgetCli.Data.Enums;
using BudgetCli.Parser.Interfaces;

namespace BudgetCli.Core.Models.CommandResults
{
    public class HelpCommandResult : ICommandResult
    {
        public ICommandAction CommandAction { get; }

        public bool IsSuccessful { get; set; }

        public ICommandRoot CommandRootTarget { get; set; }

        public HelpCommandResult(ICommandAction command, bool isSuccessful, ICommandRoot target)
        {
            CommandAction = command;
            IsSuccessful = isSuccessful;
            CommandRootTarget = target;
        }
    }
}