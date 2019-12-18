using System;
using System.Collections.Generic;
using System.IO;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories.Interfaces;
using BudgetCli.Util.Logging;
using Dapper;

namespace BudgetCli.Data.Repositories
{
    public class AccountRepository : RepositoryBase<AccountDto>, IAccountRepository
    {
        public const string TABLE_NAME = "Account";

        public AccountRepository(FileInfo dbInfo, ILog log) : base(dbInfo, log)
        {
        }

        public AccountRepository(string connectionString, ILog log) : base(connectionString, log)
        {
        }

        public IEnumerable<long> GetChildAccountIds(long categoryId)
        {
            List<long> ids = new List<long>();
            Execute((con) =>
            {
                string command = $@"SELECT Id FROM [{GetTableName()}] WHERE [CategoryId] = @CategoryId;";
                object parameter = new { CategoryId = categoryId };
                ids.AddRange(con.Query<long>(command, parameter));
            });
#if DEBUG
            if(ids.Count == 0)
            {
                LogError($"Retrieval of rows by CategoryId failed because row does not exist! (CategoryId = {categoryId})");
            }
#endif
            return ids;
        }

        public long GetIdByName(string name)
        {
            long id = -1;
            Execute((con) =>
            {
                string command = $@"SELECT COALESCE(MIN(Id), -1) FROM [{GetTableName()}] WHERE [Name] = @Name;";
                object parameter = new { Name = name };
                id = con.QueryFirstOrDefault<long>(command, parameter);
            });
#if DEBUG
            if(id == -1)
            {
                LogError($"Retrieval of row by Name failed because row does not exist! (Name = {name})");
            }
#endif
            return id;
        }

        public override string GetTableName()
        {
            return TABLE_NAME;
        }
    }
}