using System;
using System.IO;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories.Interfaces;
using BudgetCli.Util.Logging;

namespace BudgetCli.Data.Repositories
{
    public class CommandActionRepository : RepositoryBase<CommandActionDto>, ICommandActionRepository
    {
        public const string TABLE_NAME = "CommandAction";
        public CommandActionRepository(FileInfo dbInfo, ILog log) : base(dbInfo, log)
        {
        }

        public CommandActionRepository(string connectionString, ILog log) : base(connectionString, log)
        {
        }

        public override string GetTableName()
        {
            return TABLE_NAME;
        }
    }
}