using System.Threading;
using System.Threading.Tasks;
using Utilities.Core.Interfaces.Database.Repositories;

namespace Utilities.Core.Interfaces.Database.UnitOfWorks
{
    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class;
    }
}
