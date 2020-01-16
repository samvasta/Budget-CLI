using System.IO;
using Antlr4.Runtime;
using BudgetCli.Core.Grammar;
using BudgetCli.Core.Interpreters;
using BudgetCli.Core.Models.Commands;
using BudgetCli.Core.Utilities;

namespace BudgetCli.Core.Cli
{
    public class MainInterpreter
    {
        private readonly VisitorBag _visitorBag;

        public MainInterpreter(VisitorBag visitorBag)
        {
            _visitorBag = visitorBag;
        }

        public InterpreterResult<ICommandAction> GetAction(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);

            BudgetCliLexer lexer = new BudgetCliLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            BudgetCliParser parser = new BudgetCliParser(commonTokenStream);

            parser.RemoveErrorListeners();


            return _visitorBag.CommandVisitor.Visit(parser.command());
        }
    }
}