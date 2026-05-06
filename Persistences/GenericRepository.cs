using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Applications.Products;
using Microsoft.EntityFrameworkCore;

namespace Persistences
{
    public class GenericRepository<T>(AppDbContext dbContext) : IGenericRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet = dbContext.Set<T>();

        protected AppDbContext Context = dbContext;

        public T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T Add(T entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public bool Exist(Expression<Func<T, bool>> predicate)
        {
            // action =>  parameters + void
            // predicate => parameter + bool
            // Func<T,T> => parameters + T
            return _dbSet.Any(predicate);
        }


        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
    }
}