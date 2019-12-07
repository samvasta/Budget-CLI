using System.IO;
using Antlr4.Runtime;
using BudgetCliCore.Grammar;
using BudgetCliCore.Interpreters;
using BudgetCliCore.Models.Commands;
using BudgetCliCore.Utilities;

namespace BudgetCliCore.Cli
{
    public class MainInterpreter
    {
        private readonly VisitorBag _visitorBag;

        public MainInterpreter()
        {
            _visitorBag = VisitorBagUtil.GetRuntimeVisitorBag();
        }

        public ICommandAction GetAction(string input)
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