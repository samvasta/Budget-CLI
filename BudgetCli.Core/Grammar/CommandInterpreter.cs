using System;
using BudgetCli.Core.Enums;
using BudgetCli.Core.Models.Commands;
using BudgetCli.Core.Models.Commands.SystemCommands;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Repositories;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models;
using BudgetCli.Parser.Parsing;

namespace BudgetCli.Core.Grammar
{
    public class CommandInterpreter
    {
        public RepositoryBag Repositories { get; }
        public CommandLibrary Commands { get; }

        public CommandInterpreter(RepositoryBag repositories, CommandLibrary library)
        {
            Repositories = repositories;
            Commands = library;
        }

        public bool TryParseCommand(string input, out ICommandAction action)
        {
            CommandUsageMatchData match = Commands.Parse(input);
            if(match == null)
            {
                action = null;
                return false;
            }

            action = BuildAction(input, match);
            return true;
        }

        private ICommandAction BuildAction(string input, CommandUsageMatchData matchData)
        {
            if(matchData.Usage.IsHelp)
            {
                return new HelpCommand(input, Repositories, (CommandActionKind)matchData.Command.CommandId);
            }

            if(matchData.Command == BudgetCliCommands.CMD_DETAIL_ACCOUNTS)
            {
                return BuildDetailAccountCommand(matchData);
            }
            if(matchData.Command == BudgetCliCommands.CMD_LS_ACCOUNTS)
            {
                return BuildListAccountCommand(matchData);
            }
            if(matchData.Command == BudgetCliCommands.CMD_NEW_ACCOUNT)
            {
                return BuildNewAccountCommand(matchData);
            }
            if(matchData.Command == BudgetCliCommands.CMD_REMOVE_ACCOUNTS)
            {
                return BuildRemoveAccountCommand(matchData);
            }
            if(matchData.Command == BudgetCliCommands.CMD_SET_ACCOUNTS)
            {
                return BuildSetAccountCommand(matchData);
            }

            return null;
        }

        private ICommandAction BuildDetailAccountCommand(CommandUsageMatchData matchData)
        {
            throw new NotImplementedException();
        }

        private ICommandAction BuildListAccountCommand(CommandUsageMatchData matchData)
        {
            throw new NotImplementedException();
        }

        private ICommandAction BuildNewAccountCommand(CommandUsageMatchData matchData)
        {
            throw new NotImplementedException();
        }

        private ICommandAction BuildRemoveAccountCommand(CommandUsageMatchData matchData)
        {
            throw new NotImplementedException();
        }

        private ICommandAction BuildSetAccountCommand(CommandUsageMatchData matchData)
        {
            throw new NotImplementedException();
        }
    }
}