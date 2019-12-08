using System;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using BudgetCli.Data.Enums;
using BudgetCli.Util.Attributes;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Models;

namespace BudgetCli.Core.Models
{
    public class Account
    {
        [HelpInfo(Visible: false)]
        protected RepositoryBag Repositories { get; }
        
        #region - DTO Properties -
        
        [HelpInfo("Id", "A unique ID used to reference an account")]
        public virtual long? Id { get; set; }

        [HelpInfo("Name", "A unique name used to reference the account")]
        public virtual string Name { get; set; }

        [HelpInfo(Visible: false)]
        public virtual long? CategoryId { get; set; }

        [HelpInfo("Initial Funds", "Funds held when the account was first created")]
        public virtual long InitialFunds { get; set; }

        [HelpInfo("Priority", "A relative value that helps sort accounts.")]
        public virtual long Priority { get; set; }

        [HelpInfo("Account Type", "Describes, generally, how money flows through the account.")]
        public virtual AccountKind AccountKind { get; set; }
        
        [HelpInfo("Description", "A brief description of the account")]
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

        /// <summary>
        /// New Account Constructor
        /// </summary>
        public Account(string name, long initialFunds, RepositoryBag repositories)
        {
            this.Id = null;
            this.Name = name;
            this.CategoryId = null;
            this.InitialFunds = initialFunds;
            this.Priority = 5;
            this.AccountKind = AccountKind.Sink;
            this.Description = String.Empty;
            this.Repositories = repositories;
        }

        /// <summary>
        /// From DTO Constructor
        /// </summary>
        public Account(long id, string name, long? categoryId, long initialFunds, long priority, AccountKind accountKind, string description, RepositoryBag repositories)
        {
            this.Id = id;
            this.Name = name;
            this.CategoryId = categoryId;
            this.InitialFunds = initialFunds;
            this.Priority = priority;
            this.AccountKind = accountKind;
            this.Description = description;
            this.Repositories = repositories;
        }
    }
}