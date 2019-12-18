using System;
using System.Collections.Generic;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Models;

namespace BudgetCli.Data.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<AccountDto>
    {
        long GetIdByName(string name);
        IEnumerable<long> GetChildAccountIds(long categoryId);
    }
}