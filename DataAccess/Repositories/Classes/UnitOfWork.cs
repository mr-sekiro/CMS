using DataAccess.Data;
using DataAccess.Models.Shared;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        //=============== Lazy Implementation ================//
        private readonly ApplicationDbContext dbContext;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public int SaveChanges()
        {
            foreach (var entry in dbContext.ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    entry.Entity.DateModified = DateTime.Now;
            }

            return dbContext.SaveChanges();
        }

        public void Dispose() => dbContext.Dispose();
    }
}
