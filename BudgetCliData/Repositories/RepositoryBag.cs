using BudgetCliData.Models;
using BudgetCliData.Repositories.Interfaces;

namespace BudgetCliData.Repositories
{
    public class RepositoryBag
    {
        public IAccountRepository AccountRepository { get; set; }
        public ITransactionRepository TransactionRepository { get; set; }
        public ICommandActionRepository CommandActionRepository { get; set; }
        public ICommandActionParameterRepository CommandActionParameterRepository { get; set; }
    }
}