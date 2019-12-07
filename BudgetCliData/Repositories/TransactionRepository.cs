using System;
using System.Collections.Generic;
using System.IO;
using BudgetCliData.Exceptions;
using BudgetCliData.IO;
using BudgetCliData.Models;
using BudgetCliData.Repositories.Interfaces;
using BudgetCliData.Util;
using BudgetCliUtil.Attributes;
using BudgetCliUtil.Logging;
using Dapper;

namespace BudgetCliData.Repositories
{
    public class TransactionRepository : RepositoryBase<TransactionDto>, ITransactionRepository
    {
        public const string TABLE_NAME = "Transaction";

        public override string GetTableName()
        {
            return TABLE_NAME;
        }

        public TransactionRepository(FileInfo dbFile, ILog log) : base(dbFile, log)
        {

        }
        
        public TransactionRepository(string connectionString, ILog log) : base(connectionString, log)
        {

        }

        public override bool Upsert(TransactionDto data)
        {
            if(data.DestinationAccountId.HasValue && data.SourceAccountId.HasValue)
            {
                return base.Upsert(data);
            }

            LogError($"Transactions must have at least one of [{data.GetPropertyName(x => x.SourceAccountId)}, {data.GetPropertyName(x => x.DestinationAccountId)}] defined.");
            return false;
        }
    }
}