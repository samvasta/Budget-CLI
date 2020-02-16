using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Core.Models.ModelInfo;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Models
{
    public class Transaction : IDataModel<TransactionDto>, IEquatable<Transaction>
    {
        public static readonly ModelProperty PROP_ID = new ModelProperty("Id", "A unique ID used to reference a transaction.");
        public static readonly ModelProperty PROP_TIMESTAMP = new ModelProperty("Date", "The date this transaction was (or will be) executed");
        public static readonly ModelProperty PROP_SOURCE = new ModelProperty("Source Account", "The account that funds are withdrawn from. Leave this empty if funds come from an external account (eg. salary)");
        public static readonly ModelProperty PROP_DEST = new ModelProperty("Destination Account", "The account that funds are deposited into. Leave this empty if funds are deposited into an external account (eg. paying off a credit card)");
        public static readonly ModelProperty PROP_AMOUNT = new ModelProperty("Transfer Amount", "The amount being transferred");
        public static readonly ModelProperty PROP_MEMO = new ModelProperty("Memo", "Write a note explaining the transaction (Optional)");

        protected RepositoryBag Repositories { get; }

        public string TypeName { get { return "Transaction"; } }
        public string DisplayName { get { return Id?.ToString(); } }

        #region - DTO Properties -
        
        public virtual long? Id { get; }
        public virtual DateTime Timestamp { get; }
        public virtual long? SourceAccountId { get; }
        public virtual long? DestinationAccountId { get; }
        public virtual Money TransferAmount { get; }
        public virtual string Memo { get; }

        #endregion - DTO Properties -
        
        private Account _sourceAccount;
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
            this.TransferAmount = new Money(transferAmount, true);
            this.Memo = memo;
            Repositories = repositories;
        }

        /// <summary>
        /// From Dto constructor
        /// </summary>
        public Transaction(long id, DateTime timestamp, long? sourceAccountId, long? destAccountId, Money transferAmount, string memo, RepositoryBag repositories)
        {
            this.Id = id;
            this.Timestamp = timestamp;
            this.SourceAccountId = sourceAccountId;
            this.DestinationAccountId = destAccountId;
            this.TransferAmount = transferAmount;
            this.Memo = memo;
            Repositories = repositories;
        }

        public TransactionDto ToDto()
        {
            TransactionDto dto = new TransactionDto()
            {
                Id = this.Id,
                Timestamp = this.Timestamp,
                SourceAccountId = this.SourceAccountId,
                DestinationAccountId = this.DestinationAccountId,
                TransferAmount = this.TransferAmount.InternalValue,
                Memo = this.Memo
            };

            return dto;
        }

        public IEnumerable<ModelProperty> GetProperties()
        {
            yield return PROP_ID;
            yield return PROP_TIMESTAMP;
            yield return PROP_SOURCE;
            yield return PROP_DEST;
            yield return PROP_AMOUNT;
            yield return PROP_MEMO;
        }

        public IEnumerable<ModelPropertyValue> GetPropertyValues()
        {
            yield return new ModelPropertyValue<long?>(PROP_ID, Id);
            yield return new ModelPropertyValue<DateTime>(PROP_TIMESTAMP, Timestamp);
            yield return new ModelPropertyValue<Account>(PROP_SOURCE, SourceAccount);
            yield return new ModelPropertyValue<Account>(PROP_DEST, DestinationAccount);
            yield return new ModelPropertyValue<Money>(PROP_AMOUNT, TransferAmount);
            yield return new ModelPropertyValue<string>(PROP_MEMO, Memo);
        }

        public bool Equals([AllowNull] Transaction other)
        {
            if(object.ReferenceEquals(other, null))
            {
                return false;
            }
            return Id.Equals(other.Id);
        }

        public static bool operator ==(Transaction left, Transaction right)
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

        public static bool operator !=(Transaction left, Transaction right)
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
            if(obj is Transaction other)
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