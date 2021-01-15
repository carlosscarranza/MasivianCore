#nullable enable
using System;

namespace Utilities.Core.Implementation.Models
{
    public abstract class FullAuditedEntity<TPrimaryKey> : AuditedEntity<TPrimaryKey>
    {
        public virtual bool IsDeleted { get; set; }

        public virtual string? DeleterUserId { get; set; }

        public virtual DateTime? DeletionTime { get; set; }
    }
}
