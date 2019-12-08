using System;
using System.IO;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories.Interfaces;
using BudgetCli.Util.Logging;

namespace BudgetCli.Data.Repositories
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