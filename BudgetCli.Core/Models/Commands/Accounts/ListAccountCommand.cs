using System;
using System.Collections.Generic;
using BudgetCli.Core.Models.CommandResults;
using BudgetCli.Core.Models.CommandResults.InfoModels;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Repositories;
using BudgetCli.Util.Logging;

namespace BudgetCli.Core.Models.Commands.Accounts
{
    public class ListAccountCommand : CommandActionBase
    {
        private const string EMPTY_OPTION = "<EMPTY>";


        public override string ActionText
        {
            get
            {
                return "Listed accounts";
            }
        }

        public override CommandActionKind CommandActionKind { get { return CommandActionKind.ListAccount; } }

        //Options
        public IntegerCommandOption IdOption { get; set; }
        public IntegerCommandOption CategoryIdOption { get; set; }
        public StringCommandOption DescriptionOption { get; set; }
        public StringCommandOption NameOption { get; set; }
        public IntegerRangeCommandOption PriorityOption { get; set; }
        public MoneyRangeCommandOption FundsOption { get; set; }
        public EnumCommandOption<AccountKind> AccountTypeOption { get; set; }

        public ListAccountCommand(string rawText, RepositoryBag repositories) : base(rawText, repositories)
        {
            IdOption = new IntegerCommandOption(CommandOptionKind.Id);
            CategoryIdOption = new IntegerCommandOption(CommandOptionKind.Category);
            DescriptionOption = new StringCommandOption(CommandOptionKind.Description);
            NameOption = new StringCommandOption(CommandOptionKind.Name);
            PriorityOption = new IntegerRangeCommandOption(CommandOptionKind.Priority);
            FundsOption = new MoneyRangeCommandOption(CommandOptionKind.FundsExpr);
            AccountTypeOption = new EnumCommandOption<AccountKind>(CommandOptionKind.AccountType);
        }

        protected override bool TryDoAction(ILog log, IEnumerable<ICommandActionListener> listeners = null)
        {
            IEnumerable<Account> accounts = GetAccounts();

            FilterCriteria criteria = GetFilterCriteria();

            ReadCommandResult<Account> result = new ReadCommandResult<Account>(this, true, accounts, criteria);
            
            TransmitResult(result, listeners);
            return true;
        }

        private List<Account> GetAccounts()
        {
            var dtos = Repositories.AccountRepository.GetAccounts((long?)IdOption.GetValue(null), NameOption.GetValue(null), (long?)CategoryIdOption.GetValue(null), DescriptionOption.GetValue(null), PriorityOption.GetValue(null), (AccountKind?)AccountTypeOption.GetValue(null), FundsOption.GetValue(null), false);
            
            List<Account> accounts = new List<Account>();
            
            foreach(var dto in dtos)
            {
                accounts.Add(DtoToModelTranslator.FromDto(dto, Repositories));
            }
            return accounts;
        }

        public FilterCriteria GetFilterCriteria()
        {
            FilterCriteria criteria = new FilterCriteria();

            //Id
            if(IdOption.IsDataValid)
            {
                criteria.AddField(Account.PROP_ID.DisplayName, $"= {IdOption.GetValue(EMPTY_OPTION)}");
            }

            //Category
            if(CategoryIdOption.IsDataValid)
            {
                Account category = DtoToModelTranslator.FromDto(Repositories.AccountRepository.GetById(CategoryIdOption.GetValue(-1)), Repositories);

                criteria.AddField(Account.PROP_CATEGORY.DisplayName, $"= {category.Name}");
            }

            //Description
            if(DescriptionOption.IsDataValid)
            {
                criteria.AddField(Account.PROP_DESCRIPTION.DisplayName, $"contains \"{DescriptionOption.GetValue(EMPTY_OPTION)}\"");
            }

            //Name
            if(NameOption.IsDataValid)
            {
                criteria.AddField(Account.PROP_NAME.DisplayName, $"contains \"{NameOption.GetValue(EMPTY_OPTION)}\"");
            }

            //Priority
            if(PriorityOption.IsDataValid)
            {
                criteria.AddField(Account.PROP_PRIORITY.DisplayName, $"= {PriorityOption.GetValue(EMPTY_OPTION)}");
            }

            //Funds
            if(FundsOption.IsDataValid)
            {
                criteria.AddField(AccountState.PROP_FUNDS.DisplayName, $"= {FundsOption.GetValue(EMPTY_OPTION)}");
            }

            //Account Type
            if(AccountTypeOption.IsDataValid)
            {
                criteria.AddField(Account.PROP_ACCOUNT_KIND.DisplayName, $"= {AccountTypeOption.GetValue(EMPTY_OPTION)}");
            }

            return criteria;
        }

        private void TransmitResult(ReadCommandResult<Account> result, IEnumerable<ICommandActionListener> listeners = null)
        {
            if(listeners != null && result != null)
            {
                foreach(var listener in listeners)
                {
                    listener.OnCommand(result);
                }
            }
        }
    }
}