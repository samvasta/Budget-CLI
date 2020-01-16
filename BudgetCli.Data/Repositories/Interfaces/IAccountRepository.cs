using System;
using System.Collections.Generic;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Models;
using BudgetCli.Util.Models;

namespace BudgetCli.Data.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<AccountDto>
    {
        long GetIdByName(string name);
        IEnumerable<long> GetChildAccountIds(long categoryId);

        bool DoesNameExist(string accountName);
        IEnumerable<AccountDto> GetAccounts(long? id = null, string nameContains = null, long? categoryId = null, string descriptionContains = null, Range<long> priorityRange = null, AccountKind? accountKind = null, Range<Money> fundsRange = null, bool includeClosedAccounts = false);
    }
}