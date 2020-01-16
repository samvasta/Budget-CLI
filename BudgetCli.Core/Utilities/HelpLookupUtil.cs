using System;
using System.Linq;
using BudgetCli.Core.Enums;
using BudgetCli.Core.Grammar;
using BudgetCli.Core.Models.InfoItems;
using BudgetCli.Data.Enums;
using Humanizer;

namespace BudgetCli.Core.Utilities
{
    public static class HelpLookupUtil
    {
        public static readonly string[] ALL_COMMANDS = new string[] 
        {
            "Help",
            "Version",
            "Clear",
            "Exit",
            "",
            "cat account",
            "ls accounts",
            "new account",
            "remove account",
            "move account",
            "set account",
            "",
            "cat transaction",
            "ls transactions",
            "new transaction",
            "",
            "ls history",
        };

        #region - Usage Tokens -

        private static readonly HelpUsageToken USAGE_OPT_ACCOUNT = new HelpUsageToken("-a <account-name>");
        private static readonly HelpUsageToken USAGE_OPT_AMOUNT_EXPR = new HelpUsageToken("-a <amount>");
        private static readonly HelpUsageToken USAGE_OPT_CATEGORY = new HelpUsageToken("-c <category>");
        private static readonly HelpUsageToken USAGE_OPT_COUNT = new HelpUsageToken("-c <count>");
        private static readonly HelpUsageToken USAGE_OPT_DATE = new HelpUsageToken("-d <date>");
        private static readonly HelpUsageToken USAGE_OPT_DATE_EXPR = new HelpUsageToken("-d <date-expression>");
        private static readonly HelpUsageToken USAGE_OPT_DESCRIPTION = new HelpUsageToken("-d <description>");
        private static readonly HelpUsageToken USAGE_OPT_DEST = new HelpUsageToken("-d <destination-account>");
        private static readonly HelpUsageToken USAGE_OPT_FUNDS = new HelpUsageToken("-f <funds>");
        private static readonly HelpUsageToken USAGE_OPT_FUNDS_EXPR = new HelpUsageToken("-f <funds-expression>");
        private static readonly HelpUsageToken USAGE_OPT_HELP = new HelpUsageToken("-h");
        private static readonly HelpUsageToken USAGE_OPT_ID = new HelpUsageToken("-i <id>");
        private static readonly HelpUsageToken USAGE_OPT_NAME = new HelpUsageToken("-n <name>");
        private static readonly HelpUsageToken USAGE_OPT_PRIORITY = new HelpUsageToken("-p <priority>");
        private static readonly HelpUsageToken USAGE_OPT_PRIORITY_EXPR = new HelpUsageToken("-p <priority-expression>");
        private static readonly HelpUsageToken USAGE_OPT_RECURSIVE = new HelpUsageToken("-r");
        private static readonly HelpUsageToken USAGE_OPT_SOURCE = new HelpUsageToken("-s <source-account>");
        private static readonly HelpUsageToken USAGE_OPT_ACCOUNT_TYPE = new HelpUsageToken("-t <account-type>");
        private static readonly HelpUsageToken USAGE_OPT_TRANSACTION_TYPE = new HelpUsageToken("-t <transaction-type>");

        #endregion - Usage Tokens -

        #region - Options -
        private static readonly HelpOptionInfoItem OPT_CATEGORY = new HelpInfoOptionBuilder()
            .OptionName("-c")
            .Description("Name of the category account the new account will belong to")
            .WithAltName("--category")
            .WithArgTypes("string")
            .Build();
        private static readonly HelpOptionInfoItem OPT_DESCRIPTION = new HelpInfoOptionBuilder()
            .OptionName("-d")
            .Description("A brief description of the account")
            .WithAltName("--description")
            .WithArgTypes("string")
            .Build();
        private static readonly HelpOptionInfoItem OPT_FUNDS = new HelpInfoOptionBuilder()
            .OptionName("-f")
            .Description("Initial funds in the account")
            .WithAltName("--funds")
            .DefaultValue("$0.00")
            .WithArgTypes("number")
            .Build();
        private static readonly HelpOptionInfoItem OPT_PRIORITY = new HelpInfoOptionBuilder()
            .OptionName("-p")
            .Description("A relative priority than may be used for sorting accounts and other operations. 1 is the highest priority.")
            .DefaultValue("5")
            .WithPossibleValue("1 - 100")
            .WithAltName("--priority")
            .WithArgTypes("int")
            .Build();

        private static readonly HelpOptionInfoItem OPT_ACCOUNT_TYPE = new HelpInfoOptionBuilder()
            .OptionName("-t")
            .Description("Account type describes the general purpose of the account. Sinks generally receive funds, Sources generally distribute funds, and Categories hold other accounts.")
            .DefaultValue(AccountKind.Sink.Humanize())
            .WithPossibleValues(((AccountKind[])Enum.GetValues(typeof(AccountKind))).Select(x => x.Humanize()))
            .WithAltName("--type")
            .WithArgTypes("account-type")
            .Build();

        #endregion - Options -


