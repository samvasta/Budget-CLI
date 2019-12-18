using System;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Util.Attributes;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Models
{
    public class AccountState
    {
        [HelpInfo(Visible: false)]
        protected RepositoryBag Repositories { get; }
        
        #region - DTO Properties -
        
        [HelpInfo("Id", "A unique ID used to reference an account state")]
        public virtual long Id { get; set; }

        [HelpInfo(Visible: false)]
        public virtual long AccountId { get; set; }

        [HelpInfo("Funds", "How much money is in the account")]
        public virtual Money Funds { get; set; }

        [HelpInfo("Time-stamp", "The date/time that this state took effect")]
        public virtual DateTime Timestamp { get; set; }

        [HelpInfo("Is Closed", "A flag that indicates if the account is open or closed")]
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
    }
}