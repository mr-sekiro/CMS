using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IProjectRepo ProjectRepo { get; }
        int SaveChanges();
    }
}
