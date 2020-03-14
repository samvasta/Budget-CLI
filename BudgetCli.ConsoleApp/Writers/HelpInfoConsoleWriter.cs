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

        public int IndentAmount { get; set; }
        public char HeaderSeparatorChar { get; set; }

        public HelpInfoConsoleWriter(int indentAmount = 8, char headerSeparatorChar = '-')
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
            
        }
    }
}