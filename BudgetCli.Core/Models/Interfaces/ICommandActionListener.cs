using System.Collections.Generic;
using BudgetCli.Core.Models.CommandResults;

namespace BudgetCli.Core.Models.Interfaces
{
    public interface ICommandActionListener
    {
        bool ConfirmAction(string confirmationMessage);

        void OnCommand(ICommandResult result);

        void OnCommand(SystemCommandResult result);

        void OnCommand<T>(CreateCommandResult<T> result) where T : IDetailable;
        void OnCommand<T>(DeleteCommandResult<T> result) where T : IDetailable;
        void OnCommand<T>(ReadCommandResult<T> result) where T : IListable;
        void OnCommand<T>(ReadDetailsCommandResult<T> result) where T : IDetailable;
        void OnCommand<T>(UpdateCommandResult<T> result) where T : IDetailable;

    }
}