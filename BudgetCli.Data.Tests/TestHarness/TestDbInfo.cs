using System;
using System.IO;
using BudgetCli.Data.IO;

namespace BudgetCli.Data.Tests.TestHarness
{
    public class TestDbInfo : IDisposable
    {
        private string _filePath;
        public string ConnectionString { get; }

        internal TestDbInfo(string filePath)
        {
            _filePath = filePath;
            ConnectionString = DbHelper.GetSqLiteConnectionString(new FileInfo(filePath));
        }
        
        public void Dispose()
        {
            if(File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }
    }
}