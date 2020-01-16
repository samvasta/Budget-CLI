using System.Collections.Generic;
using BudgetCli.Core.Enums;
using BudgetCli.Core.Models.CommandResults;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Util.Logging;

namespace BudgetCli.Core.Models.Commands.SystemCommands
{
    public class SystemCommand : ICommandAction
    {
        public string RawText { get; set; }

        public string ActionText { get { return RawText; } }

        public SystemCommandKind CommandKind { get; set; }

        public SystemCommand(string text, SystemCommandKind commandKind)
        {
            RawText = text;
            CommandKind = commandKind;
        }

        public bool TryExecute(ILog log, IEnumerable<ICommandActionListener> listeners = null)
        {
            //No data to manipulate. Need to transmit though
            if(listeners != null)
            {
                SystemCommandResult result = new SystemCommandResult(this, true, CommandKind);
                foreach(var listener in listeners)
                {
                    listener.OnCommand(result);
                }
            }

            return true;
        }
    }
}