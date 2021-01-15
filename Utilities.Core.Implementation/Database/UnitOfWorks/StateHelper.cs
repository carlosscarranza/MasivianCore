using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Utilities.Core.Implementation.Database.UnitOfWorks
{
    public class StateHelper
    {
        public static EntityState ConvertState(EntityState state)
        {
            return state switch
            {
                EntityState.Detached => EntityState.Unchanged,
                EntityState.Unchanged => EntityState.Unchanged,
                EntityState.Added => EntityState.Added,
                EntityState.Deleted => EntityState.Deleted,
                EntityState.Modified => EntityState.Modified,
                _ => throw new ArgumentOutOfRangeException(nameof(state))
            };
        }
    }
}
