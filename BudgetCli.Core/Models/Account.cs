using System;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Models;
using BudgetCli.Util.Models;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Core.Models.ModelInfo;
using System.Diagnostics.CodeAnalysis;

namespace BudgetCli.Core.Models
{
    public class Account : IDataModel<AccountDto>, IEquatable<Account>
    {
        public static readonly ModelProperty PROP_ID = new ModelProperty("Id", "A unique ID used to reference the account", true, true);
        public static readonly ModelProperty PROP_NAME = new ModelProperty("Name", "A unique name used to reference the account", true, true);
        public static readonly ModelProperty PROP_PRIORITY = new ModelProperty("Priority", "A relative value that helps sort accounts. Lower values are higher priority.");
        public static readonly ModelProperty PROP_ACCOUNT_KIND = new ModelProperty("Account Type", "Describes, generally, how money flows through the account.");
        public static readonly ModelProperty PROP_DESCRIPTION = new ModelProperty("Description", "A brief description of the account.");
        public static readonly ModelProperty PROP_CATEGORY = new ModelProperty("Category", "The category account that this account belongs to.");
        public static readonly ModelProperty PROP_CURRENT_STATE = new ModelProperty("Current State", "The current state of the account. Contains info such as current funds, and time of last change.");

        public const long DEFAULT_PRIORITY = 5;
        public const AccountKind DEFAULT_ACCOUNT_KIND = AccountKind.Sink;

        protected RepositoryBag Repositories { get; }

        public string TypeName { get { return "Account"; } }
        public string DisplayName { get { return Name; } }
        
        #region - DTO Properties -
        
        public virtual long? Id { get; }

        public virtual string Name { get; set; }

        public virtual long? CategoryId { get; set; }

        public virtual long Priority { get; set; }

        public virtual AccountKind AccountKind { get; set; }
        
        public virtual string Description { get; set; }

        #endregion - DTO Properties -

        public DateTime CurrentDate { get; set; }

        private Account _category;
        public Account Category
        {
            get
            {
                if(_category == null && CategoryId.HasValue)
                {
                    AccountDto categoryDto = Repositories.AccountRepository.GetById(CategoryId.Value);
                    _category = DtoToModelTranslator.FromDto(categoryDto, CurrentDate, Repositories);
                }
                return _category;
            }
        }

        private AccountState _currentState;
        public AccountState CurrentState
        {
            get
            {
                if(_currentState == null && Id.HasValue)
                {
                    AccountStateDto dto = Repositories.AccountStateRepository.GetLatestByAccountId(Id.Value, CurrentDate);
                    _currentState = DtoToModelTranslator.FromDto(dto, Repositories);
                }
                return _currentState;
            }
        }

        /// <summary>
        /// New Account Constructor
        /// </summary>
        public Account(string name, Money initialFunds, DateTime date, RepositoryBag repositories)
        {
            if(repositories == null)
            {
                throw new ArgumentNullException(nameof(repositories));
            }
            this.Id = null;
            this.Name = name;
            this.CategoryId = null;
            this.Priority = DEFAULT_PRIORITY;
            this.AccountKind = DEFAULT_ACCOUNT_KIND;
            this.Description = String.Empty;
            this.CurrentDate = date;
            this.Repositories = repositories;
        }

        /// <summary>
        /// From DTO Constructor
        /// </summary>
        public Account(long id, string name, long? categoryId, long priority, AccountKind accountKind, string description, DateTime date, RepositoryBag repositories)
        {
            if(repositories == null)
            {
                throw new ArgumentNullException(nameof(repositories));
            }
            this.Id = id;
            this.Name = name;
            this.CategoryId = categoryId;
            this.Priority = priority;
            this.AccountKind = accountKind;
            this.Description = description;
            this.CurrentDate = date;
            this.Repositories = repositories;
        }

        public AccountDto ToDto()
        {
            return new AccountDto()
            {
                Id = this.Id,
                AccountKind = this.AccountKind,
                CategoryId = this.CategoryId,
                Description = this.Description,
                Name = this.Name,
                Priority = this.Priority
            };
        }

        public IEnumerable<ModelProperty> GetProperties()
        {
            yield return PROP_ID;
            yield return PROP_NAME;
            yield return PROP_DESCRIPTION;
            yield return PROP_ACCOUNT_KIND;
            yield return PROP_CURRENT_STATE;
            yield return PROP_CATEGORY;
            yield return PROP_PRIORITY;
        }

        public IEnumerable<ModelPropertyValue> GetPropertyValues()
        {
            yield return new ModelPropertyValue<long?>(PROP_ID, Id);
            yield return new ModelPropertyValue<string>(PROP_NAME, Name);
            yield return new ModelPropertyValue<string>(PROP_DESCRIPTION, Description);
            yield return new ModelPropertyValue<AccountKind>(PROP_ACCOUNT_KIND, AccountKind);
            yield return new ModelPropertyValue<AccountState>(PROP_CURRENT_STATE, CurrentState);
            yield return new ModelPropertyValue<Account>(PROP_CATEGORY, Category);
            yield return new ModelPropertyValue<long>(PROP_PRIORITY, Priority);
        }

        public bool Equals([AllowNull] Account other)
        {
            if(object.ReferenceEquals(other, null))
            {
                return false;
            }
            return Id.Equals(other.Id);
        }

        public static bool operator ==(Account left, Account right)
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

        public static bool operator !=(Account left, Account right)
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

        public override bool Equals([AllowNull] object obj)
        {
            if(obj == null)
            {
                return false;
            }
            if(obj is Account other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}