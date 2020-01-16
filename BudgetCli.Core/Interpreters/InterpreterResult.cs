using System.Collections.Generic;

namespace BudgetCli.Core.Interpreters
{
    public class InterpreterResult<T>
    {
        public bool IsSuccessful { get; }
        public IEnumerable<string> NextTokenSuggestions { get; }
        public T ReturnValue { get; }
        
        /// <summary>
        /// Non-successful constructor
        /// </summary>
        public InterpreterResult(params string[] suggestions)
        {
            IsSuccessful = false;
            NextTokenSuggestions = new List<string>(suggestions);
        }

        /// <summary>
        /// Successful constructor
        /// </summary>
        public InterpreterResult(T returnValue)
        {
            IsSuccessful = true;
            ReturnValue = returnValue;
            NextTokenSuggestions = null;
        }
    }
}