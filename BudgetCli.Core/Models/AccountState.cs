using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Core.Models.ModelInfo;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Models
{
    public class AccountState : IDataModel<AccountStateDto>, IEquatable<AccountState>
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
        
        public virtual long? Id { get; }
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
                    _account = DtoToModelTranslator.FromDto(dto, Timestamp, Repositories);
                }
                return _account;
            }
        }

        public AccountState(long id, long accountId, Money funds, DateTime timestamp, bool isClosed, RepositoryBag repositories)
        {
            if(repositories == null)
            {
                throw new ArgumentNullException(nameof(repositories));
            }
            Id = id;
            AccountId = accountId;
            Funds = funds;
            Timestamp = timestamp;
            IsClosed = isClosed;
            Repositories = repositories;
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

        public bool Equals([AllowNull] AccountState other)
        {
            if(object.ReferenceEquals(other, null))
            {
                return false;
            }
            return Id.Equals(other.Id);
        }

        public override bool Equals([AllowNull] object obj)
        {
            if(object.ReferenceEquals(obj, null))
            {
                return false;
            }
            if(obj is AccountState state)
            {
                return this.Equals(state);
            }
            return false;
        }

        public static bool operator ==(AccountState left, AccountState right)
        {
            if(object.ReferenceEquals(left, null))
            {
                //equal if both null
                return object.ReferenceEquals(right, null);
            }
            if(object.ReferenceEquals(right, null))
            {
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(AccountState left, AccountState right)
        {
            if(object.ReferenceEquals(left, null))
            {
                //not equal if not both null
                return !object.ReferenceEquals(right, null);
            }
            if(object.ReferenceEquals(right, null))
            {
                return true;
            }
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}