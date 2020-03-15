using System;
using BudgetCli.Core.Enums;
using BudgetCli.Core.Models.Commands;
using BudgetCli.Core.Models.Commands.Accounts;
using BudgetCli.Core.Models.Commands.SystemCommands;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Repositories;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models;
using BudgetCli.Parser.Parsing;
using BudgetCli.Util.Models;

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
                return new HelpCommand(input, Repositories, matchData.Command);
            }

            if(matchData.Command == BudgetCliCommands.CMD_CLEAR)
            {
                return new SystemCommand(input, CommandKind.ClearConsole);
            }
            if(matchData.Command == BudgetCliCommands.CMD_EXIT)
            {
                return new SystemCommand(input, CommandKind.Exit);
            }
            if(matchData.Command == BudgetCliCommands.CMD_HELP)
            {
                return new SystemCommand(input, CommandKind.Help);
            }
            if(matchData.Command == BudgetCliCommands.CMD_VERSION)
            {
                return new SystemCommand(input, CommandKind.Version);
            }

            if(matchData.Command == BudgetCliCommands.CMD_DETAIL_ACCOUNTS)
            {
                return BuildDetailAccountCommand(input, matchData);
            }
            if(matchData.Command == BudgetCliCommands.CMD_LS_ACCOUNTS)
            {
                return BuildListAccountCommand(input, matchData);
            }
            if(matchData.Command == BudgetCliCommands.CMD_NEW_ACCOUNT)
            {
                return BuildNewAccountCommand(input, matchData);
            }
            if(matchData.Command == BudgetCliCommands.CMD_REMOVE_ACCOUNTS)
            {
                return BuildRemoveAccountCommand(input, matchData);
            }
            if(matchData.Command == BudgetCliCommands.CMD_SET_ACCOUNTS)
            {
                return BuildSetAccountCommand(input, matchData);
            }

            return null;
        }

        private ICommandAction BuildDetailAccountCommand(string input, CommandUsageMatchData matchData)
        {
            DetailAccountCommand cmd = new DetailAccountCommand(input, Repositories);
            
            string name;
            if(matchData.TryGetArgValue(BudgetCliCommands.ARG_ACCOUNT_NAME, out name))
            {
                cmd.NameOption.SetData(name);
            }

            DateTime date;
            if(matchData.TryGetArgValue(BudgetCliCommands.OPT_DATE.Arguments[0], out date))
            {
                cmd.DateOption.SetData(date);
            }

            return cmd;
        }

        private ICommandAction BuildListAccountCommand(string input, CommandUsageMatchData matchData)
        {
            ListAccountCommand cmd = new ListAccountCommand(input, Repositories);
            
            AccountKind accountType;
            if(matchData.TryGetArgValue(BudgetCliCommands.OPT_ACCOUNT_TYPE.Arguments[0], out accountType))
            {
                cmd.AccountTypeOption.SetData(accountType);
            }

            string categoryName;
            if(matchData.TryGetArgValue(BudgetCliCommands.OPT_CATEGORY.Arguments[0], out categoryName))
            {
                cmd.CategoryIdOption.SetData(Repositories.AccountRepository.GetIdByName(categoryName));
            }

            string description;
            if(matchData.TryGetArgValue(BudgetCliCommands.OPT_DESCRIPTION.Arguments[0], out description))
            {
                cmd.DescriptionOption.SetData(description);
            }

            string name;
            if(matchData.TryGetArgValue(BudgetCliCommands.OPT_ACCOUNT_NAME.Arguments[0], out name))
            {
                cmd.NameOption.SetData(name);
            }

            Range<Money> fundsRange;
            if(matchData.TryGetArgValue(BudgetCliCommands.OPT_FUNDS_RANGE.Arguments[0], out fundsRange))
            {
                cmd.FundsOption.SetData(fundsRange);
            }

            Range<long> priorityRange;
            if(matchData.TryGetArgValue(BudgetCliCommands.OPT_PRIORITY_RANGE.Arguments[0], out priorityRange))
            {
                cmd.PriorityOption.SetData(priorityRange);
            }

            return cmd;
        }

        private ICommandAction BuildNewAccountCommand(string input, CommandUsageMatchData matchData)
        {
            string accountName;

            if(!matchData.TryGetArgValue(BudgetCliCommands.ARG_ACCOUNT_NAME, out accountName))
            {
                return null;
            }

            AddAccountCommand cmd = new AddAccountCommand(input, Repositories, accountName);

            string categoryName;
            if(matchData.TryGetArgValue(BudgetCliCommands.OPT_CATEGORY.Arguments[0], out categoryName))
            {
                cmd.CategoryNameOption.SetData(categoryName);
            }

            string description;
            if(matchData.TryGetArgValue(BudgetCliCommands.OPT_DESCRIPTION.Arguments[0], out description))
            {
                cmd.DescriptionOption.SetData(description);
            }

            Money funds;
            if(matchData.TryGetArgValue(BudgetCliCommands.OPT_FUNDS.Arguments[0], out funds))
            {
                cmd.FundsOption.SetData(funds);
            }

            AccountKind accountKind;
            if(matchData.TryGetArgValue(BudgetCliCommands.OPT_ACCOUNT_TYPE.Arguments[0], out accountKind))
            {
                cmd.AccountTypeOption.SetData(accountKind);
            }

            int priority;
            if(matchData.TryGetArgValue(BudgetCliCommands.OPT_PRIORITY.Arguments[0], out priority))
            {
                cmd.PriorityOption.SetData(priority);
            }

            return cmd;
        }

        private ICommandAction BuildRemoveAccountCommand(string input, CommandUsageMatchData matchData)
        {
            DeleteAccountCommand cmd = new DeleteAccountCommand(input, Repositories);

            string accountName;
            if(!matchData.TryGetArgValue(BudgetCliCommands.ARG_ACCOUNT_NAME, out accountName))
            {
                return null;
            }

            cmd.AccountName.SetData(accountName);

            if(matchData.HasToken(BudgetCliCommands.OPT_RECURSIVE))
            {
                cmd.IsRecursiveOption.SetData(true);
            }

            return cmd;
        }

        private ICommandAction BuildSetAccountCommand(string input, CommandUsageMatchData matchData)
        {
            throw new NotImplementedException();
        }
    }
}