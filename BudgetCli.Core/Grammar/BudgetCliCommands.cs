using System;
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
        #region - Tokens -

        #region - Verbs -
        private static readonly VerbToken HELP_VERB = new VerbToken(new Name("help", "h"));

        private static readonly VerbToken LIST = new VerbToken(new Name("list", "ls"));
        private static readonly VerbToken DETAIL = new VerbToken(new Name("detail", "details", "d"));
        private static readonly VerbToken NEW = new VerbToken(new Name("new", "n", "add", "a"));
        private static readonly VerbToken REMOVE = new VerbToken(new Name("remove", "rm", "delete", "del"));
        
        private static readonly VerbToken SET = new VerbToken(new Name("set"));

        private static readonly VerbToken ACCOUNT = new VerbToken(new Name("account", "accounts", "a"));
        private static readonly VerbToken TRANSACTION = new VerbToken(new Name("transaction", "transactions", "t", "tran"));
        private static readonly VerbToken HISTORY = new VerbToken(new Name("history", "h"));

        #endregion - Verbs

        #region - Options -
        
        private static readonly StandAloneOptionToken OPT_HELP = new StandAloneOptionToken(new Name("--help", "-h"));

        private static readonly OptionWithArgumentToken OPT_ACCOUNT_NAME = new OptionWithArgumentToken.Builder()
                .Name("--name", "-n")
                .WithArgument(TokenUtils.BuildArgInt("account-name")).Build();

        private static readonly OptionWithArgumentToken OPT_CATEGORY = new OptionWithArgumentToken.Builder()
            .Name("--category", "-c")
            .WithArgument(TokenUtils.BuildArgString("category")).Build();

        private static readonly OptionWithArgumentToken OPT_DESCRIPTION = new OptionWithArgumentToken.Builder()
            .Name("--description", "-d")
            .WithArgument(TokenUtils.BuildArgString("description")).Build();

        private static readonly OptionWithArgumentToken OPT_FUNDS = new OptionWithArgumentToken.Builder()
            .Name("--funds", "-f")
            .WithArgument(TokenUtils.BuildArgMoney("funds")).Build();

        private static readonly OptionWithArgumentToken OPT_PRIORITY = new OptionWithArgumentToken.Builder()
            .Name("--priority", "-p")
            .WithArgument(TokenUtils.BuildArgInt("priority")).Build();

        private static readonly OptionWithArgumentToken OPT_ACCOUNT_TYPE = new OptionWithArgumentToken.Builder()
            .Name("--type", "-y")
            .WithArgument(TokenUtils.BuildArgEnum<AccountKind>("account-type")).Build();

        private static readonly OptionWithArgumentToken OPT_DATE = new OptionWithArgumentToken.Builder()
            .Name("--date", "-d")
            .WithArgument(TokenUtils.BuildArgDate("date")).Build();
        private static readonly StandAloneOptionToken OPT_TREE = new StandAloneOptionToken(new Name("--tree", "-t"));
        private static readonly StandAloneOptionToken OPT_RECURSIVE = new StandAloneOptionToken(new Name("--recursive", "-r"));

        #endregion - Options -

        #endregion


        private static readonly CommandRoot CMD_NEW_ACCOUNT = new CommandRoot.Builder()
                .WithToken(NEW)
                .WithToken(ACCOUNT)
                .WithUsage(new CommandUsage.Builder()
                    .Description("Help").WithToken(OPT_HELP)
                    .WithExample("new account --help").WithExample("n a -h")
                    .Build())
                .WithUsage(new CommandUsage.Builder()
                    .Description("New Account")
                    .WithToken(TokenUtils.BuildArgString("account-name"))
                    .WithToken(OPT_CATEGORY)
                    .WithToken(OPT_DESCRIPTION)
                    .WithToken(OPT_FUNDS)
                    .WithToken(OPT_PRIORITY)
                    .WithToken(OPT_ACCOUNT_TYPE)
                    .Build())
                .Build();

        private static readonly CommandRoot CMD_LS_ACCOUNTS = new CommandRoot.Builder()
                .WithToken(LIST)
                .WithToken(ACCOUNT)
                .WithUsage(new CommandUsage.Builder()
                    .Description("Help").WithToken(OPT_HELP)
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
                
        private static readonly CommandRoot CMD_DETAIL_ACCOUNTS = new CommandRoot.Builder()
                .WithToken(DETAIL)
                .WithToken(ACCOUNT)
                .WithUsage(new CommandUsage.Builder()
                    .Description("Help").WithToken(OPT_HELP)
                    .WithExample("detail accounts --help").WithExample("d a -h")
                    .Build())
                .WithUsage(new CommandUsage.Builder()
                    .Description("Display the details of an account. Optionally specify a date to see the funds in the account at a specific date.")
                    .WithToken(TokenUtils.BuildArgString("account-name"))
                    .WithToken(OPT_DATE)
                    .Build())
                .Build();
                
        private static readonly CommandRoot CMD_REMOVE_ACCOUNTS = new CommandRoot.Builder()
                .WithToken(REMOVE)
                .WithToken(ACCOUNT)
                .WithUsage(new CommandUsage.Builder()
                    .Description("Help").WithToken(OPT_HELP)
                    .WithExample("remove account --help").WithExample("rm a -h")
                    .Build())
                .WithUsage(new CommandUsage.Builder()
                    .Description("Removes the specified account. The account will not be deleted; instead it will be marked as \"closed\" and will not be effected by other commands until re-opened.")
                    .WithToken(TokenUtils.BuildArgString("account-name"))
                    .WithToken(OPT_RECURSIVE)
                    .Build())
                .Build();

        public static CommandLibrary BuildCommandLibrary()
        {
            CommandLibrary library = new CommandLibrary();

            library.AddCommand(CMD_NEW_ACCOUNT)
                   .AddCommand(CMD_DETAIL_ACCOUNTS)
                   .AddCommand(CMD_LS_ACCOUNTS)
                   .AddCommand(CMD_REMOVE_ACCOUNTS);

            return library;
        }
        
    }
}