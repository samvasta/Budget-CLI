using System.Transactions;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories.Interfaces;
using Dapper;

namespace BudgetCli.Data.Repositories
{
    public class RepositoryBag
    {
        public IAccountRepository AccountRepository { get; set; }
        public ITransactionRepository TransactionRepository { get; set; }
        public IAccountStateRepository AccountStateRepository { get; set; }
    }
}