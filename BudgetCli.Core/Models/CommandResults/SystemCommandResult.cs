using BudgetCli.Core.Enums;
using BudgetCli.Core.Models.Commands;

namespace BudgetCli.Core.Models.CommandResults
{
    public class SystemCommandResult : ICommandResult
    {
        public ICommandAction Command { get; }

        public bool IsSuccessful { get; set; }

        public SystemCommandKind CommandKind { get; set; }

        public SystemCommandResult(ICommandAction command, bool isSuccessful, SystemCommandKind commandKind)
        {
            Command = command;
            IsSuccessful = isSuccessful;
            CommandKind = commandKind;
        }
    }
}