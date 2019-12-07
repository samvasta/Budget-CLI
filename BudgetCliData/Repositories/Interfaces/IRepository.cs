using System.Collections.Generic;
using BudgetCliData.Models;

namespace BudgetCliData.Repositories.Interfaces
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