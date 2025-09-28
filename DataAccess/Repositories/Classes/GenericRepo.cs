using DataAccess.Data;
using DataAccess.Models.Shared;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace DataAccess.Repositories.Classes
{

    public class GenericRepo<T>(ApplicationDbContext dbContext) : IGenericRepo<T> where T : BaseEntity
    {
        //Get All
        public IEnumerable<T> GetAll(bool WithTracking = false)
        {
            if (WithTracking)
            {
                return dbContext.Set<T>().Where(E => E.IsDeleted != true).ToList();
            }
            else
            {
                return dbContext.Set<T>().Where(E => E.IsDeleted != true).AsNoTracking().ToList();
            }
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector)
        {
            return dbContext.Set<T>().Where(E => E.IsDeleted != true).Select(selector).ToList();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return dbContext.Set<T>().Where(E => E.IsDeleted != true).Where(predicate).ToList();
        }

        //Get By Id
        public T? GetById(int id) => dbContext.Set<T>().Find(id);

        //Update
        public void Update(T entity)
        {
            dbContext.Set<T>().Update(entity);          
        }

        //Delete
        public void Remove(T entity)
        {
            dbContext.Set<T>().Remove(entity);  
        }

        //Insert
        public void Add(T entity)
        {
            dbContext.Set<T>().Add(entity);
        }

    }
}
