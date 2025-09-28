using DataAccess.Models.Shared;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Interfaces
{
    public interface IGenericRepo<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll(bool WithTracking = false);
        IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate);
        T? GetById(int id);
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
    }
}
