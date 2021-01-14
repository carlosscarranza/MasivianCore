using System;

namespace Utilities.Core.Implementation.Models
{
    public abstract class CreationAuditedEntity<TPrimaryKey> : Entity<TPrimaryKey>
    {
        public virtual DateTime CreationTime { get; set; }

        public virtual string? CreatorUserId { get; set; }

        protected CreationAuditedEntity()
        {
            CreationTime = DateTime.Now;
        }
    }
}
