using BudgetCli.Core.Models.Commands;
using BudgetCli.Parser.Parsing;

namespace BudgetCli.Core.Grammar
{
    public class CommandInterpreter
    {
        public CommandLibrary Commands { get; }

        public CommandInterpreter(CommandLibrary library)
        {
            Commands = library;
        }

        public bool TryParseCommand(string input, out ICommandAction action)
        {
            action = null;
            return false;
        }
    }
}