using DataAccess.Data;
using DataAccess.Models;
using DataAccess.Models.Shared;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        //=============== Lazy Implementation ================//
        private readonly ApplicationDbContext dbContext;
        private readonly Lazy<IProjectRepo> projectRepo;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            projectRepo = new Lazy<IProjectRepo>(valueFactory: () => new ProjectRepo(dbContext));
        }
        public IProjectRepo ProjectRepo => projectRepo.Value;
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
