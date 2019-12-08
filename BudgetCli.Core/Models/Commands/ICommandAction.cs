using System.Collections.Generic;

namespace BudgetCli.Core.Models.Commands
{
    public interface ICommandAction
    {
        long? Id { get; set; }

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
        bool TryExecute();
        
        /// <summary>
        /// Reverses the action
        /// </summary>
        /// /// <returns>True if undo was successful, false otherwise</returns>
        bool TryUndo();
    }
}