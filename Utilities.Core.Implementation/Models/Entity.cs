using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilities.Core.Implementation.Models
{
    public abstract class Entity<TPrimaryKey>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TPrimaryKey Id { get; set; }

        public object[] GetKeys()
        {
            return new object[] { Id };
        }
    }
}
