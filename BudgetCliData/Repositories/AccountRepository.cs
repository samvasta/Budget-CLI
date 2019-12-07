using System;
using System.IO;
using BudgetCliData.Models;
using BudgetCliUtil.Logging;
using Dapper;

namespace BudgetCliData.Repositories
{
    public class AccountRepository : RepositoryBase<AccountDto>
    {
        public const string TABLE_NAME = "Account";

        public AccountRepository(FileInfo dbInfo, ILog log) : base(dbInfo, log)
        {
        }

        public AccountRepository(string connectionString, ILog log) : base(connectionString, log)
        {
        }

        public override string GetTableName()
        {
            return TABLE_NAME;
        }
    }
}