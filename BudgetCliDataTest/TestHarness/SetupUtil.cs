using System;
using System.Data.SQLite;
using System.IO;
using System.Runtime.CompilerServices;
using BudgetCliData.IO;
using BudgetCliUtil.Logging;
using DbUp.SQLite.Helpers;
using Moq;
using Dapper;

namespace BudgetCliDataTest.TestHarness
{
    public static class SetupUtil
    {

        /// <summary>
        /// Creates an in-memory database and updates it with all available upgrade scripts
        /// <p>IMPORTANT: Remember to close out the database when finished!!</p>
        /// </summary>
        public static InMemorySQLiteDatabase CreateInMemoryDb()
        {
            var log = new Mock<ILog>();
            var db = new InMemorySQLiteDatabase();

            DbHelper.UpgradeSQLiteFile(db.ConnectionString, log.Object);

            return db;
        }
        
        /// <summary>
        /// Creates an in-memory database and updates it with all available upgrade scripts
        /// </summary>
        public static TestDbInfo CreateTestDb([CallerMemberName] string dbName = "testDb")
        {
            var log = new Mock<ILog>();

            string path = Path.Combine(Environment.CurrentDirectory, "Test Files");
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(path, dbName + ".data");
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            DbHelper.CreateSQLiteFile(filePath, log.Object);

            return new TestDbInfo(filePath);
        }

        
        
        /// <summary>
        /// Creates an in-memory database for testing with FakeDto
        /// </summary>
        public static TestDbInfo CreateFakeDb([CallerMemberName] string dbName = "fakeDb")
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Test Files");
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(path, dbName + ".data");
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            SQLiteConnection.CreateFile(filePath);
            using(var connection = new SQLiteConnection(DbHelper.GetSqLiteConnectionString(filePath)))
            {
                string addTableCommand = $@"CREATE TABLE [{FakeRepositoryBase.TABLE_NAME}] ( Id INTEGER NOT NULL PRIMARY KEY, 'Name' TEXT NOT NULL, 'Description' TEXT);";
                connection.Execute(addTableCommand);
            }

            return new TestDbInfo(filePath);
        }
    }
}