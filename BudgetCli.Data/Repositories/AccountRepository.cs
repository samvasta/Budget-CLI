using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories.Interfaces;
using BudgetCli.Util.Logging;
using BudgetCli.Util.Models;
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

        public bool DoesNameExist(string name)
        {
            long count = 0;
            Execute((con) =>
            {
                string command = $@"SELECT COUNT(*) FROM [{GetTableName()}] WHERE [Name] = @Name;";
                object parameter = new { Name = name };
                count = con.QueryFirst<long>(command, parameter);
            });

            return count > 0;
        }

        public IEnumerable<AccountDto> GetAccounts(long? id = null, string nameContains = null, long? categoryId = null, string descriptionContains = null, Range<long> priorityRange = null, AccountKind? accountKind = null, Range<Money> fundsRange = null, bool includeClosedAccounts = false)
        {
            StringBuilder commandBuilder = new StringBuilder();
            commandBuilder.Append($"SELECT * FROM [{GetTableName()}] AS A JOIN (SELECT [AccountId], [Funds], MAX([Timestamp]), [IsClosed] FROM [{AccountStateRepository.TABLE_NAME}] GROUP BY [AccountId]) AS S ON A.Id = S.AccountId");

            bool isFirstCondition = true;

            if(id.HasValue)
            {
                AddSelectConditionPrefix(commandBuilder, ref isFirstCondition);
                commandBuilder.Append(" A.[Id] = @Id");
            }
            if(!String.IsNullOrEmpty(nameContains))
            {
                AddSelectConditionPrefix(commandBuilder, ref isFirstCondition);
                commandBuilder.Append(" A.[Name] LIKE @Name");
            }
            if(!String.IsNullOrEmpty(descriptionContains))
            {
                AddSelectConditionPrefix(commandBuilder, ref isFirstCondition);
                commandBuilder.Append(" A.[Description] LIKE @Description");
            }
            if(categoryId.HasValue)
            {
                AddSelectConditionPrefix(commandBuilder, ref isFirstCondition);
                commandBuilder.Append(" A.[CategoryId] = @CategoryId");
            }
            if(accountKind.HasValue)
            {
                AddSelectConditionPrefix(commandBuilder, ref isFirstCondition);
                commandBuilder.Append(" A.[AccountKind] = @AccountKind");
            }
            if(priorityRange != null)
            {
                AddSelectConditionPrefix(commandBuilder, ref isFirstCondition);
                if(priorityRange.IsFromInclusive)
                {
                    commandBuilder.Append(" A.[Priority] >= @PriorityMin");
                }
                else
                {
                    commandBuilder.Append(" A.[Priority] > @PriorityMin");
                }
                if(priorityRange.IsToInclusive)
                {
                    commandBuilder.Append(" AND A.[Priority] <= @PriorityMax");
                }
                else
                {
                    commandBuilder.Append(" AND A.[Priority] < @PriorityMax");
                }
            }
            if(fundsRange != null)
            {
                AddSelectConditionPrefix(commandBuilder, ref isFirstCondition);
                if(fundsRange.IsFromInclusive)
                {
                    commandBuilder.Append(" S.[Funds] >= @FundsMin");
                }
                else
                {
                    commandBuilder.Append(" S.[Funds] > @FundsMin");
                }
                if(fundsRange.IsToInclusive)
                {
                    commandBuilder.Append(" AND S.[Funds] <= @FundsMax");
                }
                else
                {
                    commandBuilder.Append(" AND S.[Funds] < @FundsMax");
                }
            }
            if(!includeClosedAccounts)
            {
                AddSelectConditionPrefix(commandBuilder, ref isFirstCondition);
                commandBuilder.Append(" S.IsClosed = 0");
            }


            commandBuilder.Append(";");

            object parameters = new
            {
                Id = id,
                Name = $"%{nameContains}%",
                CategoryId = categoryId,
                Description = $"%{descriptionContains}%",
                PriorityMin = priorityRange?.From,
                PriorityMax = priorityRange?.To,
                AccountKind = accountKind,
                FundsMin = fundsRange?.From.InternalValue,
                FundsMax = fundsRange?.To.InternalValue
            };
            List<AccountDto> list = new List<AccountDto>();
            Execute((con) =>
            {
                string command = commandBuilder.ToString();
                list.AddRange(con.Query<AccountDto>(command, parameters));
            });
            return list;
        }

        private void AddSelectConditionPrefix(StringBuilder commandBuilder, ref bool isFirstCondition)
        {
            if(isFirstCondition)
            {
                commandBuilder.Append(" WHERE");
            }
            else
            {
                commandBuilder.Append(" AND ");
            }
            isFirstCondition = false;
        }

        public override string GetTableName()
        {
            return TABLE_NAME;
        }
    }
}