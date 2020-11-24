using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutsXAM.Data.Repos
{
    interface IRepository<T> where T : class, new()
    {
        Task<List<T>> Get();
        Task<T> Get(int id);
        Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate);
        Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null);
        Task<T> Get(Expression<Func<T, bool>> predicate);
        AsyncTableQuery<T> AsQueryable();
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
