using System.Drawing;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using BudgetCliData.Models;
using BudgetCliUtil.Logging;
using Dapper;
using DbUp;
using DbUp.Engine;
using DbUp.SQLite.Helpers;

namespace BudgetCliData.IO
{
    public static class DbHelper
    {
        public static void CreateSQLiteFile(string fileLocation, ILog log)
        {
            log.WriteLine($"Created file at {fileLocation}");
            SQLiteConnection.CreateFile(fileLocation);
            UpgradeSQLiteFile(new FileInfo(fileLocation), log);
        }

        public static bool UpgradeSQLiteFile(FileInfo file, ILog log)
        {
            string connectionString = GetSqLiteConnectionString(file);
            return UpgradeSQLiteFile(connectionString, log);
        }
        
        public static bool UpgradeSQLiteFile(string connectionString, ILog log)
        {
            var upgrader = DeployChanges.To
                                        .SQLiteDatabase(connectionString)
                                        .WithScriptsEmbeddedInAssembly(Assembly.GetAssembly(typeof(DbHelper)))
                                        .LogToNowhere()
                                        .Build();

            return FinishUpgrade(upgrader, log);
        }

        public static bool UpgradeSQLiteFile(SharedConnection sharedConnection, ILog log)
        {
            var upgrader = DeployChanges.To
                                        .SQLiteDatabase(sharedConnection)
                                        .WithScriptsEmbeddedInAssembly(Assembly.GetAssembly(typeof(DbHelper)))
                                        .LogToNowhere()
                                        .Build();

            return FinishUpgrade(upgrader, log);
        }

        private static bool FinishUpgrade(UpgradeEngine upgrader, ILog log)
        {
            var toExecute = upgrader.GetScriptsToExecute();
            log.WriteLine("Upgrading File...");
            log.WriteLine($"Applying {toExecute.Count} scripts.");
            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                log.WriteLine("Upgrage failed!", Color.Red);
                log.WriteLine(result.Error.ToString(), Color.Red);
                return false;
            }

            log.WriteLine("Upgrade successful!");

            foreach(SqlScript script in result.Scripts)
            {
                log.WriteLine($"Applied Upgrade Script {script.Name}.");
            }

            log.WriteLine();

            return true;
        }

        public static string GetSqLiteConnectionString(string filePath)
        {
            return GetSqLiteConnectionString(new FileInfo(filePath));
        }

        public static string GetSqLiteConnectionString(FileInfo fInfo)
        {
            return $"Data Source=\"{fInfo.FullName}\";";
        }
    }
}