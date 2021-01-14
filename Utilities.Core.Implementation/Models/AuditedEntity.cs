using System;

namespace Utilities.Core.Implementation.Models
{
    public abstract class AuditedEntity<TPrimaryKey> : CreationAuditedEntity<TPrimaryKey>
    {
        public virtual DateTime? LastModificationTime { get; set; }

        public virtual string? LastModifierUserId { get; set; }
    }
}
