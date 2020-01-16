using System.IO;
using BudgetCli.Data.Repositories;
using BudgetCli.Util.Logging;

namespace BudgetCli.Data.Util
{
    public static class RepositoryBagUtil
    {
        public static RepositoryBag GetRuntimeRepositoryBag(FileInfo dbInfo, ILog log)
        {
            return new RepositoryBag()
            {
                AccountRepository = new AccountRepository(dbInfo, log),
                AccountStateRepository = new AccountStateRepository(dbInfo, log),
                TransactionRepository = new TransactionRepository(dbInfo, log),
            };
        }
    }
}