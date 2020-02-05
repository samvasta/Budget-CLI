using System;
using System.Collections.Generic;
using System.IO;
using BudgetCli.Data.Exceptions;
using BudgetCli.Data.IO;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories.Interfaces;
using BudgetCli.Data.Util;
using BudgetCli.Util.Logging;
using Dapper;

namespace BudgetCli.Data.Repositories
{
    public class TransactionRepository : RepositoryBase<TransactionDto>, ITransactionRepository
    {
        public static string TABLE_NAME { get { return "Transaction"; } }

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

            LogError($"Transactions must have at least one of [Source Account, Destination Account] defined.");
            return false;
        }
    }
}