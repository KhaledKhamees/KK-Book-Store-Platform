using Microsoft.EntityFrameworkCore;
using MVCProject2.Data;
using MVCProject2.Models;
using MVCProject2.Repository.IRepository;
using MVCProject2.Reprository.IReprository;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MVCProject2.Reprository.IRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplecationDBContext dBContext;
        internal DbSet<T> DbSet;
        public Repository(ApplecationDBContext applecationDBContext)
        {
            dBContext = applecationDBContext;
            this.DbSet = dBContext.Set<T>();
        }
        public void Add(T item)
        {
            DbSet.Add(item);
        }

        public T? Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query = DbSet;

            // Include specified properties
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            // Apply the filter
            query = tracked ? query.Where(filter) : query.AsNoTracking().Where(filter);

            // Return the first matching entity or null if none found
            return query.FirstOrDefault();
        }


        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }
        public void Remove(T item)
        {
            DbSet.Remove(item);
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            DbSet.RemoveRange(items);
        }
    }
}
