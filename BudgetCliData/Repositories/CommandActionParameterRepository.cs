using System;
using System.IO;
using BudgetCliData.Models;
using BudgetCliData.Repositories.Interfaces;
using BudgetCliUtil.Logging;

namespace BudgetCliData.Repositories
{
    public class CommandActionParameterRepository : RepositoryBase<CommandActionParameterDto>, ICommandActionParameterRepository
    {
        public const string TABLE_NAME = "CommandActionParameter";

        public CommandActionParameterRepository(FileInfo dbInfo, ILog log) : base(dbInfo, log)
        {
        }

        public CommandActionParameterRepository(string connectionString, ILog log) : base(connectionString, log)
        {
        }

        public override string GetTableName()
        {
            return TABLE_NAME;
        }
    }
}