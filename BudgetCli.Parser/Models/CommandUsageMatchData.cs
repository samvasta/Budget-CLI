using System.Linq;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models.Tokens;

namespace BudgetCli.Parser.Models
{
    public class CommandUsageMatchData
    {
        public ICommandRoot Command { get; }
        public ICommandUsage Usage { get; }

        public virtual bool IsSuccessful { get { return _matchCollection.IsFullMatch; } }

        private readonly TokenMatchCollection _matchCollection;
        
        public CommandUsageMatchData(ICommandRoot command, ICommandUsage usage, TokenMatchCollection matchCollection)
        {
            Command = command;
            Usage = usage;
            _matchCollection = matchCollection;
        }

        public virtual bool TryGetArgValue<T>(ArgumentToken argument, out T value)
        {
            return _matchCollection.TryGetArgValue(argument, out value);
        }

        public virtual bool TryGetArgValue<T>(string argName, out T value)
        {
            return _matchCollection.TryGetArgValue(argName, out value);
        }

        public virtual bool HasToken(ICommandToken token)
        {
            return _matchCollection.Matches.Any(x => x.IsFullMatch && x.Token.Equals(token));
        }

    }
}