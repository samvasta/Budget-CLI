using System.Linq;
using System;
using System.Collections.Generic;
using BudgetCli.Core.Models.CommandResults;
using BudgetCli.Core.Models.Interfaces;

namespace BudgetCli.Core.Tests.TestHarness
{
    public class FakeCommandActionListener : ICommandActionListener
    {
        private readonly List<ICommandResult> _results;
        private readonly bool _confirmActionResult;

        public FakeCommandActionListener(bool confirmActionResult = true)
        {
            _results = new List<ICommandResult>();
            _confirmActionResult = confirmActionResult;
        }

        public bool OnlyHasType<T>()
        {
            return _results.All(x => x is T);
        }

        public IEnumerable<T> GetResults<T>()
        {
            return _results.OfType<T>();
        }

        public virtual bool ConfirmAction(string confirmationMessage)
        {
            return _confirmActionResult;
        }

        public virtual void OnCommand(ICommandResult result)
        {
            _results.Add(result);
        }

        public virtual void OnCommand(SystemCommandResult result)
        {
            _results.Add(result);
        }

        public virtual void OnCommand<T>(CreateCommandResult<T> result) where T : IDetailable
        {
            _results.Add(result);
        }

        public virtual void OnCommand<T>(DeleteCommandResult<T> result) where T : IDetailable
        {
            _results.Add(result);
        }

        public virtual void OnCommand<T>(ReadCommandResult<T> result) where T : IListable
        {
            _results.Add(result);
        }

        public virtual void OnCommand<T>(ReadDetailsCommandResult<T> result) where T : IDetailable
        {
            _results.Add(result);
        }

        public virtual void OnCommand<T>(UpdateCommandResult<T> result) where T : IDetailable
        {
            _results.Add(result);
        }
    }
}