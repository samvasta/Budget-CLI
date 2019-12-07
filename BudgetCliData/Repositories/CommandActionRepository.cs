using System;
using System.IO;
using BudgetCliData.Models;
using BudgetCliData.Repositories.Interfaces;
using BudgetCliUtil.Logging;

namespace BudgetCliData.Repositories
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