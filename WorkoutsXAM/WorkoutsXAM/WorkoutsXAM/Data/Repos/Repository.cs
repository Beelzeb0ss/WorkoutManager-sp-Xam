using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WorkoutsXAM.Models;

namespace WorkoutsXAM.Data.Repos
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private SQLiteAsyncConnection db;

        public Repository(SQLiteAsyncConnection database)
        {
            db = database;
        }

        public AsyncTableQuery<T> AsQueryable() => db.Table<T>();

        public async Task<List<T>> Get() => await db.GetAllWithChildrenAsync<T>(); //db.Table<T>().ToListAsync();

        public async Task<T> Get(int id) => await db.FindWithChildrenAsync<T>(id);

        public async Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate) => await db.GetAllWithChildrenAsync<T>(filter: predicate);

        public async Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            var query = db.Table<T>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = query.OrderBy<TValue>(orderBy);

            return await query.ToListAsync();
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate) => await db.FindWithChildrenAsync<T>(predicate);

        public async Task Delete(T entity) => await db.DeleteAsync(entity);

        public async Task Insert(T entity) => await db.InsertWithChildrenAsync(entity);

        public async Task Update(T entity) => await db.UpdateWithChildrenAsync(entity);
    }
}
