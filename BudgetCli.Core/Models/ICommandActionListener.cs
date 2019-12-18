using System.Collections.Generic;
using BudgetCli.Core.Models.CommandResults;

namespace BudgetCli.Core.Models
{
    public interface ICommandActionListener
    {
         void OnCommand(ICommandResult result);
    }
}