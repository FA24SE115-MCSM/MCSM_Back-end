﻿using System.Linq.Expressions;

namespace MCSM_Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        T GetById(Guid id);

        IQueryable<T> GetMany(Expression<Func<T, bool>> predicate);

        IQueryable<T> SkipAndTake(int skip, int take);

        int Count();

        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        void Update(T entity);

        void UpdateRange(IEnumerable<T> entities);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        bool Contains(Expression<Func<T, bool>> predicate);

        bool Any(Expression<Func<T, bool>> predicate);
        Task<bool> ContainsAsync(Expression<Func<T, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}
