using System.Reflection.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories.Interfaces;
using BudgetCli.Util.Logging;
using Dapper;

namespace BudgetCli.Data.Repositories
{
    public class AccountStateRepository : RepositoryBase<AccountStateDto>, IAccountStateRepository
    {
        public const string TABLE_NAME = "AccountState";
        
        public AccountStateRepository(FileInfo dbInfo, ILog log) : base(dbInfo, log)
        {
        }

        public AccountStateRepository(string connectionString, ILog log) : base(connectionString, log)
        {
        }

        public override string GetTableName()
        {
            return TABLE_NAME;
        }
        
        public virtual AccountStateDto GetLatestByAccountId(long accountId)
        {
            AccountStateDto latestDto = null;
            Execute((con) =>
            {
                object parameters = new { AccountId = accountId };
                string command = $@"SELECT * FROM [{GetTableName()}] WHERE [AccountId] = @AccountId AND [Timestamp] = (SELECT MAX([Timestamp]) FROM [{GetTableName()}]);";
                latestDto = con.QueryFirstOrDefault<AccountStateDto>(command, parameters);
            });
            return latestDto;
        }

        public virtual List<AccountStateDto> GetAllByAccountId(long accountId)
        {
            List<AccountStateDto> items = new List<AccountStateDto>();
            Execute((con) =>
            {
                object parameter = new { TableName = GetTableName(), AccountId = accountId };
                string command = $@"SELECT * FROM [{GetTableName()}] WHERE AccountId = @AccountId;";
                items.AddRange(con.Query<AccountStateDto>(command, parameter));
            });
            return items;
        }


        public virtual bool RemoveAllByAccountId(long accountId)
        {
            int rowsAffected = 0;
            Execute((con) =>
            {
                object parameter = new { TableName = GetTableName(), AccountId = accountId };
                string command = $@"DELETE FROM [{GetTableName()}] WHERE AccountId = @AccountId;";
                rowsAffected = con.Execute(command, parameter);
            });
            return rowsAffected > 0;
        }

        public virtual bool CloseAccount(long accountId)
        {
            return SetAccountClosedState(accountId, true);
        }

        public virtual bool ReOpenAccount(long accountId)
        {
            return SetAccountClosedState(accountId, false);
        }

        public virtual bool SetAccountClosedState(long accountId, bool isClosed)
        {
            bool successful = false;
            AccountStateDto latestDto = GetLatestByAccountId(accountId);

            if(latestDto == null)
            {
                return false;
            }

            Execute((con) =>
            {

                //Check if we even need to do anything
                if(latestDto.IsClosed == isClosed)
                {
                    //Nothing changed, so technically this wasn't successful
                    successful = false;
                }
                else
                {
                    //Create new state, copying old except for the IsClosed flag
                    AccountStateDto newState = new AccountStateDto()
                    {
                        AccountId = accountId,
                        Funds = latestDto.Funds,
                        Timestamp = DateTime.Now,
                        IsClosed = isClosed
                    };
                    successful = Upsert(newState);
                }
            });

            return successful;
        }
    }
}