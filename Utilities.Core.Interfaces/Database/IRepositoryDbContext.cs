using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Utilities.Core.Interfaces.Database
{
    public interface IRepositoryDbContext : IDisposable,
        IInfrastructure<IServiceProvider>,
        IDbContextDependencies,
        IDbSetCache,
        IDbContextPoolable
    {
    }
}
