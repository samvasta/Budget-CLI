using System;
using System.IO;
using BudgetCli.Data.Models;
using BudgetCli.Util.Logging;
using Dapper;

namespace BudgetCli.Data.Repositories
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