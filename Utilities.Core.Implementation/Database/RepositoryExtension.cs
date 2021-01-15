using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Utilities.Core.Implementation.Database.Repositories;
using Utilities.Core.Implementation.Database.UnitOfWorks;
using Utilities.Core.Interfaces.Database;
using Utilities.Core.Interfaces.Database.Repositories;
using Utilities.Core.Interfaces.Database.UnitOfWorks;

namespace Utilities.Core.Implementation.Database
{
    public static class RepositoryExtension
    {
        public static void UseRepository(this IServiceCollection services, Type dbContextType)
        {
            services.AddScoped(typeof(DbContext), dbContextType);
            services.AddScoped(typeof(IUnitOfWorkAsync), typeof(UnitOfWork));
            services.AddScoped(typeof(IRepositoryAsync<>), typeof(Repository<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}