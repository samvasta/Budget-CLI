using System;
using System.Collections.Generic;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Core.Models.ModelInfo;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Models
{
    public class AccountState : IDataModel<AccountStateDto>
    {
        public static readonly ModelProperty PROP_ID = new ModelProperty("Id", "A unique ID used to reference an account state");
        public static readonly ModelProperty PROP_FUNDS = new ModelProperty("Funds", "How much money is in the account");
        public static readonly ModelProperty PROP_TIMESTAMP = new ModelProperty("Date", "The date/time that this state took effect");
        public static readonly ModelProperty PROP_STATUS = new ModelProperty("Is Closed", "A flag that indicates if the account is open or closed");
        public static readonly ModelProperty PROP_ACCOUNT = new ModelProperty("Account", "The account that this state describes");
        
        protected RepositoryBag Repositories { get; }

        public string TypeName { get { return "Account State"; } }
        public string DisplayName { get { return $"{Account.DisplayName} - {Timestamp}"; } }
        
        #region - DTO Properties -
        
        public virtual long? Id { get; set; }
        public virtual long AccountId { get; set; }
        public virtual Money Funds { get; set; }
        public virtual DateTime Timestamp { get; set; }
        public virtual bool IsClosed { get; set; }

        #endregion - DTO Properties -

        private Account _account;
        public Account Account
        {
            get
            {
                if(_account == null)
                {
                    AccountDto dto = Repositories.AccountRepository.GetById(AccountId);
                    _account = DtoToModelTranslator.FromDto(dto, Repositories);
                }
                return _account;
            }
        }

        public AccountState(long id, long accountId, Money funds, DateTime timestamp, bool isClosed)
        {
            Id = id;
            AccountId = accountId;
            Funds = funds;
            Timestamp = timestamp;
            IsClosed = isClosed;
        }

        public AccountStateDto ToDto()
        {
            return new AccountStateDto()
            {
                Id = this.Id.Value,
                AccountId = this.AccountId,
                Funds = this.Funds.InternalValue,
                IsClosed = this.IsClosed,
                Timestamp = this.Timestamp
            };
        }

        public IEnumerable<ModelProperty> GetProperties()
        {
            yield return PROP_ID;
            yield return PROP_ACCOUNT;
            yield return PROP_TIMESTAMP;
            yield return PROP_STATUS;
            yield return PROP_FUNDS;
        }

        public IEnumerable<ModelPropertyValue> GetPropertyValues()
        {
            yield return new ModelPropertyValue<long?>(PROP_ID, Id);
            yield return new ModelPropertyValue<Account>(PROP_ACCOUNT, Account);
            yield return new ModelPropertyValue<DateTime>(PROP_TIMESTAMP, Timestamp);
            yield return new ModelPropertyValue<bool>(PROP_STATUS, IsClosed);
            yield return new ModelPropertyValue<Money>(PROP_FUNDS, Funds);
        }
    }
}