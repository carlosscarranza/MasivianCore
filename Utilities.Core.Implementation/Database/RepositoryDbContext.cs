using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Utilities.Core.Interfaces.Database;

namespace Utilities.Core.Implementation.Database
{
    public class RepositoryDbContext : DbContext, IRepositoryDbContext
    {
        public RepositoryDbContext(
            DbContextOptions options
        )
            : base(options)
        {

        }
    }
}
