using System;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Parsing;
using BudgetCli.Parser.Util;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Grammar
{
    public class BudgetCliCommands
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
                .Name("--account", "-a")
                .WithArgument(TokenUtils.BuildArgInt("account-name")).Build();

        #endregion - Options -

        #endregion


        private static readonly CommandRoot CMD_NEW_ACCOUNT = new CommandRoot.Builder()
                .WithToken(NEW)
                .WithToken(ACCOUNT)
                .WithUsage(new CommandUsage.Builder().Description("Help").WithToken(OPT_HELP).WithExample("new account --help").WithExample("n a -h").Build())
                .Build();

        private CommandLibrary _commands { get; }

        public BudgetCliCommands()
        {
            //TODO Instantiate command library
        }

        
    }
}