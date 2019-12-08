using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories.Interfaces;

namespace BudgetCli.Data.Repositories
{
    public class RepositoryBag
    {
        public IAccountRepository AccountRepository { get; set; }
        public ITransactionRepository TransactionRepository { get; set; }
        public ICommandActionRepository CommandActionRepository { get; set; }
        public ICommandActionParameterRepository CommandActionParameterRepository { get; set; }
    }
}