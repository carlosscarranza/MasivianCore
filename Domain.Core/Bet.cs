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
        public Guid RouletteId { get; set; }
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("RouletteId")]
        public Roulette Roulette { get; set; }
        
    }
}