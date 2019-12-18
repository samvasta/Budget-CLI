using System;
using System.Collections.Generic;
using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Repositories;
using BudgetCli.Util.Logging;

namespace BudgetCli.Core.Models.Commands.Accounts
{
    public class ListAccountCommand : CommandActionBase
    {
        public override string ActionText
        {
            get
            {
                return "";
            }
        }

        public override CommandActionKind CommandActionKind { get { return CommandActionKind.NonPersisted; } }

        //Options
        public IntegerCommandOption CategoryIdOption { get; set; }
        public StringCommandOption DescriptionOption { get; set; }
        public StringCommandOption NameOption { get; set; }
        public IntegerRangeCommandOption PriorityOption { get; set; }
        public EnumCommandOption<AccountKind> AccountTypeOption { get; set; }

        public ListAccountCommand(string rawText, RepositoryBag repositories) : base(rawText, repositories)
        {
            CategoryIdOption = new IntegerCommandOption(CommandOptionKind.Category);
            DescriptionOption = new StringCommandOption(CommandOptionKind.Description);
            NameOption = new StringCommandOption(CommandOptionKind.Name);
            PriorityOption = new IntegerRangeCommandOption(CommandOptionKind.Priority);
            AccountTypeOption = new EnumCommandOption<AccountKind>(CommandOptionKind.AccountType);
        }

        protected override bool TryDoAction(ILog log, IEnumerable<ICommandActionListener> listeners = null)
        {
            IEnumerable<Account> accounts = GetAccounts();

            foreach(var account in accounts)
            {
                //TODO: Log account
            }

            return true;
        }

        private IEnumerable<Account> GetAccounts()
        {
            //TODO
            return null;
        }
    }
}