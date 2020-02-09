using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models.Tokens;

namespace BudgetCli.Parser.Models
{
    public class CommandUsageMatchData
    {
        public ICommandRoot Command { get; }
        public ICommandUsage Usage { get; }

        private readonly TokenMatchCollection _matchCollection;
        
        public CommandUsageMatchData(ICommandRoot command, ICommandUsage usage, TokenMatchCollection matchCollection)
        {
            Command = command;
            Usage = usage;
            _matchCollection = matchCollection;
        }

        public bool TryGetArgValue<T>(ArgumentToken argument, out T value)
        {
            return _matchCollection.TryGetArgValue(argument, out value);
        }

    }
}