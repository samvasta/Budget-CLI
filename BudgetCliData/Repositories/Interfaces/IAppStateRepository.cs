using BudgetCliData.Models;

namespace BudgetCliData.Repositories.Interfaces
{
    public interface IAppStateRepository
    {
         long? GetLastExecutedTransactionId();
    }
}