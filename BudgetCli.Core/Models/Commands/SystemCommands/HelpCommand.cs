using System.Collections.Generic;
using BudgetCli.Core.Models.CommandResults;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Repositories;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Util.Logging;

namespace BudgetCli.Core.Models.Commands.SystemCommands
{
    public class HelpCommand : CommandActionBase
    {
        public override string ActionText => throw new System.NotImplementedException();

        public override CommandActionKind CommandActionKind { get { return CommandActionKind.Help;} }

        public ICommandRoot HelpTarget { get; }

        public HelpCommand(string rawText, RepositoryBag repositories, ICommandRoot helpTarget) : base(rawText, repositories)
        {
            HelpTarget = helpTarget;
        }

        protected override bool TryDoAction(ILog log, IEnumerable<ICommandActionListener> listeners = null)
        {
            //No data to manipulate. Need to transmit though
            if(listeners != null)
            {
                HelpCommandResult result = new HelpCommandResult(this, true, HelpTarget);
                foreach(var listener in listeners)
                {
                    listener.OnCommand(result);
                }
            }

            return true;
        }
    }
}