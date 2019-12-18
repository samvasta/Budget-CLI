using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using BudgetCli.Core.Models.CommandResults;
using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Util.Attributes;
using BudgetCli.Util.Logging;

namespace BudgetCli.Core.Models.Commands
{
    public abstract class CommandActionBase : ICommandAction
    {
        protected RepositoryBag Repositories { get; }

        public virtual string RawText { get; }

        public abstract string ActionText { get; }

        public abstract CommandActionKind CommandActionKind { get; }

        /// <summary>
        /// New Command constructor
        /// </summary>
        public CommandActionBase(string rawText, RepositoryBag repositories)
        {
            RawText = rawText;
            Repositories = repositories;
        }

        public bool TryExecute(ILog log)
        {
            // using(TransactionScope scope = new TransactionScope())
            // {
                if(TryDoAction(log))
                {
                    // scope.Complete();
                    return true;
                }
            // }

            return false;
        }

        protected abstract bool TryDoAction(ILog log, IEnumerable<ICommandActionListener> listeners = null);

        protected void TransmitResult(ICommandResult result, IEnumerable<ICommandActionListener> listeners)
        {
            if(listeners != null && result != null)
            {
                foreach(var listener in listeners)
                {
                    listener.OnCommand(result);
                }
            }
        }
    }
}