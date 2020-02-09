using System;
using System.IO;
using BudgetCli.ConsoleApp.Interfaces;
using BudgetCli.ConsoleApp.Writers;
using BudgetCli.Core.Grammar;
using BudgetCli.Core.Models.Commands;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Data.IO;
using BudgetCli.Data.Util;
using BudgetCli.Parser.Parsing;
using ReadLine = System.ReadLine;

namespace BudgetCli.ConsoleApp.App
{
    public class App : IExitListener
    {
        public string Prompt { get; set; } = ">";

        private readonly ICommandActionListener[] _listeners;

        private bool _continueLoop;
        private readonly CommandInterpreter _interpreter;

        public App(FileInfo dbInfo, CommandLibrary commandLibrary)
        {
            _listeners = new [] { new ConsoleCommandActionListener(this, new HelpInfoConsoleWriter(), commandLibrary) };

            ReadLine.HistoryEnabled = true;
            ReadLine.AutoCompletionHandler = new AutoCompletionHandler();
            _continueLoop = true;
            _interpreter = new CommandInterpreter(RepositoryBagUtil.GetRuntimeRepositoryBag(dbInfo, null), commandLibrary);
        }

        void IExitListener.Exit()
        {
            _continueLoop = false;
        }

        public void StartInteractiveShell()
        {
            while(_continueLoop)
            {
                string input = ReadLine.Read(Prompt);

                ICommandAction command;
                if(_interpreter.TryParseCommand(input, out command))
                {
                    if(command.TryExecute(null, _listeners))
                    {
                        ReadLine.AddHistory(input);
                    }
                }
                else
                {
                    Console.WriteLine("Unrecognized command");
                }
            }
        }

        public static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                Console.WriteLine("Expected a path to a .budget file.");
                return;
            }

            string fileName = args[0];
            FileInfo dbInfo = null;
            try
            {
                dbInfo = new System.IO.FileInfo(fileName);
                if(!File.Exists(fileName))
                {
                    DbHelper.CreateSQLiteFile(args[0], null);
                }

                new App(dbInfo, BudgetCliCommands.BuildCommandLibrary()).StartInteractiveShell();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Invalid file path: " + e.Message);
            }
            catch (PathTooLongException e)
            {
                Console.WriteLine("Invalid file path: " + e.Message);
            }
            catch (NotSupportedException e)
            {
                Console.WriteLine("Invalid file path: " + e.Message);
            }
        }
    }
}