using System;
using System.Collections.Generic;
using BudgetCli.Data.Models;

namespace BudgetCli.Data.Repositories.Interfaces
{
    public interface IAccountStateRepository : IRepository<AccountStateDto>
    {
        AccountStateDto GetLatestByAccountId(long accountId);
        AccountStateDto GetLatestByAccountId(long accountId, DateTime date);
        
         List<AccountStateDto> GetAllByAccountId(long accountId);
         
         bool RemoveAllByAccountId(long accountId);

        /// <summary>
        /// Adds an account state with the IsClosed flag set to true
        /// </summary>
        /// <returns></returns>
         bool CloseAccount(long accountId);

        /// <summary>
        /// Adds an account state with the IsClosed flag set to false.
        /// </summary>
         bool ReOpenAccount(long accountId);
    }
}