        public static HelpLookup GetRuntimeHelpLookup()
        {
            HelpLookup lookup = new HelpLookup();


            #region - System -
            lookup.AddHelp(CommandActionKind.Help, new HelpItemBuilder()
            .Header("Help")
            .Description($"List available commands is provided in the examples section. You may also append \"-h\" or \"--help\" after any command to get more information about that specific command. Most commands may be abbreviated - more information is available in the specific command's help documentation.")
            .WithExamples(ALL_COMMANDS)
            .Build());


            lookup.AddHelp(CommandActionKind.Clear, new HelpItemBuilder()
            .Header("Clear")
            .Description($"Clears the console.")
            .Build());


            lookup.AddHelp(CommandActionKind.Exit, new HelpItemBuilder()
            .Header("Exit")
            .Description($"Exits the interactive terminal.")
            .Build());


            lookup.AddHelp(CommandActionKind.Version, new HelpItemBuilder()
            .Header("Version")
            .Description($"Prints the current application version.")
            .Build());
            #endregion

            
            #region - Accounts -
            lookup.AddHelp(CommandActionKind.AddAccount, new HelpItemBuilder()
            .Header("Add Account")
            .Description($"Creates a new account.")
            .WithUsageTokens(new HelpUsageToken("new account", HelpUsageTokenKind.Command, false),
                            new HelpUsageToken("account-name", HelpUsageTokenKind.Argument, false),
                            USAGE_OPT_CATEGORY,
                            USAGE_OPT_DESCRIPTION,
                            USAGE_OPT_FUNDS,
                            USAGE_OPT_PRIORITY,
                            USAGE_OPT_ACCOUNT_TYPE)
            .WithOption(OPT_CATEGORY)
            .WithOption(OPT_DESCRIPTION)
            .WithOption(OPT_FUNDS)
            .WithOption(OPT_PRIORITY)
            .WithOption(OPT_ACCOUNT_TYPE)
            .Build());
            
            lookup.AddHelp(CommandActionKind.ListAccount, new HelpItemBuilder()
            .Header("List Accounts")
            .Description($"Lists accounts with optional filtering criteria.")
            .WithUsageTokens(new HelpUsageToken("list accounts", HelpUsageTokenKind.Command, false),
                            USAGE_OPT_ID,
                            USAGE_OPT_CATEGORY,
                            USAGE_OPT_DESCRIPTION,
                            USAGE_OPT_FUNDS_EXPR,
                            USAGE_OPT_NAME,
                            USAGE_OPT_PRIORITY_EXPR,
                            USAGE_OPT_ACCOUNT_TYPE)
            .Build());
            
            lookup.AddHelp(CommandActionKind.RemoveAccount, new HelpItemBuilder()
            .Header("Remove Account")
            .Description($"Removes an account. Note that accounts are not permanently removed. Instead, the account will be \"closed\" and can be re-opened later.")
            .WithUsageTokens(new HelpUsageToken("remove account", HelpUsageTokenKind.Command, false),
                            new HelpUsageToken("account-name", HelpUsageTokenKind.Argument, false),
                            USAGE_OPT_RECURSIVE)
            .Build());
            
            lookup.AddHelp(CommandActionKind.DetailAccount, new HelpItemBuilder()
            .Header("Detail Account")
            .Description($"Display the details of an account.")
            .WithUsageTokens(new HelpUsageToken("detail account", HelpUsageTokenKind.Command, false),
                            new HelpUsageToken("account-name", HelpUsageTokenKind.Argument, false),
                            USAGE_OPT_DATE)
            .Build());
            
            lookup.AddHelp(CommandActionKind.UpdateAccount, new HelpItemBuilder()
            .Header("Set Account Property")
            .Description($"Update a property of an account.")
            .WithUsageTokens(new HelpUsageToken("set account", HelpUsageTokenKind.Command, false),
                            new HelpUsageToken("account-name", HelpUsageTokenKind.Argument, false),
                            USAGE_OPT_CATEGORY,
                            USAGE_OPT_DESCRIPTION,
                            USAGE_OPT_NAME,
                            USAGE_OPT_PRIORITY,
                            USAGE_OPT_ACCOUNT_TYPE)
            .Build());
            #endregion

            
            #region - Transactions -
            lookup.AddHelp(CommandActionKind.AddTransaction, new HelpItemBuilder()
            .Header("Add Transaction")
            .Description($"Creates a new transaction.")
            .WithUsageTokens(new HelpUsageToken("new transaction", HelpUsageTokenKind.Command, false),
                            new HelpUsageToken("funds", HelpUsageTokenKind.Argument, false),
                            USAGE_OPT_SOURCE,
                            USAGE_OPT_DEST)
            .Build());
            
            lookup.AddHelp(CommandActionKind.ListTransaction, new HelpItemBuilder()
            .Header("List Transactions")
            .Description($"Lists transactions with optional filtering criteria.")
            .WithUsageTokens(new HelpUsageToken("list transactions", HelpUsageTokenKind.Command, false),
                            USAGE_OPT_ACCOUNT,
                            USAGE_OPT_DATE_EXPR,
                            USAGE_OPT_FUNDS_EXPR,
                            USAGE_OPT_ID,
                            USAGE_OPT_TRANSACTION_TYPE)
            .Build());
            
            lookup.AddHelp(CommandActionKind.DetailTransaction, new HelpItemBuilder()
            .Header("Detail Transaction")
            .Description($"Display the details of a transaction.")
            .WithUsageTokens(new HelpUsageToken("detail transaction", HelpUsageTokenKind.Command, false),
                            new HelpUsageToken("id", HelpUsageTokenKind.Argument, false))
            .Build());
            #endregion

            
            #region - Reporting -
            lookup.AddHelp(CommandActionKind.ListHistory, new HelpItemBuilder()
            .Header("List History for an Account")
            .Description($"Lists transactions with optional filtering criteria.")
            .WithUsageTokens(new HelpUsageToken("list history", HelpUsageTokenKind.Command, false),
                            new HelpUsageToken("accountName", HelpUsageTokenKind.Argument, false),
                            USAGE_OPT_COUNT,
                            USAGE_OPT_DATE_EXPR)
            .Build());
            #endregion

            

            return lookup;
        }

    }
}