using System.IO;
using System.Reflection;
using System;
using Xunit;
using BudgetCli.Data.IO;
using Moq;
using BudgetCli.Util.Logging;
using BudgetCli.Data.Repositories;
using System.Data.SQLite;
using Dapper;

namespace BudgetCli.Data.Tests.IO
{
    public class DbHelperTest
    {
        [Fact]
        public void TestCreateFile()
        {
            var log = new Mock<ILog>();
            
            string path = Path.Combine(Environment.CurrentDirectory, "Test Files");
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(path, "Test.data");
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            DbHelper.CreateSQLiteFile(filePath, log.Object);
            Assert.True(File.Exists(filePath));

            AssertHasTable(filePath, AccountRepository.TABLE_NAME);
            AssertHasTable(filePath, AccountStateRepository.TABLE_NAME);
            AssertHasTable(filePath, TransactionRepository.TABLE_NAME);

            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private void AssertHasTable(string filePath, string tableName)
        {
            using(var connection = new SQLiteConnection(DbHelper.GetSqLiteConnectionString(new FileInfo(filePath))))
            {
                string command = $@"SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='{tableName}';";
                Assert.True(connection.ExecuteScalar<long>(command) == 1, $"Database is missing table called {tableName}");
            }
        }
    }
}