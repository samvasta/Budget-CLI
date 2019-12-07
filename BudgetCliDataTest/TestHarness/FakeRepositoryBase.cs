using System.IO;
using BudgetCliData.Repositories;
using BudgetCliUtil.Logging;

namespace BudgetCliDataTest.TestHarness
{
    public class FakeRepositoryBase : RepositoryBase<FakeDto>
    {
        public const string TABLE_NAME = "Data";
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