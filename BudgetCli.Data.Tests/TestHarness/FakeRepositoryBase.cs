using System.IO;
using BudgetCli.Data.Repositories;
using BudgetCli.Util.Logging;

namespace BudgetCli.Data.Tests.TestHarness
{
    public class FakeRepositoryBase : RepositoryBase<FakeDto>
    {
        public static string TABLE_NAME { get { return "Data"; } }
        
        public FakeRepositoryBase(FileInfo dbInfo, ILog log) : base(dbInfo, log)
        {
        }

        public FakeRepositoryBase(string connectionString, ILog log) : base(connectionString, log)
        {
        }

        public override string GetTableName()
        {
            return TABLE_NAME;
        }

        /// <summary>
        /// Exposes the <see cref="RepositoryBase.Insert"/> method for direct testing
        /// </summary>
        public bool InsertProxy(FakeDto dto)
        {
            return base.Insert(dto);
        }

        /// <summary>
        /// Exposes the <see cref="RepositoryBase.Update"/> method for direct testing
        /// </summary>
        public bool UpdateProxy(FakeDto dto)
        {
            return base.Update(dto);
        }
    }
}