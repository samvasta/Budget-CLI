using System.Drawing;
using Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using BudgetCli.Core.Enums;
using Console = Colorful.Console;
using BudgetCli.Parser.Interfaces;
using ConsoleTables;
using Colorful;
using BudgetCli.Parser.Parsing;

namespace BudgetCli.ConsoleApp.Writers
{
    public class HelpInfoConsoleWriter
    {
        public const string USAGE = "Usage:\n";
        public const string OPTIONS = "Options:\n";
        public const string EXAMPLES = "Examples:\n";

        public Color ColorH1 { get; set; } = Color.Yellow;
        public Color ColorH2 { get; set; } = Color.Red;
        public Color ColorH3 { get; set; } = Color.Green;
        public int IndentAmount { get; set; }
        public char HeaderSeparatorChar { get; set; }

        public HelpInfoConsoleWriter(int indentAmount = 4, char headerSeparatorChar = '-')
        {
            IndentAmount = indentAmount;
            HeaderSeparatorChar = headerSeparatorChar;
        }

        public void WriteCommandList(CommandLibrary library)
        {
            List<ICommandRoot> commands = library.GetAllCommands().ToList();
            if(!commands.Any())
            {
                Console.WriteLine($"There are 0 available commands!");
                return;
            }

            Console.WriteLine($"{Environment.NewLine}Available Commands:");
            Console.WriteLine(                     $"-------------------");


            int padLen = commands.Max(x => x.Name.Length) + 1;  //add 1 is an arbitrary number.
                                                                //Just gives it some extra padding
            foreach(var command in commands)
            {
                Console.Write(command.Name.PadRight(padLen));
                if(!string.IsNullOrWhiteSpace(command.Description))
                {
                    Console.WriteLine($": {command.Description}", Color.Gray);
                }
                else
                {
                    Console.WriteLine();
                }
            }
            
            Console.WriteLine();
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
            ConsoleHelper.WriteWithIndent(helpItem.Name, indent*IndentAmount, ColorH1);
            Console.WriteLine();
            ConsoleHelper.WriteWithIndent(new String(HeaderSeparatorChar, helpItem.Name.Length), indent*IndentAmount, Color.Gray);
            Console.WriteLine();

            ConsoleHelper.WriteWithIndent(helpItem.Description, indent*IndentAmount);
            Console.WriteLine();
            Console.WriteLine();

            ConsoleHelper.WriteWithIndent("Usage:", indent*IndentAmount, ColorH1);
            Console.WriteLine();

            string rootVerbs = string.Join(" ", helpItem.CommonTokens.Select(x => x.Name.Preferred));
            foreach(ICommandUsage usage in helpItem.Usages)
            {
                if(usage.IsHelp)
                {
                    continue;
                }
                WriteHelpItem(helpItem, usage, indent+1);
            }
        }

        public void WriteHelpItem(ICommandRoot root, ICommandUsage helpItem, int indent = 0)
        {
            string rootVerbs = string.Join(" ", root.CommonTokens.Select(x => x.Name.Preferred));
            string usageArgs = string.Join(" ", helpItem.Tokens.Select(x => GetOptionalDisplayName(x)));
            ConsoleHelper.WriteWithIndent($"{rootVerbs} {usageArgs}", indent*IndentAmount);
            Console.WriteLine();

            Console.WriteLine();
            ConsoleHelper.WriteWithIndent("Options:", (indent-1)*IndentAmount, ColorH1);
            Console.WriteLine();
            Console.WriteLine();

            int descriptionIndent = indent*IndentAmount + helpItem.Tokens.Max(x => x.DisplayName.Length);

            foreach(var token in helpItem.Tokens)
            {
                if(token.IsOptional)
                {
                    ConsoleHelper.WriteWithIndent($"{token.DisplayName}", indent*IndentAmount);
                    ConsoleHelper.WriteWithIndent("  # ", descriptionIndent, Color.Gray);
                    ConsoleHelper.WriteWithIndent(helpItem.GetDescription(token), descriptionIndent+4, Color.Gray);
                    Console.WriteLine();
                }
            }

            Console.WriteLine();
            ConsoleHelper.WriteWithIndent("Examples:", (indent-1)*IndentAmount, ColorH1);
            Console.WriteLine();
            Console.WriteLine();

            foreach(var example in helpItem.Examples)
            {
                ConsoleHelper.WriteWithIndent(example, indent*IndentAmount);
                Console.WriteLine();
            }
        }

        private static string GetOptionalDisplayName(ICommandToken token)
        {
            if(token.IsOptional)
            {
                return $"[{token.DisplayName}]";
            }
            else
            {
                return token.DisplayName;
            }
        }
    }
}