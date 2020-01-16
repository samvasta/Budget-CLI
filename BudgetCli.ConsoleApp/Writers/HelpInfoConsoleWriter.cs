using Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using BudgetCli.Core.Enums;
using BudgetCli.Core.Models.InfoItems;
using Console = Colorful.Console;

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
        
        public void WriteHelpItems(IEnumerable<HelpInfoItem> helpItems)
        {
            foreach(HelpInfoItem helpItem in helpItems)
            {
                WriteHelpItem(helpItem);
            }
        }

        public void WriteHelpItem(HelpInfoItem helpItem, int indent = 0)
        {
            if(!string.IsNullOrEmpty(helpItem.Header))
            {
                WriteHelpHeader(helpItem, indent);
            }
            
            SafeWriteLine(helpItem.Description + Environment.NewLine, indent);

            if(helpItem.UsageTokens.Any())
            {
                SafeWriteLine(USAGE, indent);
                foreach(HelpUsageToken token in helpItem.UsageTokens)
                {
                    WriteUsageToken(token, indent+1);
                }
                Console.WriteLine();
            }

            if(helpItem.Options.Any())
            {
                Console.WriteLine();
                SafeWriteLine(OPTIONS, indent);
                int firstColLen = helpItem.Options.Max(x => x.OptionFormat.Length);
                foreach(HelpOptionInfoItem option in helpItem.Options)
                {
                    WriteOption(option, indent+1, firstColLen);
                }
                Console.WriteLine();
            }

            if(helpItem.Examples.Any())
            {
                SafeWriteLine(EXAMPLES, indent);
                foreach(string example in helpItem.Examples)
                {
                    SafeWriteLine(example, indent+1);
                }
            }

            Console.WriteLine();
        }

        private void WriteHelpHeader(HelpInfoItem helpItem, int indent)
        {
            SafeWriteLine(helpItem.Header, indent);
            SafeWriteLine(new string(HeaderSeparatorChar, Math.Min(helpItem.Header.Length, Console.WindowWidth)), indent);
        }

        private void WriteUsageToken(HelpUsageToken token, int indent)
        {
            ConsoleColor fg = ConsoleColor.White;
            string text = token.Text;

            if(token.Kind == HelpUsageTokenKind.Command)
            {
                //Nothing to modify
            }
            else if(token.Kind == HelpUsageTokenKind.Argument)
            {
                fg = ConsoleColor.Gray;
                text = $"<{text}>";
            }
            else if(token.Kind == HelpUsageTokenKind.Option)
            {
                fg = ConsoleColor.Gray;
            }

            if(token.IsOptional)
            {
                text = $"[{text}]";
            }

            ConsoleHelper.WriteWithIndent(text + " ", indent*IndentAmount, fg);
        }

        private void WriteOption(HelpOptionInfoItem option, int indent, int firstColLen)
        {
            string optionFormat = option.OptionFormat;
            ConsoleHelper.WriteWithIndent(optionFormat, indent*IndentAmount);

            //Print description + default value
            ConsoleHelper.WriteWithIndent(option.Description, indent*IndentAmount + firstColLen);
            
            if(!string.IsNullOrEmpty(option.DefaultValue))
            {
                string defaultValue = $" [default: {option.DefaultValue}]";
                ConsoleHelper.WriteWithIndent(defaultValue, indent*IndentAmount + firstColLen, ConsoleColor.Blue);
            }

            Console.WriteLine();

            if(option.PossibleValues.Any())
            {
                string possibleValues = $"Possible Values: {{{{{string.Join(", ", option.PossibleValues)}}}}}";
                ConsoleHelper.WriteWithIndent(possibleValues, firstColLen + (indent+1)*IndentAmount, ConsoleColor.Gray);
                Console.WriteLine();
            }
        }

        private void SafeWriteLine(string text, int indent, ConsoleColor fg = ConsoleColor.White)
        {
            if(!string.IsNullOrEmpty(text))
            {
                ConsoleHelper.WriteWithIndent(text, indent*IndentAmount, fg);
                Console.WriteLine();
            }
        }

        private void IndentLine(int indent)
        {
            Console.Write(new string(' ', Math.Min(0, indent*IndentAmount)));
        }
    }
}