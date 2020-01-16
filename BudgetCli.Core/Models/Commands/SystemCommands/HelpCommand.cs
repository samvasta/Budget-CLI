using System.Collections.Generic;
using BudgetCli.Core.Models.CommandResults;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Repositories;
using BudgetCli.Util.Logging;

namespace BudgetCli.Core.Models.Commands.SystemCommands
{
    public class HelpCommand : CommandActionBase
    {
        public override string ActionText => throw new System.NotImplementedException();

        public override CommandActionKind CommandActionKind { get { return CommandActionKind.Help;} }

        public CommandActionKind HelpTarget { get; }

        public HelpCommand(string rawText, RepositoryBag repositories, CommandActionKind helpTarget) : base(rawText, repositories)
        {
            HelpTarget = helpTarget;
        }

        protected override bool TryDoAction(ILog log, IEnumerable<ICommandActionListener> listeners = null)
        {
            //No data to manipulate. Need to transmit though
            if(listeners != null)
            {
                SystemCommandResult result = new SystemCommandResult(this, true, Enums.SystemCommandKind.Help);
                foreach(var listener in listeners)
                {
                    listener.OnCommand(result);
                }
            }

            return true;
        }
    }
}