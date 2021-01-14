using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Core.Implementation.Models;

namespace Domain.Core
{
    [Table("Bet")]
    public class Bet : FullAuditedEntity<Guid>
    {
        public double Amount { get; set; }
        public int NumberBet { get; set; }
        public string ColourBet { get; set; }
        public Guid UserId { get; set; }
        public ICollection<Roulette> Roulettes { get; set; }
        public ICollection<User> Users { get; set; }
    }
}