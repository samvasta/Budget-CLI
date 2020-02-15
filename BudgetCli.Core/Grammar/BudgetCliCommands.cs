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
                .WithArgument(TokenUtils.BuildArgInt("account-name")).Build();

        public static readonly OptionWithArgumentToken OPT_CATEGORY = new OptionWithArgumentToken.Builder()
            .Name("--category", "-c")
            .WithArgument(TokenUtils.BuildArgString("category")).Build();

        public static readonly OptionWithArgumentToken OPT_DESCRIPTION = new OptionWithArgumentToken.Builder()
            .Name("--description", "-d")
            .WithArgument(TokenUtils.BuildArgString("description")).Build();

        public static readonly OptionWithArgumentToken OPT_FUNDS = new OptionWithArgumentToken.Builder()
            .Name("--funds", "-f")
            .WithArgument(TokenUtils.BuildArgMoney("funds")).Build();

        public static readonly OptionWithArgumentToken OPT_PRIORITY = new OptionWithArgumentToken.Builder()
            .Name("--priority", "-p")
            .WithArgument(TokenUtils.BuildArgInt("priority")).Build();

        public static readonly OptionWithArgumentToken OPT_ACCOUNT_TYPE = new OptionWithArgumentToken.Builder()
            .Name("--type", "-y")
            .WithArgument(TokenUtils.BuildArgEnum<AccountKind>("account-type")).Build();

        public static readonly OptionWithArgumentToken OPT_DATE = new OptionWithArgumentToken.Builder()
            .Name("--date", "-d")
            .WithArgument(TokenUtils.BuildArgDate("date")).Build();
        public static readonly StandAloneOptionToken OPT_TREE = new StandAloneOptionToken(new Name("--tree", "-t"));
        public static readonly StandAloneOptionToken OPT_RECURSIVE = new StandAloneOptionToken(new Name("--recursive", "-r"));

        #endregion - Options -

        #endregion

        public static readonly CommandRoot CMD_HELP = new CommandRoot.Builder()
                .Id(CommandActionKind.Help)
                .WithToken(VERB_HELP)
                .Build();
        public static readonly CommandRoot CMD_VERSION = new CommandRoot.Builder()
                .Id(CommandActionKind.Version)
                .WithToken(VERB_VERSION)
                .Build();
        public static readonly CommandRoot CMD_CLEAR = new CommandRoot.Builder()
                .Id(CommandActionKind.Clear)
                .WithToken(VERB_CLEAR)
                .Build();
        public static readonly CommandRoot CMD_EXIT = new CommandRoot.Builder()
                .Id(CommandActionKind.Exit)
                .WithToken(VERB_EXIT)
                .Build();

        public static readonly CommandRoot CMD_NEW_ACCOUNT = new CommandRoot.Builder()
                .Id(CommandActionKind.AddAccount)
                .WithToken(VERB_NEW)
                .WithToken(VERB_ACCOUNT)
                .WithUsage(new CommandUsage.Builder()
                    .IsHelp()
                    .Description("Help").WithToken(OPT_HELP_REQUIRED)
                    .WithExample("new account --help").WithExample("n a -h")
                    .Build())
                .WithUsage(new CommandUsage.Builder()
                    .Description("New Account")
                    .WithToken(TokenUtils.BuildArgString(ARG_ACCOUNT_NAME))
                    .WithToken(OPT_CATEGORY)
                    .WithToken(OPT_DESCRIPTION)
                    .WithToken(OPT_FUNDS)
                    .WithToken(OPT_PRIORITY)
                    .WithToken(OPT_ACCOUNT_TYPE)
                    .Build())
                .Build();

        public static readonly CommandRoot CMD_LS_ACCOUNTS = new CommandRoot.Builder()
                .Id(CommandActionKind.ListAccount)
                .WithToken(VERB_LIST)
                .WithToken(VERB_ACCOUNT)
                .WithUsage(new CommandUsage.Builder()
                    .IsHelp()
                    .Description("Help").WithToken(OPT_HELP_REQUIRED)
                    .WithExample("ls accounts --help").WithExample("ls a -h")
                    .Build())
                .WithUsage(new CommandUsage.Builder()
                    .Description("List Accounts")
                    .WithToken(OPT_CATEGORY)
                    .WithToken(OPT_DESCRIPTION)
                    //TODO
                    // .WithToken(OPT_FUNDS_RNG)
                    // .WithToken(OPT_PRIORITY_RNG)
                    .WithToken(OPT_ACCOUNT_TYPE)
                    .WithToken(OPT_ACCOUNT_NAME)
                    .Build())
                .Build();
                
        public static readonly CommandRoot CMD_DETAIL_ACCOUNTS = new CommandRoot.Builder()
                .Id(CommandActionKind.DetailAccount)
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
                    .WithToken(OPT_DATE)
                    .Build())
                .Build();
                
        public static readonly CommandRoot CMD_REMOVE_ACCOUNTS = new CommandRoot.Builder()
                .Id(CommandActionKind.RemoveAccount)
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
                    .WithToken(OPT_RECURSIVE)
                    .Build())
                .Build();
                
        public static readonly CommandRoot CMD_SET_ACCOUNTS = new CommandRoot.Builder()
                .Id(CommandActionKind.UpdateAccount)
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
                    .WithToken(OPT_CATEGORY)
                    .WithToken(OPT_FUNDS)
                    .WithToken(OPT_DESCRIPTION)
                    .WithToken(OPT_PRIORITY)
                    .WithToken(OPT_ACCOUNT_TYPE)
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