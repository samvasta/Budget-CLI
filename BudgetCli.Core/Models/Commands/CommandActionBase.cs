using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using BudgetCli.Core.Models.CommandResults;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
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
        protected CommandActionBase(string rawText, RepositoryBag repositories)
        {
            RawText = rawText;
            Repositories = repositories;
        }

        public bool TryExecute(ILog log, IEnumerable<ICommandActionListener> listeners = null)
        {
            if(TryDoAction(log, listeners))
            {
                return true;
            }

            return false;
        }

        protected abstract bool TryDoAction(ILog log, IEnumerable<ICommandActionListener> listeners = null);

    }
}