using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Applications.Products
{
    public interface IGenericRepository<T> where T : class
    {
        T? GetById(int id);
        IEnumerable<T> GetAll();
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        bool Exist(Expression<Func<T, bool>> predicate);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
    }
}
