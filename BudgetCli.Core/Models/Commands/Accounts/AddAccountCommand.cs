using System.IO;
using System;
using System.Collections.Generic;
using BudgetCli.Core.Models.CommandResults;
using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Util.Logging;
using BudgetCli.Util.Models;
using BudgetCli.Core.Models.Interfaces;

namespace BudgetCli.Core.Models.Commands.Accounts
{
    public class AddAccountCommand : CommandActionBase
    {
        public override string ActionText 
        {
            get
            {
                return $"Added account \"{AccountName.GetValue(String.Empty)}\" with initial funds {FundsOption.GetValue(Money.None)}";
            }
        }

        public override CommandActionKind CommandActionKind 
        {
            get { return CommandActionKind.AddAccount; }
        }

        //Options
        public IntegerCommandOption AccountId { get; set; }
        public StringCommandOption AccountName { get; set; }
        public IntegerCommandOption CategoryIdOption { get; set; }
        public StringCommandOption DescriptionOption { get; set; }
        public MoneyCommandOption FundsOption { get; set; }
        public IntegerCommandOption PriorityOption { get; set; }
        public EnumCommandOption<AccountKind> AccountTypeOption { get; set; }

        public AddAccountCommand(string rawText, RepositoryBag repositories, string accountName) : base(rawText, repositories)
        {
            AccountId = new IntegerCommandOption(CommandOptionKind.Account);
            AccountName = new StringCommandOption(CommandOptionKind.Name);
            CategoryIdOption = new IntegerCommandOption(CommandOptionKind.Category);
            DescriptionOption = new StringCommandOption(CommandOptionKind.Description);
            FundsOption = new MoneyCommandOption(CommandOptionKind.Funds);
            PriorityOption = new IntegerCommandOption(CommandOptionKind.Priority);
            AccountTypeOption = new EnumCommandOption<AccountKind>(CommandOptionKind.AccountType);
            AccountName.SetData(accountName);
        }

        protected override bool TryDoAction(ILog log, IEnumerable<ICommandActionListener> listeners = null)
        {
            CreateCommandResult<Account> result = new CreateCommandResult<Account>(this, false, null);

            if(Repositories.AccountRepository.DoesNameExist(AccountName.GetValue(String.Empty)))
            {
                TransmitResult(result, listeners);
                return false;
            }

            AccountDto accountDto = BuildAccountDto();
            
            bool successful = Repositories.AccountRepository.Upsert(accountDto);
            AccountId.SetData(accountDto.Id.Value);

            if(!accountDto.Id.HasValue)
            {
                log?.WriteLine("Error occurred while adding account. Account was not assigned a valid Id.", LogLevel.Error);
                TransmitResult(result, listeners);
                return false;
            }

            AccountStateDto accountStateDto = BuildAccountStateDto(accountDto.Id.Value);
            successful &= Repositories.AccountStateRepository.Upsert(accountStateDto);

            if(successful)
            {
                log?.WriteLine($"Added account \"{accountDto.Name}\"", LogLevel.Normal);
                
                result = new CreateCommandResult<Account>(this, successful, DtoToModelTranslator.FromDto(accountDto, DateTime.Today, Repositories));
            }

            TransmitResult(result, listeners);
            return successful;
        }

        private AccountDto BuildAccountDto()
        {
            AccountDto dto = new AccountDto()
            {
                AccountKind = AccountTypeOption.GetValue(Account.DEFAULT_ACCOUNT_KIND),
                CategoryId = (long?)CategoryIdOption.GetValue((long?)null),
                Description = DescriptionOption.GetValue(String.Empty),
                Name = AccountName.GetValue(String.Empty),
                Priority = PriorityOption.GetValue(Account.DEFAULT_PRIORITY)
            };

            if(AccountId.IsDataValid)
            {
                dto.Id = AccountId.GetValue(-1);
            }
            else
            {
                dto.Id = null;
            }

            return dto;
        }

        private AccountStateDto BuildAccountStateDto(long accountId)
        {
            AccountStateDto dto = new AccountStateDto()
            {
                AccountId = accountId,
                Funds = FundsOption.GetValue(0).InternalValue,
                IsClosed = false,
                Timestamp = DateTime.Now
            };

            return dto;
        }

        private void TransmitResult(CreateCommandResult<Account> result, IEnumerable<ICommandActionListener> listeners = null)
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