using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MVCProject2.Repository.IRepository;

namespace MVCProject2.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null , string? includeProperties = null);
        T Get(Expression<Func<T, bool>> filter , string? includeProperties = null,bool traked = false);
        void Add(T item);
        void Remove(T item);
        void RemoveRange(IEnumerable<T> items);
    }
}
