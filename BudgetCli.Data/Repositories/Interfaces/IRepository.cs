using System.Collections.Generic;
using BudgetCli.Data.Models;

namespace BudgetCli.Data.Repositories.Interfaces
{
    public interface IRepository<T> where T : IDbModel
    {
        IEnumerable<T> GetAll();
        T GetById(long id);
        long GetNextId();

        bool Upsert(T data);

        bool Remove(T data);
        bool RemoveById(long id);
    }
}