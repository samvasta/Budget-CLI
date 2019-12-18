using System;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using BudgetCli.Data.Enums;
using BudgetCli.Util.Attributes;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Models;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Models
{
    public class Account
    {
        public const long DEFAULT_PRIORITY = 5;
        public const AccountKind DEFAULT_ACCOUNT_KIND = AccountKind.Sink;

        [HelpInfo(Visible: false)]
        protected RepositoryBag Repositories { get; }
        
        #region - DTO Properties -
        
        [HelpInfo("Id", "A unique ID used to reference an account")]
        public virtual long? Id { get; set; }

        [HelpInfo("Name", "A unique name used to reference the account")]
        public virtual string Name { get; set; }

        [HelpInfo(Visible: false)]
        public virtual long? CategoryId { get; set; }

        [HelpInfo("Priority", "A relative value that helps sort accounts.")]
        public virtual long Priority { get; set; }

        [HelpInfo("Account Type", "Describes, generally, how money flows through the account.")]
        public virtual AccountKind AccountKind { get; set; }
        
        [HelpInfo("Description", "A brief description of the account.")]
        public virtual string Description { get; set; }

        #endregion - DTO Properties -

        private Account _category;
        [HelpInfo("Category", "The category account that this account belongs to")]
        public Account Category
        {
            get
            {
                if(_category == null && CategoryId.HasValue)
                {
                    AccountDto categoryDto = Repositories.AccountRepository.GetById(CategoryId.Value);
                    _category = DtoToModelTranslator.FromDto(categoryDto, Repositories);
                }
                return _category;
            }
        }

        private AccountState _currentState;
        [HelpInfo("Current State", "The current state of the account. Contains info such as current funds, and time of last change.")]
        public AccountState CurrentState
        {
            get
            {
                if(_currentState == null && Id.HasValue)
                {
                    AccountStateDto dto = Repositories.AccountStateRepository.GetLatestByAccountId(Id.Value);
                    _currentState = DtoToModelTranslator.FromDto(dto, Repositories);
                }
                return _currentState;
            }
        }

        /// <summary>
        /// New Account Constructor
        /// </summary>
        public Account(string name, Money initialFunds, RepositoryBag repositories)
        {
            this.Id = null;
            this.Name = name;
            this.CategoryId = null;
            this.Priority = DEFAULT_PRIORITY;
            this.AccountKind = DEFAULT_ACCOUNT_KIND;
            this.Description = String.Empty;
            this.Repositories = repositories;
        }

        /// <summary>
        /// From DTO Constructor
        /// </summary>
        public Account(long id, string name, long? categoryId, long priority, AccountKind accountKind, string description, RepositoryBag repositories)
        {
            this.Id = id;
            this.Name = name;
            this.CategoryId = categoryId;
            this.Priority = priority;
            this.AccountKind = accountKind;
            this.Description = description;
            this.Repositories = repositories;
        }
    }
}