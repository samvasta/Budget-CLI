using System.Collections.Generic;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Util.Logging;

namespace BudgetCli.Core.Models.Commands
{
    public interface ICommandAction
    {
        /// <summary>
        /// The raw text used to invoke the action
        /// </summary>
        string RawText { get; }

        /// <summary>
        /// Brief description of the action
        /// </summary>
        string ActionText { get; }
        
        /// <summary>
        /// Executes the action
        /// </summary>
        /// <returns>True if execution was successful, false otherwise</returns>
        bool TryExecute(ILog log, IEnumerable<ICommandActionListener> listeners = null);
    }
}