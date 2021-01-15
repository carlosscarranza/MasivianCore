using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Core.Implementation.Models;

namespace Domain.Core
{
    [Table("User")]
    public class User : FullAuditedEntity<Guid>
    {
        public string Name { get; set; }
        public double Score { get; set; }
        public double Credit { get; set; }
        public ICollection<Bet> Bets { get; set; }
    }
}