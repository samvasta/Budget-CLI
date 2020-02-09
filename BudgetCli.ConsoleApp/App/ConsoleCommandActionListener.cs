using System;
using BudgetCli.Core.Models.CommandResults;
using BudgetCli.Core.Models.Interfaces;
using System.Linq;
using Humanizer;
using System.Collections.Generic;
using BudgetCli.Core.Models.ModelInfo;
using ConsoleTables;
using BudgetCli.Core.Enums;
using BudgetCli.ConsoleApp.Interfaces;
using BudgetCli.Core.Models.Commands.SystemCommands;
using BudgetCli.ConsoleApp.Writers;
using BudgetCli.Parser.Parsing;

namespace BudgetCli.ConsoleApp.App
{
    public class ConsoleCommandActionListener : ICommandActionListener
    {
        public IExitListener ExitListener { get; }

        public HelpInfoConsoleWriter HelpInfoWriter { get; }

        public CommandLibrary CommandLibrary { get; }

        public ConsoleCommandActionListener(IExitListener exitListener, HelpInfoConsoleWriter helpInfoWriter, CommandLibrary commandLibrary)
        {
            ExitListener = exitListener;
            HelpInfoWriter = helpInfoWriter;
            CommandLibrary = commandLibrary;
        }

        public bool ConfirmAction(string confirmationMessage)
        {
            string answer;
            do
            {
                Console.WriteLine($"{confirmationMessage} (y/n)");
                answer = ReadLine.Read(@default:"Y");
            } while (answer.ToLower().StartsWith('y') || answer.ToLower().StartsWith('n'));

            return answer.ToLower().StartsWith('y');
        }

        public void OnCommand(ICommandResult result)
        {
            //Automagically figure out which of the more specific methods to use based on the concrete type of result
            OnCommand((dynamic) result);
        }

        public void OnCommand(SystemCommandResult result)
        {
            if(result.CommandKind == CommandKind.Exit)
            {
                Console.WriteLine("Goodbye.");
                ExitListener?.Exit();
            }
            else if(result.CommandKind == CommandKind.Version)
            {
                Console.WriteLine($"Version {System.Reflection.Assembly.GetEntryAssembly().GetName().Version}");
            }
            else if(result.CommandKind == CommandKind.ClearConsole)
            {
                Console.Clear();
            }
            else if(result.CommandKind == CommandKind.Help)
            {
                HelpCommand helpCommand = (HelpCommand)result.Command;
                //TODO: Get ICommandRoot and pass to HelpInfoWriter
            }
        }

        public void OnCommand<T>(CreateCommandResult<T> result) where T : IDetailable
        {
            if(result.IsSuccessful)
            {
                Console.WriteLine($"{result.CreatedItem.TypeName} \"{result.CreatedItem.DisplayName}\" successfully created.");
            }
            else
            {
                Console.WriteLine($"Something went wrong. Failed to create object");
            }
        }

        public void OnCommand<T>(DeleteCommandResult<T> result) where T : IDetailable
        {
            if(result.IsSuccessful)
            {
                string typeName = result.DeletedItems.First().TypeName;
                Console.WriteLine($"Successfully deleted {result.DeletedItems.Count()} {typeName.Pluralize(true)}:");
                foreach(var item in result.DeletedItems)
                {
                    Console.WriteLine($"\t• \"{item.DisplayName}\"");
                }
            }
            else
            {
                Console.WriteLine($"Something went wrong. Failed to delete object");
            }
        }

        public void OnCommand<T>(ReadCommandResult<T> result) where T : IListable
        {
            if(result.IsSuccessful)
            {
                int count = result.FilteredItems.Count();
                Console.WriteLine($"Successfully found {result.FilteredItems.First().TypeName.ToQuantity(count)}.");

                if(result.Criteria.Fields.Any())
                {
                    Console.WriteLine("Criteria:");
                    int i = 1;
                    foreach(var criteria in result.Criteria.Fields)
                    {
                        Console.WriteLine($"\t{i++}) {criteria.Key} {criteria.Value}");
                    }

                    Console.WriteLine();
                }

                ConsoleTable table = new ConsoleTable();

                Dictionary<T, List<object>> rows = new Dictionary<T, List<object>>();
                bool firstRow = true;
                foreach(var item in result.FilteredItems)
                {
                    var properties = item.GetPropertyValues().Where(x => x.Property.IsVisibleInList);
                    if(firstRow)
                    {
                        table.AddColumn(properties.Select(x => x.Property.DisplayName));
                        firstRow = false;
                    }
                    table.AddRow(properties.Select(x => x.Value.ToString()).ToArray());
                }

                table.Write(Format.Minimal);
            }
            else
            {
                Console.WriteLine($"Something went wrong. Failed to delete object");
            }
        }

        public void OnCommand<T>(ReadDetailsCommandResult<T> result) where T : IDetailable
        {
            if(result.IsSuccessful)
            {
                string typeName = result.Item.TypeName;

                foreach(var propValue in result.Item.GetPropertyValues().Where(x => x.Property.IsVisibleInDetails))
                {
                    WritePropertyValueDetails(propValue);
                }
            }
            else
            {
                Console.WriteLine($"Something went wrong. Failed to delete object");
            }
        }

        public void OnCommand<T>(UpdateCommandResult<T> result) where T : IDetailable
        {
            throw new System.NotImplementedException();
        }

        public void WritePropertyValueDetails(ModelPropertyValue propertyValue)
        {
            //TODO
        }

    }
}