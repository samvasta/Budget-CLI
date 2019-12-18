using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System;
using System.Collections.Generic;
using BudgetCli.Data.Models;
using BudgetCli.Data.IO;
using System.Data.SqlClient;
using Dapper;
using BudgetCli.Data.Repositories.Interfaces;
using System.Data.SQLite;
using BudgetCli.Data.Util;
using BudgetCli.Util.Logging;

namespace BudgetCli.Data.Repositories
{
    public abstract class RepositoryBase <T> : IRepository<T> where T : IDbModel
    {
        internal delegate void DatabaseOperation(SQLiteConnection connection);

        public ILog Log { get; }
        public string ConnectionString { get; }

        protected RepositoryBase(FileInfo dbInfo, ILog log)
            : this(DbHelper.GetSqLiteConnectionString(dbInfo), log)
        {
            //intentionally left blank
        }
        protected RepositoryBase(string connectionString, ILog log)
        {
            ConnectionString = connectionString;
            Log = log;
        }

        public virtual IEnumerable<T> GetAll()
        {
            List<T> list = new List<T>();
            Execute((con) =>
            {
                string command = $@"SELECT * FROM [{GetTableName()}];";
                list.AddRange(con.Query<T>(command));
            });
            return list;
        }

        public virtual T GetById(long id)
        {
            T item = default(T);
            Execute((con) =>
            {
                string command = $@"SELECT * FROM [{GetTableName()}] WHERE Id = @Id;";
                object parameter = new { Id = id };
                item = con.QueryFirstOrDefault<T>(command, parameter);
            });
#if DEBUG
            if(item == null)
            {
                LogError($"Retrieval of row by Id failed because row does not exist! (Id = {id})");
            }
#endif
            return item;
        }

        public virtual long GetNextId()
        {
            long maxId = 0;
            Execute((con) =>
            {
                string command = $@"SELECT MAX(Id) FROM [{GetTableName()}];";
                maxId = con.ExecuteScalar<long>(command);
            });
            return maxId + 1;
        }

        /// <summary>
        /// Inserts objects with ID = null or Updates objects with ID != null
        /// </summary>
        /// <returns>true if operation was successful, false otherwise</returns>
        public virtual bool Upsert(T data)
        {
            if(data.Id.HasValue)
            {
                return Update(data);
            }
            else
            {
                return Insert(data);
            }
        }

        protected virtual bool Insert(T data)
        {
            long? originalId = data.Id;
            if(!data.Id.HasValue)
            {
                data.Id = GetNextId();
            }
            else
            {
#if DEBUG
                LogError($"Attempted to insert an object that already has an Id! (Id = {data.Id})");
#endif
                return false;
            }

            try
            {
                Execute((connection) =>
                {
                    var propertyToColNameDict = AttributeUtil.GetPersistedPropertyToColumnNameMap<T>();

                    string colNames = string.Join(", ", propertyToColNameDict.Values.Select(x => $"[{x}]"));
                    string parameterNames = string.Join(", ", propertyToColNameDict.Values.Select(x => $"@{x}"));

                    string command = $@"INSERT INTO [{GetTableName()}] ( {colNames} ) VALUES ( {parameterNames} );";
                    connection.Execute(command, data);
                });
            }
            catch(SQLiteException e)
            {
#if DEBUG
                LogException(e);
#endif
                //Need to revert because it doesn't exist in DB.
                //If id was kept, it would be possible for two DTOs to have the same id
                data.Id = originalId;
                return false;
            }
            return true;
        }

        protected virtual bool Update(T data)
        {
            if(!data.Id.HasValue)
            {
#if DEBUG
                LogError("Update was called for object with no Id!");
#endif
                return false;
            }

            Execute((connection) =>
            {
                var propertyToColNameDict = AttributeUtil.GetPersistedPropertyToColumnNameMap<T>();

                string setStatements = string.Join(", ", propertyToColNameDict.Select(kvp => $"{kvp.Value} = @{kvp.Key}"));

                string command = $@"UPDATE [{GetTableName()}] SET {setStatements} WHERE Id = @Id;";
                connection.Execute(command, data);
            });
            return true;
        }

        public virtual bool Remove(T data)
        {
            if(data == null)
            {
#if DEBUG
                LogError("Remove was called for a null object!");
#endif
                return false;
            }
            if(data.Id.HasValue)
            {
                return RemoveById(data.Id.Value);
            }
#if DEBUG
            LogError("Remove was called for object with no Id!");
#endif
            return false;
        }

        public virtual bool RemoveById(long id)
        {
            bool exists = false;
            Execute((con) =>
            {
                object parameter = new { TableName = GetTableName(), Id = id };
                string existsCommand = $@"SELECT EXISTS (SELECT 1 FROM [{GetTableName()}] WHERE Id = @Id);";
                
                exists = con.ExecuteScalar<bool>(existsCommand, parameter);

                if(exists)
                {
                    string command = $@"DELETE FROM [{GetTableName()}] WHERE Id = @Id;";
                    con.Execute(command, parameter);
                }

            });
#if DEBUG
            if(!exists)
            {
                LogError($"Remove was called for object that does not exist in the table! (Id = {id})");
            }
#endif
            return exists;
        }

        public abstract string GetTableName();

        /// <summary>
        /// Wraps a database operation inside an SqlConnection using block
        /// </summary>
        internal void Execute(DatabaseOperation op)
        {
            using(SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                op(connection);
                connection.Close();
            }
        }

        protected void LogException(Exception e)
        {
            Log?.WriteLine(e.ToString(), LogLevel.Error);
        }

        protected void LogError(String errorText)
        {
            Log?.WriteLine(errorText, LogLevel.Error);
            Log?.WriteLine($"\tTable = {GetTableName()}", LogLevel.Error);
        }
    }
}