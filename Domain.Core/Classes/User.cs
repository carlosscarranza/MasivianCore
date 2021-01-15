using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Core.Implementation.Models;

namespace Domain.Core.Classes
{
    [Table("User")]
    public class User : FullAuditedEntity<Guid>
    {
        [Required]
        public string Name { get; set; }
        public double Score { get; set; }
        public double Credit { get; set; }
        public ICollection<Bet> Bets { get; set; }
    }
}