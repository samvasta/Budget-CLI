using System;
using System.Collections.Generic;
using System.Linq;
using BudgetCliData.Models;
using BudgetCliData.Repositories;
using BudgetCliUtil.Attributes;

namespace BudgetCliCore.Models
{

    [HelpInfo("Transaction", "Represents a transfer of funds between accounts")]
    public class Transaction : IDataModel<TransactionDto>
    {
        [HelpInfo(Visible: false)]
        protected RepositoryBag Repositories { get; }

        #region - DTO Properties -
        
        [HelpInfo("Id", "A unique ID used to reference a transaction")]
        public virtual long? Id { get; set; }


        [HelpInfo("Date", "The date this transaction was (or will be) executed")]
        public virtual DateTime Timestamp { get; }


        [HelpInfo(Visible: false)]
        public virtual long? SourceAccountId { get; }


        [HelpInfo(Visible: false)]
        public virtual long? DestinationAccountId { get; }


        [HelpInfo("Transfer Amount", "The amount being transferred")]
        public virtual long TransferAmount { get; }


        [HelpInfo("Memo", "Write a note explaining the transaction (Optional)")]
        public virtual string Memo { get; }

        #endregion - DTO Properties -
        
        private Account _sourceAccount;
        [HelpInfo("Source Account", "The account that funds are withdrawn from. Leave this empty if funds come from an external account (eg. salary)")]
        public virtual Account SourceAccount
        {
            get
            {
                if(_sourceAccount == null && SourceAccountId.HasValue)
                {
                    _sourceAccount = DtoToModelTranslator.FromDto(Repositories.AccountRepository.GetById(SourceAccountId.Value), Repositories);
                }
                return _sourceAccount;
            }
        }
        
        private Account _destAccount;
        [HelpInfo("Destination Account", "The account that funds are deposited into. Leave this empty if funds are deposited into an external account (eg. paying off a credit card)")]
        public virtual Account DestinationAccount
        {
            get
            {
                if(_destAccount == null && DestinationAccountId.HasValue)
                {
                    _destAccount = DtoToModelTranslator.FromDto(Repositories.AccountRepository.GetById(DestinationAccountId.Value), Repositories);
                }
                return _destAccount;
            }
        }

        /// <summary>
        /// New Transaction constructor
        /// </summary>
        public Transaction(DateTime timestamp, long? sourceAccountId, long? destAccountId, long transferAmount, string memo, RepositoryBag repositories)
        {
            this.Id = null;
            this.Timestamp = timestamp;
            this.SourceAccountId = sourceAccountId;
            this.DestinationAccountId = destAccountId;
            this.TransferAmount = transferAmount;
            this.Memo = memo;
        }

        /// <summary>
        /// From Dto constructor
        /// </summary>
        public Transaction(long id, DateTime timestamp, long? sourceAccountId, long? destAccountId, long transferAmount, string memo, RepositoryBag repositories)
        {
            this.Id = id;
            this.Timestamp = timestamp;
            this.SourceAccountId = sourceAccountId;
            this.DestinationAccountId = destAccountId;
            this.TransferAmount = transferAmount;
            this.Memo = memo;
        }

        public TransactionDto ToDto()
        {
            TransactionDto dto = new TransactionDto()
            {
                Id = this.Id,
                Timestamp = this.Timestamp,
                SourceAccountId = this.SourceAccountId,
                DestinationAccountId = this.DestinationAccountId,
                TransferAmount = this.TransferAmount,
                Memo = this.Memo
            };

            return dto;
        }
    }
}