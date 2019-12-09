using System;
using System.Collections.Generic;
using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Repositories;

namespace BudgetCli.Core.Models.Commands.Accounts
{
    public class AddAccountCommand : CommandActionBase
    {
        public override string ActionText 
        {
            get
            {
                //TODO
                return "";
            }
        }
        public override CommandActionKind CommandActionKind 
        {
            get { return CommandActionKind.AddAccount; }
        }

        //Options
        public IntegerCommandOption CategoryIdOption { get; set; }
        public StringCommandOption DescriptionOption { get; set; }
        public DecimalCommandOption FundsOption { get; set; }
        public IntegerCommandOption PriorityOption { get; set; }
        public EnumCommandOption<AccountKind> AccountTypeOption { get; set; }

        public AddAccountCommand(string rawText, RepositoryBag repositories) : base(rawText, repositories)
        {
        }

        public AddAccountCommand(long id, string rawText, bool isExecuted, DateTime timestamp, RepositoryBag repositories) : base(id, rawText, isExecuted, timestamp, repositories)
        {
        }

        protected override IEnumerable<CommandOptionBase> GetOptions()
        {
            yield return CategoryIdOption;
            yield return DescriptionOption;
            yield return FundsOption;
            yield return PriorityOption;
            yield return AccountTypeOption;
        }

        protected override bool TryDoAction()
        {
            throw new NotImplementedException();
        }

        protected override bool TryUndoAction()
        {
            throw new NotImplementedException();
        }
    }
}