using BudgetCliData.Repositories;
using Xunit;
using Moq;
using BudgetCliUtil.Logging;

namespace BudgetCliDataTest.Repositories
{
    public class TableNameTests
    {
        [Fact]
        public void TestTableNames()
        {
            Mock<ILog> log = new Mock<ILog>();
            string connectionString = "DataSource=:memory:;Version=3;New=True;";

            Assert.Equal("Account", (new AccountRepository(connectionString, log.Object)).GetTableName());
            Assert.Equal("CommandAction", (new CommandActionRepository(connectionString, log.Object)).GetTableName());
            Assert.Equal("CommandActionParameter", (new CommandActionParameterRepository(connectionString, log.Object)).GetTableName());
            Assert.Equal("Transaction", (new TransactionRepository(connectionString, log.Object)).GetTableName());
        }
    }
}