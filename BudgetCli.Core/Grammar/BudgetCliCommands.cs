using System;
using BudgetCli.Core.Enums;
using BudgetCli.Data.Enums;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Parsing;
using BudgetCli.Parser.Util;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Grammar
{
    public static class BudgetCliCommands
    {
        public const string ARG_ACCOUNT_NAME = "account-name";
        #region - Tokens -

        #region - Verbs -
        public static readonly VerbToken VERB_HELP = new VerbToken(new Name("help"));
        public static readonly VerbToken VERB_CLEAR = new VerbToken(new Name("clear"));
        public static readonly VerbToken VERB_VERSION = new VerbToken(new Name("version"));
        public static readonly VerbToken VERB_EXIT = new VerbToken(new Name("exit"));

        public static readonly VerbToken VERB_LIST = new VerbToken(new Name("list", "ls"));
        public static readonly VerbToken VERB_DETAIL = new VerbToken(new Name("detail", "details", "d"));
        public static readonly VerbToken VERB_NEW = new VerbToken(new Name("new", "n", "add", "a"));
        public static readonly VerbToken VERB_REMOVE = new VerbToken(new Name("remove", "rm", "delete", "del"));

        public static readonly VerbToken VERB_SET = new VerbToken(new Name("set"));

        public static readonly VerbToken VERB_ACCOUNT = new VerbToken(new Name("account", "accounts", "a"));
        public static readonly VerbToken VERB_TRANSACTION = new VerbToken(new Name("transaction", "transactions", "t", "tran"));
        public static readonly VerbToken VERB_HISTORY = new VerbToken(new Name("history", "h"));

        #endregion - Verbs

        #region - Options -
        
        public static readonly VerbToken OPT_HELP_REQUIRED = new VerbToken(new Name("--help", "-h"));

        public static readonly OptionWithArgumentToken OPT_ACCOUNT_NAME = new OptionWithArgumentToken.Builder()
                .Name("--name", "-n")
                .WithArgument(TokenUtils.BuildArgString("account-name")).Build();

        public static readonly OptionWithArgumentToken OPT_CATEGORY = new OptionWithArgumentToken.Builder()
            .Name("--category", "-c")
            .WithArgument(TokenUtils.BuildArgString("category")).Build();

        public static readonly OptionWithArgumentToken OPT_DESCRIPTION = new OptionWithArgumentToken.Builder()
            .Name("--description", "-d")
            .WithArgument(TokenUtils.BuildArgString("description")).Build();

        public static readonly OptionWithArgumentToken OPT_FUNDS = new OptionWithArgumentToken.Builder()
            .Name("--funds", "-f")
            .WithArgument(TokenUtils.BuildArgMoney("funds")).Build();

        public static readonly OptionWithArgumentToken OPT_FUNDS_RANGE = new OptionWithArgumentToken.Builder()
            .Name("--funds", "-f")
            .WithArgument(TokenUtils.BuildArgMoneyRange("funds")).Build();

        public static readonly OptionWithArgumentToken OPT_PRIORITY = new OptionWithArgumentToken.Builder()
            .Name("--priority", "-p")
            .WithArgument(TokenUtils.BuildArgInt("priority")).Build();

        public static readonly OptionWithArgumentToken OPT_PRIORITY_RANGE = new OptionWithArgumentToken.Builder()
            .Name("--priority", "-p")
            .WithArgument(TokenUtils.BuildArgLongRange("priority")).Build();

        public static readonly OptionWithArgumentToken OPT_ACCOUNT_TYPE = new OptionWithArgumentToken.Builder()
            .Name("--type", "-y")
            .WithArgument(TokenUtils.BuildArgEnum<AccountKind>("account-type")).Build();

        public static readonly OptionWithArgumentToken OPT_DATE = new OptionWithArgumentToken.Builder()
            .Name("--date", "-d")
            .WithArgument(TokenUtils.BuildArgDate("date")).Build();

        public static readonly OptionWithArgumentToken OPT_TAG = new OptionWithArgumentToken.Builder()
            .Name("--tag", "-g")
            .WithArgument(TokenUtils.BuildArgString("tag")).Build();

        public static readonly StandAloneOptionToken OPT_TREE = new StandAloneOptionToken(new Name("--tree", "-t"));
        public static readonly StandAloneOptionToken OPT_RECURSIVE = new StandAloneOptionToken(new Name("--recursive", "-r"));

        #endregion - Options -

        #endregion

        public static readonly CommandRoot CMD_HELP = new CommandRoot.Builder()
                .Id(CommandActionKind.Help)
                .Description("List available commands.")
                .WithToken(VERB_HELP)
                .Build();
        public static readonly CommandRoot CMD_VERSION = new CommandRoot.Builder()
                .Id(CommandActionKind.Version)
                .Description("Print the current app version.")
                .WithToken(VERB_VERSION)
                .Build();
        public static readonly CommandRoot CMD_CLEAR = new CommandRoot.Builder()
                .Id(CommandActionKind.Clear)
                .Description("Clear the console.")
                .WithToken(VERB_CLEAR)
                .Build();
        public static readonly CommandRoot CMD_EXIT = new CommandRoot.Builder()
                .Id(CommandActionKind.Exit)
                .Description("Exit the app.")
                .WithToken(VERB_EXIT)
                .Build();

        public static readonly CommandRoot CMD_NEW_ACCOUNT = new CommandRoot.Builder()
                .Id(CommandActionKind.AddAccount)
                .Description("Add a new account.")
                .WithToken(VERB_NEW)
                .WithToken(VERB_ACCOUNT)
                .WithUsage(new CommandUsage.Builder()
                    .IsHelp()
                    .Description("Help").WithToken(OPT_HELP_REQUIRED)
                    .WithExample("new account --help").WithExample("n a -h")
                    .Build())
                .WithUsage(new CommandUsage.Builder()
                    .Description("New Account")
                    .WithToken(TokenUtils.BuildArgString(ARG_ACCOUNT_NAME), "name of the new account")
                    .WithToken(OPT_CATEGORY, "name of the category the new account will belong to")
                    .WithToken(OPT_DESCRIPTION, "a brief description of the account")
                    .WithToken(OPT_FUNDS, "initial funds in the account")
                    .WithToken(OPT_PRIORITY, "used for operations like filtering and sorting. Lower values are higher priority", 5)
                    .WithToken(OPT_ACCOUNT_TYPE, "Sink typically collects funds, Source typically distributes funds, Category contains other accounts", AccountKind.Sink)
                    .WithExample("new account \"account name\"")
                    .WithExample("new account \"account name\" -d \"description\"")
                    .WithExample("new account \"account name\" -f $250 -d \"groceries\" -y Sink")
                    .Build())
                .Build();

        public static readonly CommandRoot CMD_LS_ACCOUNTS = new CommandRoot.Builder()
                .Id(CommandActionKind.ListAccount)
                .Description("List all accounts.")
                .WithToken(VERB_LIST)
                .WithToken(VERB_ACCOUNT)
                .WithUsage(new CommandUsage.Builder()
                    .IsHelp()
                    .Description("Help").WithToken(OPT_HELP_REQUIRED)
                    .WithExample("ls accounts --help").WithExample("ls a -h")
                    .Build())
                .WithUsage(new CommandUsage.Builder()
                    .Description("List Accounts")
                    .WithToken(OPT_CATEGORY, "filter by accounts belonging to this category")
                    .WithToken(OPT_DESCRIPTION, "filter by accounts with this in their description")
                    .WithToken(OPT_FUNDS_RANGE, "filter by accounts with funds in this range")
                    .WithToken(OPT_PRIORITY_RANGE, "filter by accounts with priority in this range")
                    .WithToken(OPT_ACCOUNT_TYPE, "filter by accounts of this type")
                    .WithToken(OPT_ACCOUNT_NAME, "filter by accounts with this in their name")
                    .WithToken(OPT_TREE, "display results as a visual tree")
                    .Build())
                .Build();
                
        public static readonly CommandRoot CMD_DETAIL_ACCOUNTS = new CommandRoot.Builder()
                .Id(CommandActionKind.DetailAccount)
                .Description("Show details for a particular account.")
                .WithToken(VERB_DETAIL)
                .WithToken(VERB_ACCOUNT)
                .WithUsage(new CommandUsage.Builder()
                    .IsHelp()
                    .Description("Help").WithToken(OPT_HELP_REQUIRED)
                    .WithExample("detail accounts --help").WithExample("d a -h")
                    .Build())
                .WithUsage(new CommandUsage.Builder()
                    .Description("Display the details of an account. Optionally specify a date to see the funds in the account at a specific date.")
                    .WithToken(TokenUtils.BuildArgString(ARG_ACCOUNT_NAME))
                    .WithToken(OPT_DATE, "show the account's state on this date")
                    .Build())
                .Build();
                
        public static readonly CommandRoot CMD_REMOVE_ACCOUNTS = new CommandRoot.Builder()
                .Id(CommandActionKind.RemoveAccount)
                .Description("Close an account.")
                .WithToken(VERB_REMOVE)
                .WithToken(VERB_ACCOUNT)
                .WithUsage(new CommandUsage.Builder()
                    .IsHelp()
                    .Description("Help").WithToken(OPT_HELP_REQUIRED)
                    .WithExample("remove account --help").WithExample("rm a -h")
                    .Build())
                .WithUsage(new CommandUsage.Builder()
                    .Description("Removes the specified account. The account will not be deleted; instead it will be marked as \"closed\" and will not be effected by other commands until re-opened.")
                    .WithToken(TokenUtils.BuildArgString(ARG_ACCOUNT_NAME))
                    .WithToken(OPT_RECURSIVE, "remove this account and all accounts under it. Only applied to Category accounts")
                    .Build())
                .Build();
                
        public static readonly CommandRoot CMD_SET_ACCOUNTS = new CommandRoot.Builder()
                .Id(CommandActionKind.UpdateAccount)
                .Description("Update properties of a particular account.")
                .WithToken(VERB_SET)
                .WithToken(VERB_ACCOUNT)
                .WithUsage(new CommandUsage.Builder()
                    .IsHelp()
                    .Description("Help").WithToken(OPT_HELP_REQUIRED)
                    .WithExample("set account --help").WithExample("set a -h")
                    .Build())
                .WithUsage(new CommandUsage.Builder()
                    .Description("Change one or more fields of the account.")
                    .WithToken(TokenUtils.BuildArgString(ARG_ACCOUNT_NAME))
                    .WithToken(OPT_CATEGORY, "set the category to a new value")
                    .WithToken(OPT_FUNDS, "set the funds to a new value")
                    .WithToken(OPT_DESCRIPTION, "set the description to a new value")
                    .WithToken(OPT_PRIORITY, "set the priority to a new value")
                    .WithToken(OPT_ACCOUNT_TYPE, "set the account type to a new value")
                    .Build())
                .Build();

        public static CommandLibrary BuildCommandLibrary()
        {
            CommandLibrary library = new CommandLibrary();

            library.AddCommand(CMD_HELP)
                   .AddCommand(CMD_VERSION)
                   .AddCommand(CMD_CLEAR)
                   .AddCommand(CMD_EXIT)
                   .AddCommand(CMD_NEW_ACCOUNT)
                   .AddCommand(CMD_DETAIL_ACCOUNTS)
                   .AddCommand(CMD_LS_ACCOUNTS)
                   .AddCommand(CMD_REMOVE_ACCOUNTS)
                   .AddCommand(CMD_SET_ACCOUNTS)
                   ;

            return library;
        }
        
    }
}