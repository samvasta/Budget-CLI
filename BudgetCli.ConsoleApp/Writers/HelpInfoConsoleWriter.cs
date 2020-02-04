using Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using BudgetCli.Core.Enums;
using Console = Colorful.Console;
using BudgetCli.Parser.Interfaces;

namespace BudgetCli.ConsoleApp.Writers
{
    public class HelpInfoConsoleWriter
    {
        public const string USAGE = "Usage:\n";
        public const string OPTIONS = "Options:\n";
        public const string EXAMPLES = "Examples:\n";

        public int IndentAmount { get; set; }
        public char HeaderSeparatorChar { get; set; }

        public HelpInfoConsoleWriter(int indentAmount = 8, char headerSeparatorChar = '-')
        {
            IndentAmount = indentAmount;
            HeaderSeparatorChar = headerSeparatorChar;
        }
        
        public void WriteHelpItems(IEnumerable<ICommandRoot> helpItems)
        {
            foreach(ICommandRoot helpItem in helpItems)
            {
                WriteHelpItem(helpItem);
            }
        }

        public void WriteHelpItem(ICommandRoot helpItem, int indent = 0)
        {
            //TODO
        }
    }
}